using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AsteroidReplacer))]

public class AsteroidReplacerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AsteroidReplacer myScript = (AsteroidReplacer)target;
        if (GUILayout.Button("ReplaceAsteroids"))
        {
            myScript.Replace();
        }

    }
}
