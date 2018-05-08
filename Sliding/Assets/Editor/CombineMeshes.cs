using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class CombineMeshes : MonoBehaviour {

	public void CombineMesh()
    {
        Quaternion rot = transform.rotation;
        Vector3 pos = transform.position;

        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;

        MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();

        Debug.Log("Combining " + filters.Length + " meshes");

        Mesh finalMesh = new Mesh();

        CombineInstance[] combiners = new CombineInstance[filters.Length];

        for(int i = 0; i < filters.Length; i++)
        {
            if (filters[i].transform != transform)
            {
                combiners[i].subMeshIndex = 0;
                combiners[i].mesh = filters[i].sharedMesh;
                combiners[i].transform = filters[i].transform.localToWorldMatrix;
            }
        }

        finalMesh.CombineMeshes(combiners);

        GetComponent<MeshFilter>().sharedMesh = finalMesh;

        transform.rotation = rot;
        transform.position = pos;
    }

    public void saveMesh()
    {
        Mesh m1 = transform.GetComponent<MeshFilter>().sharedMesh;
        if (m1 != null)
        {
            AssetDatabase.CreateAsset(m1, "Assets/FBX/Platforms/Meshes/" + transform.name + ".asset"); // saves to "assets/"
            AssetDatabase.SaveAssets();
        }
    }
}
