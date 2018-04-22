using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //Public
    public static GameManager Instance;        
    public GameObject player;

    public bool isDead = false;

    //Private
    Rigidbody[] ragdoll;
    RigidbodyController playerController;
    Rigidbody playerRb;
    Animator playerAnimator;
    new ThirdPersonCamera camera;

    bool deathTrigger = true;

    private void Awake()
    {
        Instance = this;

        ragdoll = player.GetComponentsInChildren<Rigidbody>();
        playerController = player.GetComponent<RigidbodyController>();
        playerRb = player.GetComponent<Rigidbody>();
        playerAnimator = player.GetComponentInChildren<Animator>();
        camera = Camera.main.GetComponent<ThirdPersonCamera>();
    }
	
	// Update is called once per frame
	void Update () {
		if(deathTrigger && isDead)
        {
            deathTrigger = !deathTrigger;
            OnDeath();
        }
	}

    void OnDeath()
    {
        playerAnimator.enabled = false;
        playerController.enabled = false;
        camera.lookAt = true;
        camera.target = ragdoll[1].transform;

        foreach(Rigidbody r in ragdoll)
        {
            r.isKinematic = false;
            r.velocity = playerRb.velocity;
        }
        playerRb.velocity = Vector3.zero;
    }
}
