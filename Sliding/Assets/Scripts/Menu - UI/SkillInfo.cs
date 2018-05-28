using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfo : LobbyItem {

    public Sprite skillIcon;
    public string skillName;
    public string description;

    public GameObject buttonInfo;


    override public bool Unlocked
    {
        get
        {
            return unlocked;
        }

        set
        {
            unlocked = value;

            lockedSprite.SetActive(!unlocked);
            buttonInfo.SetActive(unlocked);
        }
    }
}
