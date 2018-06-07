using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMultiplier : MonoBehaviour {

    public Score score;

    float timer = 0;
    float cooldown;

	// Use this for initialization
	void Start () {
        cooldown = SkillInformations.passiveAbilitiesCooldowns[0];
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer > cooldown)
        {
            score.Steps += 1;
            timer = 0;
        }
	}
}
