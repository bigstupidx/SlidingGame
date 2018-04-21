using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    public Transform player;
    public float distance = 5;
    public float height = 2;
    public float smoothing = 0.125f;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = player.GetComponent<Rigidbody>();        
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        Vector3 desiredPosition = rb.position - player.forward * distance + player.up * height;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothing);
        transform.position = smoothedPosition;

        Quaternion targetRotation = Quaternion.LookRotation(rb.position - transform.position, player.up);
        Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothing);
        transform.rotation = smoothedRotation;
    }
}
