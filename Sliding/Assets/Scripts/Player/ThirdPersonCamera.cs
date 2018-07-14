using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    //Public
    public Transform target;
    public float distance = 5;
    public float height = 2;
    public float smoothing = 0.125f;

    [HideInInspector]
    public bool lookAt;
    //Private


    Vector3 previousUp;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(!lookAt)
        {
            Vector3 desiredPosition = target.position - target.forward * distance + target.up * height;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothing);
            transform.position = smoothedPosition;

            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position, target.up);
            Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothing);
            transform.rotation = smoothedRotation;
            previousUp = transform.up;
        }
        else
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position, previousUp);
            Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothing);
            transform.rotation = smoothedRotation;
        }
    }
}
