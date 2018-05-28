using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkateCameraMovement : MonoBehaviour {

    public float speed;
    public float maxY;
    public float minY;

    public Vector3 direction;

    public bool relativeDirection = true;
	// Update is called once per frame
	void Update () {
        transform.position += transform.TransformDirection(direction) * speed * Time.deltaTime;

        if (transform.position.y > maxY)
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);
	}
}
