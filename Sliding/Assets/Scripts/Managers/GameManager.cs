using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using UnityEngine.Audio;


public class GameManager : MonoBehaviour {

    //Public
    public static GameManager Instance;        
    public GameObject player;
    public bool isDead = false;

    public GameObject DeathMenu;
    public Text score;
    public Text bestScore;
    public Text newHighScoreText;
    public Score scoreManager;
    public PauseDeathMenu deathMenu;
    public AudioMixer mixer;
    public AudioSource scoreFillSource;
    public AudioClip highScore;

    public Color[] newHighScoreColors;

    //Private
    Rigidbody[] ragdoll;
    RigidbodyController playerController;
    Rigidbody playerRb;
    Animator playerAnimator;
    SkatePosition skate;
    new ThirdPersonCamera camera;
    AdManager ads;

    bool deathTrigger = true;


    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        Application.targetFrameRate = 60;

        Instance = this;

        ragdoll = player.GetComponentsInChildren<Rigidbody>();
        playerController = player.GetComponent<RigidbodyController>();
        playerRb = player.GetComponent<Rigidbody>();
        playerAnimator = player.GetComponentInChildren<Animator>(false);
        skate = player.GetComponentInChildren<SkatePosition>();
        camera = Camera.main.GetComponent<ThirdPersonCamera>();
        ads = GetComponent<AdManager>();

        mixer.SetFloat("MusicVolume", DataManager.gameData.options.musicVolume);
        mixer.SetFloat("SoundVolume", DataManager.gameData.options.soundVolume);
    }
	
	// Update is called once per frame
	void Update () {
		if(deathTrigger && isDead)
        {
            deathTrigger = !deathTrigger;
            OnDeath();
        }
	}

    void OnDeath()
    {
        playerAnimator.enabled = false;
        playerController.enabled = false;
        camera.lookAt = true;
        camera.target = ragdoll[1].transform;

        skate.DetachSkate();

        foreach(Rigidbody r in ragdoll)
        {
            r.isKinematic = false;
            r.velocity = playerRb.velocity;
        }
        playerRb.velocity = Vector3.zero;
        StartCoroutine(OpenDeathMenuWithDelay(3));
    }

    IEnumerator OpenDeathMenuWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        deathMenu.canOpenPauseMenu = false;

        bestScore.text = String.Format("{0:0,0}", DataManager.gameData.highScore);
        DeathMenu.SetActive(true);
        DeathMenu.transform.localScale = Vector3.zero;

        float time = .3f;
        float timer = 0;

        while(timer < time)
        {
            timer += Time.deltaTime;
            DeathMenu.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timer / time);
            yield return null;
        }

        timer = 0;
        time = 1;
        float displayScore = 0;

        scoreFillSource.Play();
        while(timer < time)
        {
            timer += Time.deltaTime;
            displayScore = Mathf.Lerp(0, scoreManager.score, timer / time);
            score.text = String.Format("{0:0,0}", displayScore);
            score.transform.localScale = Vector3.one + Vector3.one * Mathf.PingPong(timer * 2, .2f);
            
            yield return null;
        }

        yield return new WaitForSeconds(0.15f);
        scoreFillSource.Stop();

        if (scoreManager.score > DataManager.gameData.highScore)
        {
            DataManager.gameData.highScore = (int)scoreManager.score;
            DataManager.SaveData();
            StartCoroutine(ScaleScore());
            StartCoroutine(ColorNewHighScore());
            bestScore.text = "" + String.Format("{0:0,0}", scoreManager.score);
            scoreFillSource.PlayOneShot(highScore);
        }



        if (ads.IsReady("video"))
        {
            ads.ShowVideo();
        }
        else
        {
            deathMenu.canInteract = true;
        }
    }

    IEnumerator ScaleScore()
    {
        newHighScoreText.text = "NEW HIGHSCORE!";
        while(true)
        {
            newHighScoreText.transform.localScale = Vector3.one + Vector3.one * Mathf.PingPong(Time.time, .15f);
            yield return null;
        }
    }

    IEnumerator ColorNewHighScore()
    {
        float timer = 0;
        float time = .3f;

        int nextColor = 0;

        Color previousColor = newHighScoreText.color;

        while(true)
        {
            while (timer < time)
            {
                timer += Time.deltaTime;
                newHighScoreText.color = Color.Lerp(previousColor, newHighScoreColors[nextColor], timer / time);
                yield return null;
            }
            timer = 0;
            previousColor = newHighScoreText.color;
            nextColor++;
            if (nextColor >= newHighScoreColors.Length)
                nextColor = 0;
        }
    }

    public void AdFinished()
    {
        deathMenu.canInteract = true;
    }

    public void Mute()
    {
        mixer.SetFloat("MusicVolume", -80);
        mixer.SetFloat("SoundVolume", -80);
    }
}
