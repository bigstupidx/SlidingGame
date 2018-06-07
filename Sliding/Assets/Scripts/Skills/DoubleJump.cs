using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : ActiveBase {

    public RigidbodyController player;

    PlayerSounds playerSounds;
    float timer = 0;


	// Use this for initialization
	void Awake () {
        cooldown = SkillInformations.activeAbilitiesCooldowns[0];
        playerSounds = player.GetComponent<PlayerSounds>();
	}
	
	// Update is called once per frame
	void Update () {
		if(used)
        {
            timer += Time.deltaTime;
            if(timer > cooldown)
            {
                used = false;
                timer = 0;
            }
        }
	}

    public override void Use()
    {
        used = true;
        player.anim.SetTrigger("jump");
        player.gravityForce = player.jumpForce;
        player.gravity = player.transform.up * player.gravityForce;

        if (player.anim.GetFloat("back") > 0.5)
            playerSounds.PlayBackFlipSound();
        else playerSounds.PlayFlipSound();
    }


}
