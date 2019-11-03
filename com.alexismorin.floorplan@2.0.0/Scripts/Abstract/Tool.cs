using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using alexism.Floorplan.Core.Enums;
using alexism.Floorplan.Core;
//Implement strategy pattern for drawing tools 


public abstract class Tool
{
    public abstract void MouseDown(Vector3 mousePos);
    public abstract void MouseDrag(Vector3 mousePos);
    public abstract void MouseUp(Vector3 mousePos,TileTypes tileType, floorplan script,Material mat);

    public abstract void RenderPreview();
}


//Some classes that inherit Tool. I should probably put these into their own separate files.

public class RectangleFilledStrat : Tool
{
    float width;
    float height;
    Vector3 mouseStart;
    Vector3 mouseEnd;

    public void Render(GameObject tile, floorplan script,Material mat)
    {
        GameObject gO = new GameObject("Floor");
        gO.transform.parent = GameObject.Find("New Floorplan Geometry").transform;
        Vector3 topLeft = new Vector3(Mathf.Max(mouseStart.x, mouseEnd.x), mouseStart.y, Mathf.Max(mouseStart.z, mouseEnd.z));
        for (int y = 0; y < Mathf.Abs(height); y += (int)script.tileSize)
        {
            for (int x = 0; x < Mathf.Abs(width); x += (int)script.tileSize)
            {
                GameObject floor=script.createInstance(tile, (topLeft - new Vector3(x, 0, y + script.tileSize)), Quaternion.identity);
                floor.GetComponent<Renderer>().material = mat;
                floor.transform.parent.parent = gO.transform;
            }
        }
        Undo.RegisterCreatedObjectUndo(gO,"Undo floor creation");
    }

    public override void MouseDown(Vector3 mousePos)
    {
        mouseStart = mousePos;
    }

    public override void MouseDrag(Vector3 mousePos)
    {
        width = -(mouseEnd.x - mouseStart.x);
        height = -(mouseEnd.z - mouseStart.z);
        mouseEnd = mousePos;
    }

    public override void MouseUp(Vector3 mousePos, TileTypes tileType, floorplan script, Material mat)
    {
        Vector3[] points = new Vector3[]
        {
            mouseStart,
            mouseEnd,
            mouseStart-(new Vector3(width,0,0)),
            mouseEnd+(new Vector3(width,0,0))
        };

        switch (tileType)
        {
            case TileTypes.Wall:
                break;
            case TileTypes.Floor:
                Render(script.tileset.floorTiles[0],script,mat);
                break;
            case TileTypes.Pillar:
                break;
        }

        mouseStart = Vector3.zero;
        mouseEnd = Vector3.zero;
    }

    public override void RenderPreview()
    {
        Handles.color = Color.red;
        Handles.DrawWireCube(mouseStart - new Vector3(width / 2, 0, height / 2), new Vector3(width, 2, height));
    }
}


public class RectangleStrat : Tool
{
    float width;
    float height;
    Vector3 mouseStart;
    Vector3 mouseEnd;
    void Render(floorplan script, GameObject tile,Material mat)
    {
        GameObject gO = new GameObject("Walls");
        gO.transform.parent = GameObject.Find("New Floorplan Geometry").transform;
        Vector3 topLeft = new Vector3(Mathf.Max(mouseStart.x, mouseEnd.x), mouseStart.y, Mathf.Max(mouseStart.z, mouseEnd.z));
        Vector3 bottomLeft = new Vector3(Mathf.Min(mouseStart.x, mouseEnd.x), mouseStart.y, Mathf.Min(mouseStart.z, mouseEnd.z));

        for (int x = 0; x < Mathf.Abs(width); x += (int)script.tileSize)
        {
            GameObject wall = script.createInstance(tile, (topLeft - new Vector3(x + script.tileSize, 0, 0)), Quaternion.LookRotation(Vector3.right, Vector3.up));
            wall.GetComponent<Renderer>().material = mat;
            wall.transform.parent.parent = gO.transform;

            wall = script.createInstance(tile, (bottomLeft - new Vector3(-x, 0, 0)), Quaternion.LookRotation(Vector3.right, Vector3.up));
            wall.GetComponent<Renderer>().material = mat;
            wall.transform.parent.parent = gO.transform;
        }
        for (int z = 0; z < Mathf.Abs(height); z += (int)script.tileSize)
        {
            GameObject wall = script.createInstance(tile, (topLeft - new Vector3(0, 0, z + script.tileSize)), Quaternion.identity);
            wall.GetComponent<Renderer>().material = mat;
            wall.transform.parent.parent = gO.transform;

            wall = script.createInstance(tile, (bottomLeft - new Vector3(0, 0, -z)), Quaternion.identity);
            wall.GetComponent<Renderer>().material = mat;
            wall.transform.parent.parent = gO.transform;

        }
        Undo.RegisterCreatedObjectUndo(gO, "Undo wall creation");
    }

    public override void RenderPreview()
    {

        Handles.color = Color.red;
        Handles.DrawWireCube(mouseStart - new Vector3(width / 2, 0, height / 2), new Vector3(width, 2, height));
    }

    public override void MouseDown(Vector3 mousePos)
    {
        mouseStart = mousePos;
    }
    public override void MouseDrag(Vector3 mousePos)
    {
        width = -(mouseEnd.x - mouseStart.x);
        height = -(mouseEnd.z - mouseStart.z);
        mouseEnd = mousePos;
    }
    public override void MouseUp(Vector3 mousePos, TileTypes tileType, floorplan script, Material mat)
    {
        //Get the 4 corners of the rectangle
        Vector3[] points = new Vector3[]
        {
            mouseStart,
            mouseEnd,
            mouseStart-(new Vector3(width,0,0)),
            mouseEnd+(new Vector3(width,0,0))
        };

        switch (tileType)
        {
            case TileTypes.Wall:
                Render(script, script.tileset.wallTiles[0],mat);
                break;
            case TileTypes.Floor:
                break;
            case TileTypes.Pillar:
                break;
        }
        
        mouseStart = Vector3.zero;
        mouseEnd = Vector3.zero;
    }
}