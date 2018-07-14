using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbo : ActiveBase {

    public RigidbodyController player;
    public float time = 7;

    public float newMoveSpeed = 100;
    public float newAirSpeed = 22;
    public float newSlidePower = 20;

    float timer;

	// Use this for initialization
	void Awake () {
        cooldown = SkillInformations.activeAbilitiesCooldowns[3];

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
        StartCoroutine(TurboSpeed());
    }

    IEnumerator TurboSpeed()
    {
        float previousSlidePower = player.slidePower;
        float previousMoveSpeed = player.moveSpeed;
        float previousAirSpeed = player.airSpeed;

        player.airAccelEnabled = true;
        player.slidePower = newSlidePower;
        player.moveSpeed = newMoveSpeed;
        player.airSpeed = newAirSpeed;

        Debug.Log("turbo start");

        yield return new WaitForSeconds(time);

        Debug.Log("turbo finsih");

        player.airAccelEnabled = false;
        player.slidePower = previousSlidePower;
        player.moveSpeed = previousMoveSpeed;
        player.airSpeed = previousAirSpeed;
    }
}
