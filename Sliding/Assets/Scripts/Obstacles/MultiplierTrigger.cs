using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierTrigger : MonoBehaviour, IInitializable {

    
    public GameObject items;
    public ParticleSystem particles;
    public int value = 1;
    public float pickDistance = 3;

    Transform player;
    Score score;
    bool trigger = false;

    PlayerSounds playerSounds;

    public void Awake()
    {
        score = GameObject.FindObjectOfType<Score>();
        player = Camera.main.GetComponent<ThirdPersonCamera>().target;

        if(DataManager.gameData.userPref.passiveSkill == 1)
        {
            pickDistance = 8;
        }

        playerSounds = player.GetComponent<PlayerSounds>();
    }

    public void Initialize()
    {
        trigger = false;
        items.SetActive(true);
    }

    void Update()
    {
        if (!trigger && Vector3.Distance(player.position, transform.position) < pickDistance)
        {
            trigger = true;
            StartCoroutine(PositionLerp());
        }
    }

    IEnumerator PositionLerp()
    {
        Vector3 startingPos = transform.position;

        float time = 0.2f;
        float timer = 0;

        while (timer < time)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startingPos, player.position, timer / time);
            yield return null;
        }

        items.SetActive(false);
        score.Steps += value;
        particles.Play();
        playerSounds.PlayPickUpSound();
    }

}
