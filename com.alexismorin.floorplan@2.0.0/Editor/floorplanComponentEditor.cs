using System.Collections;
using UnityEditor;
using UnityEngine;
using alexism.Floorplan.Core.Components;
using alexism.Floorplan.Core.Enums;

using System;
using System.Collections.Generic;

namespace alexism.Floorplan.Core.editor
{
    [CustomEditor(typeof(floorplanComponent))]
    public class floorplanComponentEditor : Editor
    {
        floorplanComponent script;

        public void OnEnable()
        {
        }
        void TileTypeChange(object newTileType)
        {
            script.ChangeComponentType((GameObject)newTileType);
        }
        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();
            script = (floorplanComponent)target;

            if (EditorGUILayout.DropdownButton(new GUIContent("Tile type"), FocusType.Passive))
            {

                GenericMenu menu = new GenericMenu();
                foreach (TileTypes tileType in (TileTypes[])Enum.GetValues(typeof(TileTypes)))
                {
                    if (tileType == TileTypes.None)
                        continue;
                    foreach (GameObject tile in script.getTilesFromType(tileType))
                    {
                        menu.AddItem(new GUIContent(tileType.ToString()+"/"+tile.name), false,TileTypeChange,tile);
                    }
                }
                menu.ShowAsContext();
            }
        }
    }
}