using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Data {

    public int minerals = 0;
    public float highScore = 0;

    public Options options;
    public UserPref userPref;

    public List<bool> playerSkins = new List<bool>();
    public List<bool> skateSkins = new List<bool>();
    public List<bool> activeSkills = new List<bool>();
    public List<bool> passiveSkills = new List<bool>();

    public Data()
    {
        options = new Options();
        userPref = new UserPref();
    }
}

[Serializable]
public class Options {
    public float musicVolume = -10;
    public float soundVolume = -10;

    public bool playTutorial = true;
    public bool menuAnimations = true;
}

[Serializable]
public class UserPref {
    public int skate = 0;
    public int skin = 0;
    public int activeSkill = 0;
    public int passiveSkill = 0;
}
