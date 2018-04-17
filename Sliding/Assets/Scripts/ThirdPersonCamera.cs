using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    public Transform player;
    public float distance = 5;
    public float yOffset = 1;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void LateUpdate () {

        //transform.position = Vector3.Lerp(transform.position,(player.position -player.forward * distance) + new Vector3(0,yOffset,0), Time.deltaTime * 5);
        transform.position = player.position - player.forward * distance;
        transform.LookAt(player.position);
	}
}
