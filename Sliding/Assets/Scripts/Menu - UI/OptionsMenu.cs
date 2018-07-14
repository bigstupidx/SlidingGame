using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour {

    public MainMenu main;

    public Slider musicVolume;
    public Slider soundVolume;
    public Slider playTutorial;
    public Slider menuAnimations;

    public AudioMixer mixer;

    public Image switchBackground;
    public Image secondSwitchBackground;
    public Color inactiveColor;
    public Color activeColor;

    public void SetOptionsValues()
    {
        musicVolume.value = DataManager.gameData.options.musicVolume;
        soundVolume.value = DataManager.gameData.options.soundVolume;
        playTutorial.value = DataManager.gameData.options.playTutorial ? 1 : 0;
        menuAnimations.value = DataManager.gameData.options.menuAnimations ? 1 : 0;

        SetMusicVolume();
        SetSoundVolume();
        SetTutorialTrigger();
        SetMenuAnimationsTrigger();
    }

    public void Mute()
    {
        mixer.SetFloat("MusicVolume", -80);
        mixer.SetFloat("SoundVolume", -80);
    }

    public void SetMusicVolume()
    {
        DataManager.gameData.options.musicVolume = musicVolume.value;
        mixer.SetFloat("MusicVolume", musicVolume.value);
    }

    public void SaveValues()
    {
        DataManager.SaveData();
    }

    public void SetSoundVolume()
    {        
        DataManager.gameData.options.soundVolume = soundVolume.value;
        mixer.SetFloat("SoundVolume", soundVolume.value);
    }

    public void SetTutorialTrigger()
    {
        if(playTutorial.value < 1)
        {
            DataManager.gameData.options.playTutorial = false;
            switchBackground.color = inactiveColor;
        }
        else
        {
            DataManager.gameData.options.playTutorial = true;
            switchBackground.color = activeColor;
        }
    }

    public void SetMenuAnimationsTrigger()
    {
        if(menuAnimations.value < 1)
        {
            DataManager.gameData.options.menuAnimations = false;
            main.menuAnimations = false;
            secondSwitchBackground.color = inactiveColor;
        }
        else
        {
            DataManager.gameData.options.menuAnimations = true;
            main.menuAnimations = true;
            secondSwitchBackground.color = activeColor;
        }
    }
}
