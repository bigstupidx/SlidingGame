using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lokd : MonoBehaviour {

    public Transform lookat;

	// Use this for initialization
	void Start () {
        transform.LookAt(lookat);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
