using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : ActiveBase {

    public RigidbodyController player;
    PlayerSounds playerSounds;
    public GameObject shield;
    public GameManager manager;
    float timer;

    // Use this for initialization
    void Awake () {
        cooldown = SkillInformations.activeAbilitiesCooldowns[2];
        playerSounds = player.GetComponent<PlayerSounds>();
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
        used = true;
        StartCoroutine(ActivateShield());
    }


    IEnumerator ActivateShield()
    {
        float timer = 0;
        float time = 8;
        Physics.IgnoreLayerCollision(8, 10, true);
        shield.SetActive(true);

        while(timer < time && !manager.isDead)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        shield.SetActive(false);
        Physics.IgnoreLayerCollision(8, 10, false);
    }
}
