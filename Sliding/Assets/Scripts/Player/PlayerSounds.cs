using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {

    public AudioSource source;

    public AudioClip flip;
    public AudioClip backFlip;
    public AudioClip pickup;
    public AudioClip mineralPickup;
    public AudioClip[] boneCrack;
    public AudioClip[] landing;
	
	public void PlayFlipSound()
    {
        source.PlayOneShot(flip);
    }

    public void PlayBackFlipSound()
    {
        source.PlayOneShot(backFlip);
    }

    public void PlayPickUpSound()
    {
        source.PlayOneShot(pickup);
    }

    public void PlayBoneCrackSound()
    {
        source.PlayOneShot(boneCrack[Random.Range(0,boneCrack.Length)]);
    }

    public void PlayLandingSound()
    {
        source.PlayOneShot(landing[Random.Range(0, landing.Length)]);
    }

    public void PlayMineralPickup()
    {
        source.PlayOneShot(mineralPickup);
    }
}
