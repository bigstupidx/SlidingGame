using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : ActiveBase {

    public RigidbodyController player;
    public Transform flyingPivot;
    public LayerMask deathMask;
    public GameObject shield;
    public float force;
    

    CapsuleCollider capsule;
    PlayerSounds playerSounds;
    Vector3 targetRot = new Vector3(0, 0, 360);
    float timer;

    float height;
    float radius;

    // Use this for initialization
    void Awake () {
        cooldown = SkillInformations.activeAbilitiesCooldowns[1];
        playerSounds = player.GetComponent<PlayerSounds>();
        capsule = player.GetComponent<CapsuleCollider>();
        height = capsule.height;
        radius = capsule.radius;
    }
	
	// Update is called once per frame
	void Update () {
        if (used)
        {
            timer += Time.deltaTime;
            if (timer > cooldown)
            {
                used = false;
                timer = 0;
            }
        }
    }

    public override void Use()
    {
        StartCoroutine(DashAndSpin());
        used = true;        
    }

    IEnumerator DashAndSpin()
    {
        //Physics.IgnoreLayerCollision(8, 10, true);
        playerSounds.PlayDashSound();
        //shield.SetActive(true);
        player.gravityEnabled = false;

        float previousRotationSpeed = player.airSpeed;
        player.airSpeed = 100;
        player.AddForce(player.transform.forward * 50, false, false);


        float dashTime = .6f;
        float dashTimer = 0;

        while(dashTimer < dashTime)
        {
            dashTimer += Time.deltaTime;
            flyingPivot.localEulerAngles = Vector3.Lerp(Vector3.zero, targetRot, dashTimer / dashTime);
            //player.AddForce(player.transform.forward * force * Time.deltaTime, false, false);
            yield return null;
        }

        player.airSpeed = previousRotationSpeed;
        player.gravityEnabled = true;


        while (Physics.CheckCapsule(capsule.transform.TransformPoint(capsule.center) + capsule.transform.up * (height * 0.5f - radius),
            capsule.transform.TransformPoint(capsule.center) - capsule.transform.up * (height * 0.5f - radius), radius, deathMask))
        {
            yield return null;
        }

        //shield.SetActive(false);
        //Physics.IgnoreLayerCollision(8, 10, false);

    }

}
