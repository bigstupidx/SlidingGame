using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineralTrigger : MonoBehaviour, IInitializable {

    public GameObject items;
    public ParticleSystem particles;
    public float pickDistance = 3;

    PlayerSounds playerSounds;
    Transform player;
    Module parent;
    Text mineralText;    
    float chance;
    bool trigger;

    // Use this for initialization
    void Awake () {
        mineralText = GameObject.Find("MineralText").GetComponent<Text>();
        parent = transform.parent.GetComponent<Module>();
        player = Camera.main.GetComponent<ThirdPersonCamera>().target;

        if (parent.level == 1)
            chance = 0.03f;
        if (parent.level == 2)
            chance = 0.04f;
        if (parent.level == 3)
            chance = 0.08f;

        if (DataManager.gameData.userPref.passiveSkill == 1)
        {
            pickDistance = 8;
        }
        playerSounds = player.GetComponent<PlayerSounds>();

    }

    public void Initialize()
    {
        if(Random.value <= chance)
        {
            trigger = false;
            items.SetActive(true);
        }
        else
        {
            trigger = true;
            items.SetActive(false);
        }
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

        while(timer < time)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startingPos, player.position, timer / time);
            yield return null;
        }

        items.SetActive(false);
        DataManager.gameData.minerals += 1;
        mineralText.text = "" + DataManager.gameData.minerals;
        DataManager.SaveData();
        particles.Play();
        playerSounds.PlayMineralPickup();
    }

}
