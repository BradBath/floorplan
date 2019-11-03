using System.Collections;
using UnityEngine;
using alexism.Floorplan.Core.Enums;

namespace alexism.Floorplan.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Floorplan Tileset", menuName = "Floorplan Tileset")]
    public class floorplanTileset : ScriptableObject
    {
        public GameObject[] floorTiles;
        public GameObject[] wallTiles;
        public GameObject[] archTiles;
        public GameObject[] windowTiles;
        public GameObject[] pillarTiles;
    }
}