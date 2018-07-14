using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public MenuSounds sounds;
    public GameObject Main;
    public GameObject Lobby;
    public GameObject Options;
    public GameObject Video;
    public GameObject showVideoButton;
    public GameObject unavailableVideoButton;
    public GameObject loadingScreen;
    public Image loadingBar;
    public Text loadingText;


    public Transform astronaut;
    public Transform title;

    public Transform optionsBack;
    public Transform optionsPanel;

    public Transform lobbyButtons;
    public Transform lobbyPanel;

    public bool menuAnimations = true;

    public OptionsMenu options;
    public LobbyMenu lobbyMenu;

    public int videoRewardMinerals = 10;
    AdManager ads;

    bool canInteract = false;

    Vector3[] initialMainPositions = new Vector3[3];
    Vector3[] finalMainPositions = new Vector3[3];

    Vector3 initialAstronautPosition;
    Vector3 finalAstronautPosition;

    Vector3 initialTitlePosition;
    Vector3 finalTitlePosition;

    Vector3 initialOptionsBackPosition;
    Vector3 finalOptionsBackPosition;
    Vector3 initialOptionsPanelPosition;
    Vector3 finalOptionsPanelPosition;

    Vector3 initialLobbyButtonsPosition;
    Vector3 finalLobbyButtonsPosition;
    Vector3 initialLobbyPanelPosition;
    Vector3 finalLobbyPanelPosition;

    private void Start()
    {
        Main.SetActive(false);
        astronaut.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
        StartCoroutine(PreStartRoutine());

        options.SetOptionsValues();

        ads = GetComponent<AdManager>();

        menuAnimations = DataManager.gameData.options.menuAnimations;
    }

    IEnumerator PreStartRoutine()
    {
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < 3; i++)
        {
            initialMainPositions[i] = Main.transform.GetChild(i).position;
            finalMainPositions[i] = initialMainPositions[i] - new Vector3(600, 0, 0);

            if (menuAnimations)
                Main.transform.GetChild(i).position = finalMainPositions[i];
        }

        initialAstronautPosition = astronaut.position;
        finalAstronautPosition = astronaut.position + new Vector3(3, -3, 0);
        if (menuAnimations)
            astronaut.position = finalAstronautPosition;

        initialTitlePosition = title.position;
        finalTitlePosition = title.position + new Vector3(0, 5, 0);
        if (menuAnimations)
            title.position = finalTitlePosition;

        initialOptionsBackPosition = optionsBack.position;
        finalOptionsBackPosition = optionsBack.position + new Vector3(0, -300, 0);
        if (menuAnimations)
            optionsBack.position = finalOptionsBackPosition;

        initialOptionsPanelPosition = optionsPanel.position;
        finalOptionsPanelPosition = optionsPanel.position + new Vector3(0, 1000, 0);
        if (menuAnimations)
            optionsPanel.position = finalOptionsPanelPosition;

        initialLobbyButtonsPosition = lobbyButtons.position;
        finalLobbyButtonsPosition = lobbyButtons.position + new Vector3(0, -300, 0);
        if (menuAnimations)
            lobbyButtons.position = finalLobbyButtonsPosition;

        initialLobbyPanelPosition = lobbyPanel.position;
        finalLobbyPanelPosition = lobbyPanel.position + new Vector3(0, 1000, 0);
        if (menuAnimations)
            lobbyPanel.position = finalLobbyPanelPosition;

        Main.SetActive(true);
        astronaut.gameObject.SetActive(true);
        title.gameObject.SetActive(true);

        StartCoroutine(StartRoutine());

    }

    IEnumerator StartRoutine()
    {
        if (menuAnimations)
        {
            float timer = 0;
            float time = 0.15f;

            for (int i = 0; i < 3; i++)
            {
                while (timer < time)
                {
                    timer += Time.deltaTime;
                    Main.transform.GetChild(i).position = Vector3.Lerp(finalMainPositions[i], initialMainPositions[i], timer / time);
                    yield return null;
                }
                timer = 0;
                sounds.PlayWhoopSound();
            }


            while (timer < .5f)
            {
                timer += Time.deltaTime;
                astronaut.position = Vector3.Lerp(astronaut.position, initialAstronautPosition, Time.deltaTime *11f);
                title.position = Vector3.Lerp(title.position, initialTitlePosition, Time.deltaTime * 11f);
                yield return null;
            }
            canInteract = true;

        }

        canInteract = true;

    }

    public void GoToLobby()
    {
        if(canInteract)
        {
            canInteract = false;
            StartCoroutine(GoToLobbyCoroutine());
            sounds.PlayClickSound();
        }        
    }

    public void GoToMenu()
    {
        if (canInteract && lobbyMenu.canInteract)
        {
            canInteract = false;
            StartCoroutine(GoToMenuCoroutine());
            sounds.PlayClickSound();
        }
    }

    public void GoToOptions()
    {
        if (canInteract)
        {
            canInteract = false;
            StartCoroutine(GoToOptionsCoroutine());
            sounds.PlayClickSound();
        }
    }

    public void GoToGame(int level)
    {
        if(canInteract && lobbyMenu.canInteract)
        {
            sounds.PlayClickSound();
            StartCoroutine(LoadingScreenCoroutine());
            StartCoroutine(TextUpdateRoutine());
            canInteract = false;
            lobbyMenu.canInteract = false;
        }
    }

    IEnumerator GoToLobbyCoroutine()
    {
        if (menuAnimations)
        {
            float timer = 0;
            float time = 0.15f;

            for (int i = 0; i < 3; i++)
            {
                while (timer < time)
                {
                    timer += Time.deltaTime;
                    Main.transform.GetChild(i).position = Vector3.Lerp(initialMainPositions[i], finalMainPositions[i], timer / time);
                    astronaut.position = Vector3.Lerp(astronaut.position, finalAstronautPosition, Time.deltaTime * 3);
                    title.position = Vector3.Lerp(title.position, finalTitlePosition, Time.deltaTime * 3);
                    yield return null;
                }

                timer = 0;
            }

            Main.SetActive(false);
            Lobby.SetActive(true);

            time = 0.5f;

            while (timer < time)
            {
                timer += Time.deltaTime;
                lobbyButtons.position = Vector3.Lerp(finalLobbyButtonsPosition, initialLobbyButtonsPosition, timer / time);
                lobbyPanel.position = Vector3.Lerp(finalLobbyPanelPosition, initialLobbyPanelPosition, timer / time);
                yield return null;
            }
        }
        else
        {
            Main.SetActive(false);
            Lobby.SetActive(true);
            astronaut.position = finalAstronautPosition;
            title.position = finalTitlePosition;
        }

        canInteract = true;
    }

    IEnumerator GoToMenuCoroutine()
    {
        if (menuAnimations)
        {
            float timer = 0;
            float time = 0.3f;

            for (int i = 0; i < 3; i++)
            {
                Main.transform.GetChild(i).position = finalMainPositions[i];
            }

            while (timer < time)
            {
                timer += Time.deltaTime;
                optionsBack.position = Vector3.Lerp(initialOptionsBackPosition, finalOptionsBackPosition, timer / time);
                optionsPanel.position = Vector3.Lerp(initialOptionsPanelPosition, finalOptionsPanelPosition, timer / time);
                lobbyButtons.position = Vector3.Lerp(initialLobbyButtonsPosition, finalLobbyButtonsPosition, timer / time);
                lobbyPanel.position = Vector3.Lerp(initialLobbyPanelPosition, finalLobbyPanelPosition, timer / time);
                yield return null;
            }

            Lobby.SetActive(false);
            Options.SetActive(false);
            Main.SetActive(true);

            timer = 0;
            time = 0.15f;

            for (int i = 0; i < 3; i++)
            {
                while (timer < time)
                {
                    timer += Time.deltaTime;
                    Main.transform.GetChild(i).position = Vector3.Lerp(finalMainPositions[i], initialMainPositions[i], timer / time);
                    astronaut.position = Vector3.Lerp(astronaut.position, initialAstronautPosition, Time.deltaTime * 8);
                    title.position = Vector3.Lerp(title.position, initialTitlePosition, Time.deltaTime * 8);
                    yield return null;
                }
                sounds.PlayWhoopSound();

                timer = 0;
            }

        }
        else
        {
            Lobby.SetActive(false);
            Options.SetActive(false);
            Main.SetActive(true);
            astronaut.position = initialAstronautPosition;
            title.position = initialTitlePosition;


            lobbyPanel.position = initialLobbyPanelPosition;
            lobbyButtons.position = initialLobbyButtonsPosition;

            for(int i = 0; i < 3; i++)
            {
                Main.transform.GetChild(i).position = initialMainPositions[i];
            }
        }
        canInteract = true;

    }

    IEnumerator GoToOptionsCoroutine()
    {
        if (menuAnimations)
        {


            float timer = 0;
            float time = 0.15f;

            for (int i = 0; i < 3; i++)
            {
                while (timer < time)
                {
                    timer += Time.deltaTime;
                    Main.transform.GetChild(i).position = Vector3.Lerp(initialMainPositions[i], finalMainPositions[i], timer / time);
                    astronaut.position = Vector3.Lerp(astronaut.position, finalAstronautPosition, Time.deltaTime * 3);
                    title.position = Vector3.Lerp(title.position, finalTitlePosition, Time.deltaTime * 3);
                    yield return null;
                }
                timer = 0;
            }

            Options.SetActive(true);
            Main.SetActive(false);

            time = 0.5f;

            while (timer < time)
            {
                timer += Time.deltaTime;
                optionsBack.position = Vector3.Lerp(finalOptionsBackPosition, initialOptionsBackPosition, timer / time);
                optionsPanel.position = Vector3.Lerp(finalOptionsPanelPosition, initialOptionsPanelPosition, timer / time);
                yield return null;
            }
        }
        else
        {
            Options.SetActive(true);
            Main.SetActive(false);
            astronaut.position = finalAstronautPosition;
            title.position = finalTitlePosition;
        }
        canInteract = true;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void OpenWatchAVideoPanel()
    {
        if(canInteract)
        {
            canInteract = false;
            Video.SetActive(true);

            if (ads.IsReady("rewardedVideo"))
            {
                showVideoButton.SetActive(true);
            }
            else
            {
                unavailableVideoButton.SetActive(true);
            }

            sounds.PlayClickSound();
        }
    }

    public void CloseWatchAVideoPanel()
    {
        unavailableVideoButton.SetActive(false);
        showVideoButton.SetActive(false);
        Video.SetActive(false);
        canInteract = true;
        sounds.PlayClickSound();
    }

    public void ShowRewardedVideo()
    {
        ads.ShowRewardedVideo();
    }

    public void HasWatchedVideo()
    {
        DataManager.gameData.minerals += videoRewardMinerals;
        DataManager.SaveData();
    }

    public void HasFinishedWatching()
    {
        CloseWatchAVideoPanel();
    }

    IEnumerator TextUpdateRoutine()
    {
        float timer = 0;
        float step = 0.5f;

        while(true)
        {
            timer += Time.deltaTime;

            Debug.Log(timer);

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

    IEnumerator LoadingScreenCoroutine()
    {
        options.Mute();
        loadingScreen.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        while (!asyncLoad.isDone)
        {
           
            loadingBar.fillAmount = asyncLoad.progress;
            yield return null;
        }

    }
}
