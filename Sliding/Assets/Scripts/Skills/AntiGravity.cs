using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiGravity : ActiveBase {

    public RigidbodyController player;
    public float time;
    public float newSlidePower = 5;
    public AudioSource source;
    public Behaviour halo;

    float timer = 0;
    float activeTimer = 0;
    float previousAirDrag;


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
        used = true;
        StartCoroutine(DisableGravity());
    }

    IEnumerator DisableGravity()
    {
        previousAirDrag = player.airDrag;

        player.gravityEnabled = false;
        player.airAccelEnabled = true;
        player.airDrag = player.slideDrag;
        player.disableDeathByAltitude = true;
        halo.gameObject.SetActive(true);
        source.Play();

        activeTimer = 0;

        while (activeTimer < time && !GameManager.Instance.isDead)
        {
            activeTimer += Time.deltaTime;
            yield return null;
        }

        Disable();
    }

    public override void Disable()
    {
        halo.gameObject.SetActive(false);
        player.gravityEnabled = true;
        player.airAccelEnabled = false;
        player.airDrag = previousAirDrag;
        player.disableDeathByAltitude = false;
        source.Stop();
    }
}
