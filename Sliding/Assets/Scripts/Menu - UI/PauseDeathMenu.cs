using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseDeathMenu : MonoBehaviour {

    public GameObject pauseMenu;
    public GameObject loadingScreen;
    public Image loadingBar;
    public Text loadingText;
    public GameManager manager;

    public bool canInteract;
    public bool canOpenPauseMenu = true;

    public void OpenPauseMenu()
    {
        if (canOpenPauseMenu)
        {
            canInteract = true;
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
    }

    public void Resume()
    {
        if (canInteract)
        {
            canInteract = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Menu()
    {
        if(canInteract)
        {
            canInteract = false;
            Time.timeScale = 1;
            StartCoroutine(LoadingScreenCoroutine(0));
        }        
    }

    public void Restart()
    {
        if (canInteract)
        {
            Time.timeScale = 1;
            canInteract = false;
            StartCoroutine(LoadingScreenCoroutine(SceneManager.GetActiveScene().buildIndex));
            StartCoroutine(TextUpdateRoutine());
        }
    }

    IEnumerator TextUpdateRoutine()
    {
        float timer = 0;
        float step = 0.5f;

        while(true)
        {
            timer += Time.deltaTime;
            if (timer < step * 3 && timer > step * 2)
            {
                loadingText.text = "LOADING . . .";
            }
            else if (timer < step * 2 && timer > step)
            {
                loadingText.text = "LOADING . .";

            }
            else if (timer < step)
            {
                loadingText.text = "LOADING .";
            }
            else if (timer < step * 4)
            {
                timer = 0;
            }
            yield return null;
        }
    }

    IEnumerator LoadingScreenCoroutine(int scene)
    {
        manager.Mute();
        loadingScreen.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);


        while (!asyncLoad.isDone)
        {            
            loadingBar.fillAmount = asyncLoad.progress;
            yield return null;
        }

    }
}
