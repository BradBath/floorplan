using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using alexism.Floorplan.Core.ScriptableObjects;
using alexism.Floorplan.Core.Components;
using alexism.Floorplan.Core.Enums;
namespace alexism.Floorplan.Core
{
    [ExecuteInEditMode]

    public class floorplan : MonoBehaviour
    {

        bool toolActive;
        [SerializeField]
        public floorplanTileset tileset;

        public Material[] wallMaterials;

        [Space(15)]
        Vector3 lastHandlePosition;
        Vector3 snapLastHandlePosition;
        Vector3 handlePosition;
        Vector3 lastTileDelta;
        Vector3 tileDelta;
        GameObject geometryRoot;
        [HideInInspector]
        public float tileSize = 2f;
        Color gizmoColor = Color.red;


        void OnEnable()
        {
            snapLastHandlePosition = transform.position;
        }

        void Start()
        {
            snapLastHandlePosition = transform.position;
            geometryRoot = GameObject.Find("New Floorplan Geometry");
        }

        void Update()
        {
        }


        public GameObject createInstance(GameObject instanceType, Vector3 spawnPosition, Quaternion spawnRotation)
        {
            GameObject instance = PrefabUtility.InstantiatePrefab(instanceType) as GameObject;
            instance.transform.position = spawnPosition;
            instance.transform.rotation = spawnRotation;
            instance.transform.parent = geometryRoot.transform;
            instance.GetComponent<floorplanComponent>().tileset = tileset;
            instance.name = instanceType.name;
            return instance.transform.GetChild(0).gameObject;
        }
    }
}