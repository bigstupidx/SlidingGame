using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LobbyMenu : MonoBehaviour {

    // Public 

    public GameObject playerSkinsContainer;
    public GameObject skateContainer;
    public GameObject activeSkillsContainer;
    public GameObject passiveSkillsContainer;

    public Text minerals;

    public GameObject buySkillWindow;
    public Image skillIcon;
    public Text skillName;
    public Text skillDescription;
    public Text skillPrice;

    public GameObject playerModelsContainer;
    public GameObject skateModelsContainer;

    public GameObject skinLockBuyPanel;
    public GameObject skateLockBuyPanel;
    public Text skinPrice;
    public Text skatePrice;

    public GameObject skillBuyButton;
    // Private

    LobbyItem[] playerSkins;
    LobbyItem[] skateSkins;
    LobbyItem[] activeSkills;
    LobbyItem[] passiveSkills;

    LobbyItem selectedPlayerSkin;
    LobbyItem selectedSkate;
    LobbyItem selectedActive;
    LobbyItem selectedPassive;

    GameObject[] player3DModels;
    GameObject[] skate3DModels;

    GameObject currentPlayer3DModel;
    GameObject currentSkate3DModel;

    LobbyItem skateToBuy;
    LobbyItem skinToBuy;
    LobbyItem skillToBuy;


	// Use this for initialization
	void Awake () {
        playerSkins = playerSkinsContainer.GetComponentsInChildren<LobbyItem>();
        skateSkins = skateContainer.GetComponentsInChildren<LobbyItem>();
        activeSkills = activeSkillsContainer.GetComponentsInChildren<LobbyItem>();
        passiveSkills = passiveSkillsContainer.GetComponentsInChildren<LobbyItem>();

        player3DModels = new GameObject[playerModelsContainer.transform.childCount];
        for (int i = 0; i < playerModelsContainer.transform.childCount; i++)
            player3DModels[i] = playerModelsContainer.transform.GetChild(i).gameObject;

        skate3DModels = new GameObject[skateModelsContainer.transform.childCount];
        for (int i = 0; i < skateModelsContainer.transform.childCount; i++)
            skate3DModels[i] = skateModelsContainer.transform.GetChild(i).gameObject;

        minerals.text = "" + DataLoader.gameData.minerals;

        InitializeItemStatus();

        

    }

    public void SelectPlayerSkin(LobbyItem item)
    {
        if (item.Unlocked)
        {
            skinLockBuyPanel.SetActive(false);

            selectedPlayerSkin.Selected = false;
            item.Selected = true;
            selectedPlayerSkin = item;
            DataLoader.gameData.userPref.skin = System.Array.IndexOf(playerSkins, selectedPlayerSkin);
            DataLoader.SaveData();
        }
        else
        {
            skinToBuy = item;
            OpenBuySkinWindow();
        }

        currentPlayer3DModel.SetActive(false);
        currentPlayer3DModel = player3DModels[System.Array.IndexOf(playerSkins, item)];
        currentPlayer3DModel.SetActive(true);
    }

    public void SelectSkateSkin(LobbyItem item)
    {
        if (item.Unlocked)
        {
            skateLockBuyPanel.SetActive(false);

            selectedSkate.Selected = false;
            item.Selected = true;
            selectedSkate = item;
            DataLoader.gameData.userPref.skate = System.Array.IndexOf(skateSkins, selectedSkate);
            DataLoader.SaveData();
        }
        else
        {
            skateToBuy = item;
            OpenBuySkateWindow();
        }

        currentSkate3DModel.SetActive(false);
        currentSkate3DModel = skate3DModels[System.Array.IndexOf(skateSkins, item)];
        currentSkate3DModel.SetActive(true);
    }

    public void SelectActiveSkill(LobbyItem item)
    {
        if (item.Unlocked)
        {
            selectedActive.Selected = false;
            item.Selected = true;
            selectedActive = item;
            DataLoader.gameData.userPref.activeSkill = System.Array.IndexOf(activeSkills, selectedActive);
            DataLoader.SaveData();
        }
        else
        {
            skillToBuy = item;
            OpenBuySkillWindow(item as SkillInfo);
        }
    }

    public void SelectPassiveSkill(LobbyItem item)
    {
        if (item.Unlocked)
        {
            selectedPassive.Selected = false;
            item.Selected = true;
            selectedPassive = item;
            DataLoader.gameData.userPref.passiveSkill = System.Array.IndexOf(passiveSkills, selectedPassive);
            DataLoader.SaveData();
        }
        else
        {
            skillToBuy = item;
            OpenBuySkillWindow(item as SkillInfo);
        }
    }

    public void InitializeItemStatus()
    {
        for (int i = 0; i < playerSkins.Length; i++)
        {
            if (i > DataLoader.gameData.playerSkins.Count - 1)
            {
                DataLoader.gameData.playerSkins.Add(playerSkins[i].Unlocked);
            }
            else
            {
                playerSkins[i].Unlocked = DataLoader.gameData.playerSkins[i];
            }
        }

        for (int i = 0; i < skateSkins.Length; i++)
        {
            if (i > DataLoader.gameData.skateSkins.Count - 1)
            {
                DataLoader.gameData.skateSkins.Add(skateSkins[i].Unlocked);
            }
            else
            {
                skateSkins[i].Unlocked = DataLoader.gameData.skateSkins[i];
            }
        }

        for (int i = 0; i < activeSkills.Length; i++)
        {
            if (i > DataLoader.gameData.activeSkills.Count - 1)
            {
                DataLoader.gameData.activeSkills.Add(activeSkills[i].Unlocked);
            }
            else
            {
                activeSkills[i].Unlocked = DataLoader.gameData.activeSkills[i];
            }
        }

        for (int i = 0; i < passiveSkills.Length; i++)
        {
            if (i > DataLoader.gameData.passiveSkills.Count - 1)
            {
                DataLoader.gameData.passiveSkills.Add(passiveSkills[i].Unlocked);
            }
            else
            {
                passiveSkills[i].Unlocked = DataLoader.gameData.passiveSkills[i];
            }
        }

        selectedPlayerSkin = playerSkins[DataLoader.gameData.userPref.skin];        
        selectedSkate = skateSkins[DataLoader.gameData.userPref.skate];        
        selectedActive = activeSkills[DataLoader.gameData.userPref.activeSkill];
        selectedPassive = passiveSkills[DataLoader.gameData.userPref.passiveSkill];

        currentPlayer3DModel = player3DModels[DataLoader.gameData.userPref.skin];
        currentSkate3DModel = skate3DModels[DataLoader.gameData.userPref.skate];
        currentPlayer3DModel.SetActive(true);
        currentSkate3DModel.SetActive(true);

        selectedPlayerSkin.Selected = true;
        selectedSkate.Selected = true;
        selectedActive.Selected = true;
        selectedPassive.Selected = true;

        DataLoader.SaveData();
    }
	
    public void CloseBuySkillWindow()
    {
        buySkillWindow.SetActive(false);
    }

    public void OpenBuySkillWindow(SkillInfo skill)
    {
        skillPrice.text = "" + skill.price;
        skillDescription.text = skill.description;
        skillIcon.sprite = skill.skillIcon;
        skillName.text = skill.skillName;

        skillBuyButton.SetActive(true);
        buySkillWindow.SetActive(true);
    }

    public void OpenInfoPanel(SkillInfo skill)
    {
        skillPrice.text = "" + skill.price;
        skillDescription.text = skill.description;
        skillIcon.sprite = skill.skillIcon;
        skillName.text = skill.skillName;

        skillBuyButton.SetActive(false);
        buySkillWindow.SetActive(true);
    }

    public void OpenBuySkinWindow()
    {
        skinPrice.text = "" + skinToBuy.price;
        skinLockBuyPanel.SetActive(true);
    }

    public void OpenBuySkateWindow()
    {
        skatePrice.text = "" + skateToBuy.price;
        skateLockBuyPanel.SetActive(true);
    }

    public void BuySkate()
    {
        if(DataLoader.gameData.minerals >= skateToBuy.price)
        {
            selectedSkate.Selected = false;
            selectedSkate = skateToBuy;
            selectedSkate.Unlocked = true;
            selectedSkate.Selected = true;
            skateLockBuyPanel.SetActive(false);

            DataLoader.gameData.skateSkins[Array.IndexOf(skateSkins, selectedSkate)] = true;
            DataLoader.gameData.userPref.skate = Array.IndexOf(skateSkins, selectedSkate);
            DataLoader.gameData.minerals -= selectedSkate.price;
            DataLoader.SaveData();

            minerals.text = "" + DataLoader.gameData.minerals;
        }
    }

    public void BuySkin()
    {
        if (DataLoader.gameData.minerals >= skinToBuy.price)
        {
            selectedPlayerSkin.Selected = false;
            selectedPlayerSkin = skinToBuy;
            selectedPlayerSkin.Unlocked = true;
            selectedPlayerSkin.Selected = true;
            skinLockBuyPanel.SetActive(false);

            DataLoader.gameData.playerSkins[Array.IndexOf(playerSkins, selectedPlayerSkin)] = true;
            DataLoader.gameData.userPref.skin = Array.IndexOf(playerSkins, selectedPlayerSkin);
            DataLoader.gameData.minerals -= skinToBuy.price;
            DataLoader.SaveData();

            minerals.text = "" + DataLoader.gameData.minerals;
        }
    }

    public void BuySkill()
    {
        if (DataLoader.gameData.minerals >= skillToBuy.price)
        {
            if(Array.IndexOf(activeSkills, skillToBuy) > -1)
            {
                selectedActive.Selected = false;
                selectedActive = skillToBuy;
                selectedActive.Unlocked = true;
                selectedActive.Selected = true;
                CloseBuySkillWindow();

                DataLoader.gameData.activeSkills[Array.IndexOf(activeSkills, selectedActive)] = true;
                DataLoader.gameData.userPref.activeSkill = Array.IndexOf(activeSkills, selectedActive);
                DataLoader.gameData.minerals -= skillToBuy.price;
                DataLoader.SaveData();

                minerals.text = "" + DataLoader.gameData.minerals;
            }
            else
            {
                selectedPassive.Selected = false;
                selectedPassive = skillToBuy;
                selectedPassive.Unlocked = true;
                selectedPassive.Selected = true;
                CloseBuySkillWindow();

                DataLoader.gameData.passiveSkills[Array.IndexOf(passiveSkills, selectedPassive)] = true;
                DataLoader.gameData.userPref.passiveSkill = Array.IndexOf(passiveSkills, selectedPassive);
                DataLoader.gameData.minerals -= skillToBuy.price;
                DataLoader.SaveData();

                minerals.text = "" + DataLoader.gameData.minerals;
            }
            
        }
    }

    
}
