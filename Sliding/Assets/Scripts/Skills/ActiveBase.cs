using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBase : MonoBehaviour {

    public float cooldown;
    public bool used;

    public void Start()
    {
        if(DataManager.gameData.userPref.passiveSkill == 3)
        {
            cooldown *= .6f;
        }
    }

    public virtual void Use()
    { }

    public virtual void Disable()
    {

    }
}
