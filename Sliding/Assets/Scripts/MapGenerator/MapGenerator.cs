using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapGenerator : MonoBehaviour {

    //Public
    public Module startingModule;
    public List<Module> tiles;
    public Transform player;

    //Private
    List<Module> oldModules = new List<Module>();
    Module currentModule;
    Module[] chosenModules;

    int currentLevel = 1;
    // Use this for initialization
    void Start () {
        currentModule = Instantiate(startingModule, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update () {

        if (Vector3.Distance(player.position, currentModule.transform.position) < 80)
        {
            chosenModules = tiles.Where(x => x.level == currentLevel).ToArray();
            Module newModule = Instantiate(chosenModules[Random.Range(0, chosenModules.Length)], Vector3.zero, Quaternion.identity);
            MatchExits(newModule, newModule.GetEntry(), currentModule.GetExit());
            oldModules.Add(currentModule);
            currentModule = newModule;

            if (oldModules.Count > 3)
            {
                Destroy(oldModules[0].gameObject);
                oldModules.RemoveAt(0);
            }

        }
    }

    void MatchExits(Module newModule, Transform newEntry, Transform oldExit)
    {
        if (oldExit.transform.forward == newEntry.transform.forward)
        {
            newModule.transform.Rotate(newEntry.transform.up, 30);
        }

        Quaternion rotZ = Quaternion.FromToRotation(newEntry.transform.forward, -oldExit.transform.forward);
        newModule.transform.rotation = rotZ * newModule.transform.rotation;

        if (oldExit.transform.up == -newEntry.transform.up)
        {
            newModule.transform.Rotate(newEntry.transform.forward, 30);
        }

        Quaternion rotY = Quaternion.FromToRotation(newEntry.transform.up, oldExit.transform.up);
        newModule.transform.rotation = rotY * newModule.transform.rotation;
       
        Vector3 translation = oldExit.position - newEntry.position;
        newModule.transform.position += translation;
    }
}
