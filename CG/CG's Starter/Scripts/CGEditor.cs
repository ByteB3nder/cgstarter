using System.Collections;
using System.Collections.Generic;
using CG.Starter;
using UnityEditor;
using UnityEngine;

public class CGEditor : MonoBehaviour
{
    [MenuItem("Tools/CG Tools/Create CG Manager")]
    private static void CreateObject()
    {
        GameObject newObj = new GameObject("CG Manager");
        newObj.AddComponent<CGManager>();
        newObj.transform.position = Vector3.zero; 
        Selection.activeGameObject = newObj; 
    }
}
