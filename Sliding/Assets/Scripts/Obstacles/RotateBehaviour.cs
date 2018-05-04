using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBehaviour : MonoBehaviour {

    public Vector3 rotation;
    public bool relative = true;

	// Update is called once per frame
	void Update () {
        transform.Rotate(rotation * Time.deltaTime, relative ? Space.Self : Space.World);
	}
}
