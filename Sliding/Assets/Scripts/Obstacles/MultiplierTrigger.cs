using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierTrigger : MonoBehaviour, IInitializable {

    public Score score;
    public GameObject items;

    public int value = 1;

    bool trigger = false;

    public void Initialize()
    {
        trigger = false;
        items.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!trigger && other.tag == "Player")
        {
            trigger = true;
            score.Steps += value;
            items.SetActive(false);
        }
    }


}
