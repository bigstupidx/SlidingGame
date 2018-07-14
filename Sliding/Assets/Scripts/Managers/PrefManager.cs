using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefManager : MonoBehaviour {

    public RigidbodyController player;
    public SkatePosition skate;
    public Transform activeSkillsContainer;
    public Transform passiveSkillsContainer;
    public Transform playerSkinsContainer;
    public Transform skateSkinsContainer;

	// Use this for initialization
	void Awake () {
        for( int i = 0; i < activeSkillsContainer.childCount; i++)
        {
            if (i != DataManager.gameData.userPref.activeSkill)
                activeSkillsContainer.GetChild(i).gameObject.SetActive(false);
            else player.active = activeSkillsContainer.GetChild(i).GetComponent<ActiveBase>();
        }

        for (int i = 0; i < passiveSkillsContainer.childCount; i++)
        {
            if (i != DataManager.gameData.userPref.passiveSkill)
                passiveSkillsContainer.GetChild(i).gameObject.SetActive(false);
        }

        
        for (int i = 0; i < playerSkinsContainer.childCount; i++)
        {
            if (i != DataManager.gameData.userPref.skin)
                playerSkinsContainer.GetChild(i).gameObject.SetActive(false);
            else
            {
                PlayerSkinReferences p = playerSkinsContainer.GetChild(i).GetComponent<PlayerSkinReferences>();
                player.anim = p.animator;
                skate.feetPivot = p.feetPivot;
            }

        }
        
        for (int i = 0; i < skateSkinsContainer.childCount; i++)
        {
            if (i != DataManager.gameData.userPref.skate)
                skateSkinsContainer.GetChild(i).gameObject.SetActive(false);
        }
    }
}
