using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateTextureOffset : MonoBehaviour {

    Material material;
    public Vector2 offset;
    public float speed;

    float value;

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().materials[1];
    }

    // Update is called once per frame
    void Update () {
        value += speed * Time.deltaTime; 
        material.mainTextureOffset = offset * value;
	}
}
