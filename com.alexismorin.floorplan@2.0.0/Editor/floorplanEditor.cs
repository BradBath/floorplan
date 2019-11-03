using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using alexism.Floorplan.Core.Enums;
namespace alexism.Floorplan.Core.editor
{
    [CustomEditor(typeof(floorplan))]
    public class floorplanEditor : Editor
    {
        floorplan script;

        Vector3 mouseStart;
        Vector3 mouseEnd;

        int drawToolInt = 0;
        int drawObjetInt = 0;
        int selectedMaterial = 0;

        GUIContent[] drawToolStrings;
        Tool currentTool;
        Tool[] tools = { new RectangleStrat(), new RectangleFilledStrat() };
        GUIContent[] drawItemTextures;
        Material[] materials;

        GUIStyle TitleLabels;

        Vector2 scrollPos;

        List<GameObject> placingBuffer;

        void print(object print)
        {
            Debug.Log(print);
        }

        public void OnEnable()
        {
            drawToolStrings = new GUIContent[]{
                new GUIContent("0",Resources.Load<Texture>("icons/RectTool"),"Rectangle"),
                new GUIContent("1",Resources.Load<Texture>("icons/FilledRectTool"),"Filled rectangle")
            };
            drawItemTextures = new GUIContent[] {
                new GUIContent("0",Resources.Load<Texture>("icons/BrickIcon"),"Wall"),
                new GUIContent("1",Resources.Load<Texture>("icons/PillarIcon"),"Pillar"),
                new GUIContent("2",Resources.Load<Texture>("icons/FloorIcon"),"Floor")
            };
            drawToolInt = EditorPrefs.GetInt("DrawTool", 0);
            drawObjetInt = EditorPrefs.GetInt("DrawObjet", 0);
            selectedMaterial = EditorPrefs.GetInt("SelectedMaterial", 0);
        }
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            //Display drawing tools

            TitleLabels = new GUIStyle()
            {
                fontStyle = FontStyle.Normal
            };
            TitleLabels.normal.textColor = Color.white;
            script = (floorplan)target;

            //Saving certain variables (selected draw tool, selected object, selected material)
            EditorPrefs.SetInt("DrawTool", drawToolInt);
            EditorPrefs.SetInt("DrawObjet", drawObjetInt);
            EditorPrefs.SetInt("SelectedMaterial", selectedMaterial);

            //Generate toolbars for the drawing tools and objects.
            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.BeginHorizontal("Box");
                {
                    GUILayout.Label("Drawing tools", TitleLabels, GUILayout.Height(16), GUILayout.Width(96));
                    drawToolInt = GUILayout.Toolbar(drawToolInt, drawToolStrings, GUILayout.Width(48 * drawToolStrings.Length), GUILayout.Height(48));

                    GUILayout.Label("Objects", TitleLabels, GUILayout.Height(16), GUILayout.Width(64));
                    drawObjetInt = GUILayout.Toolbar(drawObjetInt, drawItemTextures, GUILayout.Width(48 * drawItemTextures.Length), GUILayout.Height(48));
                    currentTool = tools[drawToolInt];
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.LabelField("Materials", TitleLabels);
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(250), GUILayout.Height(64));
                {
                    //Generate material selection
                    materials = script.wallMaterials;
                    GUIContent[] materialPreviews = new GUIContent[materials.Length];
                    for (int m = 0; m < materials.Length; m++)
                    {
                        if (materials[m] != null)
                            materialPreviews[m] = new GUIContent(m.ToString(),materials[m].mainTexture, materials[m].name);
                    }
                    selectedMaterial = GUILayout.Toolbar(selectedMaterial, materialPreviews, GUILayout.Width(50 * materials.Length), GUILayout.Height(50));
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }

        public static Vector3 snap(Vector3 pos, int v)
        {
            float x = pos.x;
            float y = pos.y;
            float z = pos.z;
            x = Mathf.RoundToInt(x / v) * v;
            y = Mathf.RoundToInt(y / v) * v;
            z = Mathf.RoundToInt(z / v) * v;
            return new Vector3(x, y, z);
        }

        public void GetMousePosition(Vector3 MouseScreenPosition,out Vector3 MouseWorldPosition)
        {
            MouseWorldPosition = Vector3.zero; //initialize MouseWorldPosition as Vector3.zero
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                MouseWorldPosition = snap(hit.point, (int)script.tileSize);
                MouseWorldPosition.y = hit.point.y;
            }
        }



        void OnSceneGUI()
        {
            floorplan script = (floorplan)target;
            if(mouseEnd!=Vector3.zero && mouseStart != Vector3.zero)
            {
                currentTool.RenderPreview();
            }
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Keyboard));
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.KeyDown:
                    {
                        break;
                    }
                case EventType.MouseDown:
                    {
                        if (Event.current.button == 1)
                            return;
                        GetMousePosition(Event.current.mousePosition, out mouseStart);
                        currentTool.MouseDown(mouseStart);
                        break;
                    }
                case EventType.MouseDrag:
                    {
                        if (Event.current.button == 1 || mouseStart == Vector3.zero)
                            return;
                        GetMousePosition(Event.current.mousePosition, out mouseEnd);
                        currentTool.MouseDrag(mouseEnd);
                        break;
                    }
                case EventType.MouseUp:
                    {
                        if (mouseEnd == Vector3.zero || mouseStart == Vector3.zero)
                            return;
                        GetMousePosition(Event.current.mousePosition, out mouseEnd);
                        currentTool.MouseUp(mouseEnd, (TileTypes)drawObjetInt,script,materials[selectedMaterial]);
                        mouseEnd = Vector3.zero;
                        break;
                    }
                case EventType.ExecuteCommand:
                    {
                        Debug.Log(Event.current.commandName);
                        break;
                    }
            }
        }
    }
}