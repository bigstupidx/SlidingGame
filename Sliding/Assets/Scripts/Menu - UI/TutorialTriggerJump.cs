using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTriggerJump : MonoBehaviour {

	GameObject centerImage;
    GameObject skillText;
    GameObject jumpText;
    GameObject congratulationsText;

    Image center;
    Image iconHighlight;
    RigidbodyController player;

    bool trigger;
    bool coroutineStarted;
    float timer = 0;

    private void Awake()
    {
        Transform tut = GameObject.Find("TutorialImages").transform;
        centerImage = tut.GetChild(0).gameObject;
        center = centerImage.GetComponent<Image>();
        jumpText = tut.GetChild(4).gameObject;
        skillText = tut.GetChild(5).gameObject;
        congratulationsText = tut.GetChild(6).gameObject;

        iconHighlight = skillText.transform.GetChild(1).GetComponent<Image>();
        player = Camera.main.GetComponent<ThirdPersonCamera>().target.GetComponent<RigidbodyController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!trigger && other.tag == "Player")
        {
            trigger = true;
            Time.timeScale = 0;
            centerImage.SetActive(true);
            InputManager.ResetInputAxis();
            Input.ResetInputAxes();
            jumpText.SetActive(true);
            player.canJump = true;
        }
    }

    private void Update()
    {
        if (trigger)
        {
            timer += Time.unscaledDeltaTime;
            center.color = new Color(center.color.r, center.color.g, center.color.b, .3f + Mathf.Abs(Mathf.Sin(timer)));

            if (timer > 1f && !coroutineStarted)
            {
                if (InputManager.GetJumpInput() || Input.GetButton("Jump") )
                {
                    coroutineStarted = true;
                    Time.timeScale = 1;
                    StartCoroutine(SkillTutorial());
                }
            }
        }
    }

    IEnumerator SkillTutorial()
    {
        centerImage.SetActive(false);

        yield return new WaitForSeconds(.5f);

        jumpText.SetActive(false);
        skillText.SetActive(true);
        player.canUseSkill = true;
        centerImage.SetActive(true);
        Time.timeScale = 0;
        timer = 0;

        while(!(InputManager.GetJumpInput() || Input.GetButton("Jump")) || timer < 1)
        {
            iconHighlight.color = new Color(center.color.r, center.color.g, center.color.b, .3f + Mathf.Abs(Mathf.Sin(timer)));
            yield return null;
        }

        Time.timeScale = 1;

        centerImage.SetActive(false);
        skillText.SetActive(false);

        congratulationsText.SetActive(true);

        yield return new WaitForSeconds(5);

        congratulationsText.SetActive(false);
    }
}
