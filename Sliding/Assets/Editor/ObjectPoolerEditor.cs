using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectPooler))]


public class ObjectPoolerEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObjectPooler myScript = (ObjectPooler)target;
        if (GUILayout.Button("Instantiate platforms"))
        {
            myScript.InstantiatePlatforms();
        }

    }
}
