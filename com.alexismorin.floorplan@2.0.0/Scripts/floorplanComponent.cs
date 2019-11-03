using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using alexism.Floorplan.Core.ScriptableObjects;
using alexism.Floorplan.Core.Enums;

namespace alexism.Floorplan.Core.Components
{
    [ExecuteInEditMode]
    [SelectionBase]
    public class floorplanComponent : MonoBehaviour
    {
        public floorplanTileset tileset;
        public List<GameObject> stuff;
        public TileTypes tileType=TileTypes.Wall;
        void Start()
        {
            stuff = new List<GameObject>();
            if (EditorApplication.isPlaying)
                return;
            if (Physics.CheckSphere(transform.GetChild(0).GetComponent<Renderer>().bounds.center, .1f))
            {

                Collider[] overlaps = Physics.OverlapSphere(transform.GetChild(0).GetComponent<Renderer>().bounds.center, .1f);
                foreach (Collider overlap in overlaps)
                {
                    if (overlap.transform.root == transform.root && overlap.transform!=transform.GetChild(0))
                    {
                        print("Destroyed overlap");
                        DestroyImmediate(overlap.transform.parent.gameObject);
                    }
                }
            }
        }

        public GameObject[] getTilesFromType(TileTypes type)
        {
            switch (type)
            {
                case TileTypes.Wall:
                    return tileset.wallTiles;
                case TileTypes.Pillar:
                    return tileset.pillarTiles;
                case TileTypes.Floor:
                    return tileset.floorTiles;
                case TileTypes.Arch:
                    return tileset.archTiles;
                case TileTypes.Window:
                    return tileset.windowTiles;
            }
            return null;
        }

        public TileTypes getTypeFromTile(GameObject tile)
        {
            TileTypes type=TileTypes.None;
            if(tileset.wallTiles.ToList().Find(x => x==tile))
                type = TileTypes.Wall;
            if(tileset.floorTiles.ToList().Find(x => x==tile))
                type = TileTypes.Floor;
            if(tileset.windowTiles.ToList().Find(x => x==tile))
                type = TileTypes.Window;
            if(tileset.pillarTiles.ToList().Find(x => x==tile))
                type = TileTypes.Pillar;
            if(tileset.archTiles.ToList().Find(x => x==tile))
                type = TileTypes.Arch;
            return type;
        }

        public void ChangeComponentType(GameObject newType)
        {

            //    GameObject newInstance = GameObject.Instantiate (newType, transform.position, transform.rotation);
            GameObject newInstance = PrefabUtility.InstantiatePrefab(newType) as GameObject;
            newInstance.transform.position = this.transform.position;
            newInstance.transform.rotation = this.transform.rotation;
            newInstance.transform.parent = this.transform.parent;
            newInstance.GetComponent<floorplanComponent>().tileset = tileset;
            newInstance.transform.GetChild(0).GetComponent<Renderer>().material = transform.GetChild(0).GetComponent<Renderer>().sharedMaterial;
            GameObject.DestroyImmediate(this.gameObject);

        }
    }
}