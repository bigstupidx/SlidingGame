using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Score : MonoBehaviour {

    public GameObject stepsPanel;
    public Text scoreText;
    public Text multiplierText;
    public Text mineralText;
    public Rigidbody player;
    public float multiplier = 1;



    GameObject[] stepIcons = new GameObject[3];
    Vector3 localVelocity;
    [HideInInspector]
    public float score = 0;
    int steps = 0;
    Transform playerTransform;

    public int Steps
    {
        get
        {
            return steps;
        }

        set
        {
            if(value >= 3)
            {
                steps = value % 3;
                multiplier += value / 3;
                multiplierText.text = "x" + multiplier;
            }
            else
            {
                steps = value;
            }

            for (int i = 0; i < stepIcons.Length; i++)
            {
                if (i <= steps - 1 )
                    stepIcons[i].SetActive(true);
                else stepIcons[i].SetActive(false);
            }
        }
    }


    // Use this for initialization
    void Awake () {
        for(int i = 0; i < stepIcons.Length; i++)
            stepIcons[i] = stepsPanel.transform.GetChild(i).gameObject;

        mineralText.text = "" + DataManager.gameData.minerals;

        playerTransform = player.transform;
	}
	
	// Update is called once per frame
	void Update () {

        localVelocity = playerTransform.TransformDirection(player.velocity);
        localVelocity.y = 0;

        score += (localVelocity.magnitude * Time.deltaTime * 100 * multiplier);



        scoreText.text = String.Format("{0:0,0}", score);
    }
}
