using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour {

    public Transform entry;
    public Transform exit;

    [Range(1,3)]
    public int level = 1;

	public Transform GetExit()
    {
        return exit;
    }

    public Transform GetEntry()
    {
        return entry;
    }
}
