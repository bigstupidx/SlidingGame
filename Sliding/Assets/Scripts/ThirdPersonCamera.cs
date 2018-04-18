using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    public Transform player;
    public float distance = 5;
    public float smoothing = 0.125f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        Vector3 desiredPosition = player.position - player.forward * distance;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, 0.125f);
        transform.position = smoothedPosition;

        Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position, player.up);
        Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.125f);
        transform.rotation = smoothedRotation;


    }
}
