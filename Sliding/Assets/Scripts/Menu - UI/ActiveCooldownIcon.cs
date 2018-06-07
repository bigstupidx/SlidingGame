using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ActiveCooldownIcon : MonoBehaviour {

    public Transform iconsContainer;
    public Transform activeSkillsContainer;
    public Image cooldownImage;
    public Text cooldownText;

    ActiveBase active;
    float cooldown;
    float cooldownTimer;

    float remainingCooldown;
	// Use this for initialization
	void Awake () {
        iconsContainer.GetChild(DataManager.gameData.userPref.activeSkill).gameObject.SetActive(true);

        active = activeSkillsContainer.GetChild(DataManager.gameData.userPref.activeSkill).GetComponent<ActiveBase>();
        cooldown = SkillInformations.activeAbilitiesCooldowns[DataManager.gameData.userPref.activeSkill];
	}
	
	// Update is called once per frame
	void Update () {
        if (active.used)
        {
            cooldownTimer += Time.deltaTime;
            cooldownImage.fillAmount = Mathf.Lerp(1, 0, cooldownTimer / cooldown);
            remainingCooldown -= Time.deltaTime;
            cooldownText.text = String.Format("{0:0.0}", remainingCooldown);
        }
        else
        {
            cooldownImage.fillAmount = 0;
            remainingCooldown = cooldown;
            cooldownText.text = "";
            cooldownTimer = 0;
        }
	}
}
