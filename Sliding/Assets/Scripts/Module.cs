using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour {

    public Transform entry;
    public Transform exit;

	public Transform GetExit()
    {
        return exit;
    }

    public Transform GetEntry()
    {
        return entry;
    }
}
