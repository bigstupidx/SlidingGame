using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CombineMeshes))]

public class CombineMeshesEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CombineMeshes myScript = (CombineMeshes)target;
        if (GUILayout.Button("Merge Meshes"))
        {
            myScript.CombineMesh();
        }

        if (GUILayout.Button("Save mesh"))
        {
            myScript.saveMesh();
        }
    }
}
