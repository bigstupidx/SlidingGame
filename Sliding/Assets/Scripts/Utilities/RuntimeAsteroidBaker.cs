using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RuntimeAsteroidBaker : MonoBehaviour {
    
    Material asteroid;
    Material asteroidOutline;

	// Use this for initialization
	void Awake () {
        BasicMerge();
	}

    public void BasicMerge()
    {
        asteroid = transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials[0];
        asteroidOutline = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().sharedMaterials[0];

        Quaternion rot = transform.rotation;
        Vector3 pos = transform.position;

        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;


        MeshFilter[] filters = new MeshFilter[transform.childCount];
        MeshFilter[] outlines = new MeshFilter[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            filters[i] = transform.GetChild(i).GetComponent<MeshFilter>();
            outlines[i] = transform.GetChild(i).GetChild(0).GetComponent<MeshFilter>();
        }


        Mesh asteroidMesh = new Mesh();
        Mesh outlineMesh = new Mesh();

        List<CombineInstance> asteroidCombiners = new List<CombineInstance>();
        List<CombineInstance> outlineCombiners = new List<CombineInstance>();


        for (int i = 0; i < filters.Length; i++)
        {
            CombineInstance ci = new CombineInstance();

            ci.subMeshIndex = 0;
            ci.mesh = filters[i].sharedMesh;
            ci.transform = filters[i].transform.localToWorldMatrix;
            asteroidCombiners.Add(ci);

            CombineInstance ou = new CombineInstance();

            ou.subMeshIndex = 0;
            ou.mesh = outlines[i].sharedMesh;
            ou.transform = outlines[i].transform.localToWorldMatrix;
            outlineCombiners.Add(ou);
            
        }

        asteroidMesh.CombineMeshes(asteroidCombiners.ToArray());
        GetComponent<MeshFilter>().sharedMesh = asteroidMesh;
        GetComponent<MeshRenderer>().material = asteroid;

        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        outlineMesh.CombineMeshes(outlineCombiners.ToArray());
        GameObject outline = new GameObject();
        outline.transform.parent = this.transform;
        outline.AddComponent<MeshRenderer>();
        outline.AddComponent<MeshFilter>();

        outline.GetComponent<MeshFilter>().sharedMesh = outlineMesh;
        outline.GetComponent<MeshRenderer>().material = asteroidOutline;

        gameObject.AddComponent<MeshCollider>();
       
        transform.rotation = rot;
        transform.position = pos;
    }

    public void AdvancedMerge()
    {
        asteroid = transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials[0];
        asteroidOutline = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().sharedMaterials[0];

        Quaternion rot = transform.rotation;
        Vector3 pos = transform.position;

        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;

        // All our children (and us)
        MeshFilter[] filters = GetComponentsInChildren<MeshFilter>(false);

        // All the meshes in our children (just a big list)
        List<Material> materials = new List<Material>();
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>(false); // <-- you can optimize this
        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer.transform == transform)
                continue;
            Material[] localMats = renderer.sharedMaterials;
            foreach (Material localMat in localMats)
                if (!materials.Contains(localMat))
                    materials.Add(localMat);
        }

        // Each material will have a mesh for it.
        List<Mesh> submeshes = new List<Mesh>();
        foreach (Material material in materials)
        {
            // Make a combiner for each (sub)mesh that is mapped to the right material.
            List<CombineInstance> combiners = new List<CombineInstance>();
            foreach (MeshFilter filter in filters)
            {
                if (filter.transform == transform) continue;
                // The filter doesn't know what materials are involved, get the renderer.
                MeshRenderer renderer = filter.GetComponent<MeshRenderer>();  // <-- (Easy optimization is possible here, give it a try!)
                if (renderer == null)
                {
                    Debug.LogError(filter.name + " has no MeshRenderer");
                    continue;
                }

                // Let's see if their materials are the one we want right now.
                Material[] localMaterials = renderer.sharedMaterials;
                for (int materialIndex = 0; materialIndex < localMaterials.Length; materialIndex++)
                {
                    if (localMaterials[materialIndex] != material)
                        continue;
                    // This submesh is the material we're looking for right now.
                    CombineInstance ci = new CombineInstance();
                    ci.mesh = filter.sharedMesh;
                    ci.subMeshIndex = materialIndex;
                    ci.transform = filter.transform.localToWorldMatrix;
                    combiners.Add(ci);
                }
            }
            // Flatten into a single mesh.
            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combiners.ToArray(), true);
            submeshes.Add(mesh);
        }

        // The final mesh: combine all the material-specific meshes as independent submeshes.
        List<CombineInstance> finalCombiners = new List<CombineInstance>();
        foreach (Mesh mesh in submeshes)
        {
            CombineInstance ci = new CombineInstance();
            ci.mesh = mesh;
            ci.subMeshIndex = 0;
            ci.transform = Matrix4x4.identity;
            finalCombiners.Add(ci);
        }
        Mesh finalMesh = new Mesh();
        finalMesh.CombineMeshes(finalCombiners.ToArray(), false);
        GetComponent<MeshFilter>().sharedMesh = finalMesh;
        GetComponent<MeshRenderer>().sharedMaterials = new Material[2] { asteroid, asteroidOutline };
        transform.rotation = rot;
        transform.position = pos;

        for ( int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        gameObject.AddComponent<MeshCollider>();
    }


}
