using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour {

    public Transform exit;

    [Range(1,3)]
    public int level = 1;

    IInitializable[] initializableItems;

    private void Awake()
    {
        initializableItems = GetComponentsInChildren<IInitializable>();
        this.gameObject.SetActive(false);
    }

    public Transform GetExit()
    {
        return exit;
    }

    public void Initialize()
    {
        for(int i = 0; i < initializableItems.Length; i++)
        {
            initializableItems[i].Initialize();
        }
    }
}
