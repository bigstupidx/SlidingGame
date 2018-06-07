using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSounds : MonoBehaviour {

    public AudioSource source;
    public AudioClip click;
    public AudioClip error;
    public AudioClip bought;
    public AudioClip switching;
    public AudioClip popUp;
    public AudioClip whoop;

	public void PlayClickSound()
    {
        source.PlayOneShot(click);
    }

    public void PlayErrorSound()
    {
        source.PlayOneShot(error);
    }

    public void PlayBoughtSound()
    {
        source.PlayOneShot(bought);
    }

    public void PlaySwitchingSound()
    {
        source.PlayOneShot(switching);
    }

    public void PlayPopUpSound()
    {
        source.PlayOneShot(popUp);
    }

    public void PlayWhoopSound()
    {
        source.PlayOneShot(whoop);
    }
}
