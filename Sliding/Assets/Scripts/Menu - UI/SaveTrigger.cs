using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            DataManager.gameData.options.playTutorial = false;
            DataManager.SaveData();
        }
    }
}
