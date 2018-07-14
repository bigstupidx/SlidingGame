using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LobbyMenu : MonoBehaviour {

    // Public 
    public MenuSounds sounds;
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
    public MainMenu main;

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

    [HideInInspector]
    public bool canInteract = true;

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


        InitializeItemStatus();

    }

    private void Update()
    {
        minerals.text = "" + DataManager.gameData.minerals;
    }

    public void SelectPlayerSkin(LobbyItem item)
    {
        if (!canInteract)
            return;

        if (item.Unlocked)
        {
            skinLockBuyPanel.SetActive(false);

            selectedPlayerSkin.Selected = false;
            item.Selected = true;
            selectedPlayerSkin = item;
            DataManager.gameData.userPref.skin = System.Array.IndexOf(playerSkins, selectedPlayerSkin);
            DataManager.SaveData();
            sounds.PlaySwitchingSound();
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
        if (!canInteract)
            return;

        if (item.Unlocked)
        {
            skateLockBuyPanel.SetActive(false);

            selectedSkate.Selected = false;
            item.Selected = true;
            selectedSkate = item;
            DataManager.gameData.userPref.skate = System.Array.IndexOf(skateSkins, selectedSkate);
            DataManager.SaveData();
            sounds.PlaySwitchingSound();
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
        if (!canInteract)
            return;

        if (item.Unlocked)
        {
            selectedActive.Selected = false;
            item.Selected = true;
            selectedActive = item;
            DataManager.gameData.userPref.activeSkill = System.Array.IndexOf(activeSkills, selectedActive);
            DataManager.SaveData();
            sounds.PlaySwitchingSound();
        }
        else
        {
            skillToBuy = item;
            OpenBuySkillWindow(item as SkillInfo);
        }
    }

    public void SelectPassiveSkill(LobbyItem item)
    {
        if (!canInteract)
            return;

        if (item.Unlocked)
        {
            selectedPassive.Selected = false;
            item.Selected = true;
            selectedPassive = item;
            DataManager.gameData.userPref.passiveSkill = System.Array.IndexOf(passiveSkills, selectedPassive);
            DataManager.SaveData();
            sounds.PlaySwitchingSound();
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
            if (i > DataManager.gameData.playerSkins.Count - 1)
            {
                DataManager.gameData.playerSkins.Add(playerSkins[i].Unlocked);
            }
            else
            {
                playerSkins[i].Unlocked = DataManager.gameData.playerSkins[i];
            }
        }

        for (int i = 0; i < skateSkins.Length; i++)
        {
            if (i > DataManager.gameData.skateSkins.Count - 1)
            {
                DataManager.gameData.skateSkins.Add(skateSkins[i].Unlocked);
            }
            else
            {
                skateSkins[i].Unlocked = DataManager.gameData.skateSkins[i];
            }
        }

        for (int i = 0; i < activeSkills.Length; i++)
        {
            if (i > DataManager.gameData.activeSkills.Count - 1)
            {
                DataManager.gameData.activeSkills.Add(activeSkills[i].Unlocked);
            }
            else
            {
                activeSkills[i].Unlocked = DataManager.gameData.activeSkills[i];
            }
        }

        for (int i = 0; i < passiveSkills.Length; i++)
        {
            if (i > DataManager.gameData.passiveSkills.Count - 1)
            {
                DataManager.gameData.passiveSkills.Add(passiveSkills[i].Unlocked);
            }
            else
            {
                passiveSkills[i].Unlocked = DataManager.gameData.passiveSkills[i];
            }
        }

        selectedPlayerSkin = playerSkins[DataManager.gameData.userPref.skin];        
        selectedSkate = skateSkins[DataManager.gameData.userPref.skate];        
        selectedActive = activeSkills[DataManager.gameData.userPref.activeSkill];
        selectedPassive = passiveSkills[DataManager.gameData.userPref.passiveSkill];

        currentPlayer3DModel = player3DModels[DataManager.gameData.userPref.skin];
        currentSkate3DModel = skate3DModels[DataManager.gameData.userPref.skate];
        currentPlayer3DModel.SetActive(true);
        currentSkate3DModel.SetActive(true);

        selectedPlayerSkin.Selected = true;
        selectedSkate.Selected = true;
        selectedActive.Selected = true;
        selectedPassive.Selected = true;

        DataManager.SaveData();
    }
	
    public void CloseBuySkillWindow()
    {
        buySkillWindow.SetActive(false);
        sounds.PlayClickSound();
        canInteract = true;
    }

    public void OpenBuySkillWindow(SkillInfo skill)
    {
        skillPrice.text = "" + skill.price;
        skillDescription.text = skill.description;
        skillIcon.sprite = skill.skillIcon;
        skillName.text = skill.skillName;

        skillBuyButton.SetActive(true);
        buySkillWindow.SetActive(true);

        sounds.PlayPopUpSound();

        canInteract = false;
    }

    public void OpenInfoPanel(SkillInfo skill)
    {
        skillPrice.text = "" + skill.price;
        skillDescription.text = skill.description;
        skillIcon.sprite = skill.skillIcon;
        skillName.text = skill.skillName;

        skillBuyButton.SetActive(false);
        buySkillWindow.SetActive(true);

        sounds.PlayPopUpSound();
        canInteract = false;
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
        if (!canInteract)
            return;

        if (DataManager.gameData.minerals >= skateToBuy.price)
        {
            selectedSkate.Selected = false;
            selectedSkate = skateToBuy;
            selectedSkate.Unlocked = true;
            selectedSkate.Selected = true;
            skateLockBuyPanel.SetActive(false);

            DataManager.gameData.skateSkins[Array.IndexOf(skateSkins, selectedSkate)] = true;
            DataManager.gameData.userPref.skate = Array.IndexOf(skateSkins, selectedSkate);
            DataManager.gameData.minerals -= selectedSkate.price;
            DataManager.SaveData();

            minerals.text = "" + DataManager.gameData.minerals;

            sounds.PlayBoughtSound();
        }
        else
        {
            main.OpenWatchAVideoPanel();
            sounds.PlayErrorSound();
        }
    }

    public void BuySkin()
    {
        if (!canInteract)
            return;

        if (DataManager.gameData.minerals >= skinToBuy.price)
        {
            selectedPlayerSkin.Selected = false;
            selectedPlayerSkin = skinToBuy;
            selectedPlayerSkin.Unlocked = true;
            selectedPlayerSkin.Selected = true;
            skinLockBuyPanel.SetActive(false);

            DataManager.gameData.playerSkins[Array.IndexOf(playerSkins, selectedPlayerSkin)] = true;
            DataManager.gameData.userPref.skin = Array.IndexOf(playerSkins, selectedPlayerSkin);
            DataManager.gameData.minerals -= skinToBuy.price;
            DataManager.SaveData();

            minerals.text = "" + DataManager.gameData.minerals;

            sounds.PlayBoughtSound();
        }
        else
        {
            main.OpenWatchAVideoPanel();
            sounds.PlayErrorSound();
        }

    }

    public void BuySkill()
    {
        if (DataManager.gameData.minerals >= skillToBuy.price)
        {
            if(Array.IndexOf(activeSkills, skillToBuy) > -1)
            {
                selectedActive.Selected = false;
                selectedActive = skillToBuy;
                selectedActive.Unlocked = true;
                selectedActive.Selected = true;
                CloseBuySkillWindow();

                DataManager.gameData.activeSkills[Array.IndexOf(activeSkills, selectedActive)] = true;
                DataManager.gameData.userPref.activeSkill = Array.IndexOf(activeSkills, selectedActive);
                DataManager.gameData.minerals -= skillToBuy.price;
                DataManager.SaveData();

                minerals.text = "" + DataManager.gameData.minerals;

                sounds.PlayBoughtSound();
            }
            else
            {
                selectedPassive.Selected = false;
                selectedPassive = skillToBuy;
                selectedPassive.Unlocked = true;
                selectedPassive.Selected = true;
                CloseBuySkillWindow();

                DataManager.gameData.passiveSkills[Array.IndexOf(passiveSkills, selectedPassive)] = true;
                DataManager.gameData.userPref.passiveSkill = Array.IndexOf(passiveSkills, selectedPassive);
                DataManager.gameData.minerals -= skillToBuy.price;
                DataManager.SaveData();

                minerals.text = "" + DataManager.gameData.minerals;

                sounds.PlayBoughtSound();
            }

        }
        else
        {
            main.OpenWatchAVideoPanel();
            CloseBuySkillWindow();
            sounds.PlayErrorSound();
        }
    }

    
}
