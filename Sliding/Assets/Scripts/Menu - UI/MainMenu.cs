using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public MenuSounds sounds;
    public GameObject Main;
    public GameObject Lobby;
    public GameObject Options;

    public Transform astronaut;
    public Transform title;

    public Transform optionsBack;
    public Transform optionsPanel;

    public Transform lobbyButtons;
    public Transform lobbyPanel;

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
        for (int i = 0; i < 3; i++)
        {
            initialMainPositions[i] = Main.transform.GetChild(i).position;
            finalMainPositions[i] = initialMainPositions[i] - new Vector3(600, 0, 0);

            Main.transform.GetChild(i).position = finalMainPositions[i];
        }

        initialAstronautPosition = astronaut.position;
        finalAstronautPosition = astronaut.position + new Vector3(3, -3, 0);
        astronaut.position = finalAstronautPosition;

        initialTitlePosition = title.position;
        finalTitlePosition = title.position + new Vector3(0, 5, 0);
        title.position = finalTitlePosition;

        initialOptionsBackPosition = optionsBack.position;
        finalOptionsBackPosition = optionsBack.position + new Vector3(0, -300, 0);
        optionsBack.position = finalOptionsBackPosition;

        initialOptionsPanelPosition = optionsPanel.position;
        finalOptionsPanelPosition = optionsPanel.position + new Vector3(0, 1000, 0);
        optionsPanel.position = finalOptionsPanelPosition;

        initialLobbyButtonsPosition = lobbyButtons.position;
        finalLobbyButtonsPosition = lobbyButtons.position + new Vector3(0, -300, 0);
        lobbyButtons.position = finalLobbyButtonsPosition;

        initialLobbyPanelPosition = lobbyPanel.position;
        finalLobbyPanelPosition = lobbyPanel.position + new Vector3(0, 1000, 0);
        lobbyPanel.position = finalLobbyPanelPosition;

        StartCoroutine(StartRoutine());
    }

    IEnumerator StartRoutine()
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

        canInteract = true;

        while (timer < 1f)
        {
            timer += Time.deltaTime;
            astronaut.position = Vector3.Lerp(astronaut.position, initialAstronautPosition, Time.deltaTime * 5.5f);
            title.position = Vector3.Lerp(title.position, initialTitlePosition, Time.deltaTime * 5.5f);
            yield return null;
        }

    }

    public void GoToLobby()
    {
        if(canInteract)
        {
            StartCoroutine(GoToLobbyCoroutine());
            canInteract = false;
            sounds.PlayClickSound();
        }        
    }

    public void GoToMenu()
    {
        if (canInteract)
        {
            StartCoroutine(GoToMenuCoroutine());
            canInteract = false;
            sounds.PlayClickSound();
        }
    }

    public void GoToOptions()
    {
        if (canInteract)
        {
            StartCoroutine(GoToOptionsCoroutine());
            canInteract = false;
            sounds.PlayClickSound();
        }
    }

    public void GoToGame(int level)
    {
        if(canInteract)
        {
            sounds.PlayClickSound();
            SceneManager.LoadScene(level);
        }
    }

    IEnumerator GoToLobbyCoroutine()
    {
        float timer = 0;
        float time = 0.15f;

        for(int i = 0; i < 3; i++)
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

        canInteract = true;
    }

    IEnumerator GoToMenuCoroutine()
    {
        float timer = 0;
        float time = 0.3f;

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


        canInteract = true;

    }

    IEnumerator GoToOptionsCoroutine()
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

        while(timer < time)
        {
            timer += Time.deltaTime;
            optionsBack.position = Vector3.Lerp(finalOptionsBackPosition, initialOptionsBackPosition, timer / time);
            optionsPanel.position = Vector3.Lerp(finalOptionsPanelPosition, initialOptionsPanelPosition, timer / time);
            yield return null;
        }

        canInteract = true;
    }

    public void Exit()
    {
        Application.Quit();
    }

}
