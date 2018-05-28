using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapGenerator : MonoBehaviour {

    //Public
    public Module startingModule;
    public List<Module> platforms;
    public Transform player;
    public ObjectPooler pooler;

    //Private
    List<Module> oldModules = new List<Module>();
    Module currentModule;
    Module[] chosenModules;

    int currentLevel = 1;
    // Use this for initialization
    void Awake () {
        currentModule = Instantiate(startingModule, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update () {

        if (Vector3.Distance(player.position, currentModule.transform.position) < 80)
        {
            Module newModule = pooler.GetObject(currentLevel);
            newModule.transform.rotation = currentModule.GetExit().rotation;
            newModule.transform.position = currentModule.GetExit().position;
            oldModules.Add(currentModule);
            currentModule = newModule;

            if (oldModules.Count > 3)
            {
                pooler.AddObject(oldModules[0]);
                oldModules.RemoveAt(0);
            }

        }
    }
    
}
