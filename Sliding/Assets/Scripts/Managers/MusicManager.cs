using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public AudioSource[] sources;

	// Use this for initialization
	void Start () {
        sources[Random.Range(0, sources.Length)].Play();
	}
	
}
