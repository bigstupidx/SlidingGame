using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTriggerLeftRight : MonoBehaviour {

    GameObject leftImage;
    GameObject rightImage;
    GameObject leftRightText;

    Image left;
    Image right;

    RigidbodyController player;

    bool trigger;
    bool set;
    float timer = 0;

    private void Awake()
    {
        Transform tut = GameObject.Find("TutorialImages").transform;

        leftImage = tut.GetChild(1).gameObject;
        rightImage = tut.GetChild(2).gameObject;
        leftRightText = tut.GetChild(3).gameObject;

        left = leftImage.GetComponent<Image>();
        right = rightImage.GetComponent<Image>();
    }

    private void Start()
    {
        player = Camera.main.GetComponent<ThirdPersonCamera>().target.GetComponent<RigidbodyController>();
        player.canJump = false;
        player.canUseSkill = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!trigger && other.tag == "Player")
        {
            trigger = true;
            Time.timeScale = 0;
            leftImage.SetActive(true);
            rightImage.SetActive(true);
            InputManager.ResetInputAxis();
            Input.ResetInputAxes();
            leftRightText.SetActive(true);
        }
    }

    private void Update()
    {
        if(trigger)
        {
            timer += Time.unscaledDeltaTime;

            left.color = new Color(left.color.r, left.color.g, left.color.b, .3f + Mathf.Abs(Mathf.Sin(timer)));
            right.color = new Color(right.color.r, right.color.g, right.color.b, .3f + Mathf.Abs(Mathf.Sin(timer)));

            if (timer > 1f && !set)
            {
                if(Mathf.Abs(InputManager.GetHorizontalInput()) > 0 ||Input.GetButton("Horizontal"))
                {
                    set = true;
                    Time.timeScale = 1;
                    leftImage.SetActive(false);
                    rightImage.SetActive(false);
                    leftRightText.SetActive(false);
                }
            }
        }
    }
}
