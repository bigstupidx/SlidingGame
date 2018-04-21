using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    //Public
    public List<Module> tiles;
    public Transform player;

    //Private
    Module currentModule;

    List<Module> oldModules = new List<Module>();
	// Use this for initialization
	void Start () {
        currentModule = Instantiate(tiles[0], Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update () {

        if (Vector3.Distance(player.position, currentModule.transform.position) < 30)
        {
            Module newModule = Instantiate(tiles[Random.Range(0,tiles.Count)], Vector3.zero, Quaternion.identity);
            MatchExits(newModule, newModule.GetEntry(), currentModule.GetExit());
            oldModules.Add(currentModule);
            currentModule = newModule;

            if (oldModules.Count > 3)
            {
                Debug.Log("stroying");
                Destroy(oldModules[0].gameObject);
                oldModules.RemoveAt(0);
            }
        }
    }

    void MatchExits(Module newModule, Transform newEntry, Transform oldExit)
    {
        Quaternion rotZ = Quaternion.FromToRotation(newEntry.transform.forward, -oldExit.transform.forward);
        newModule.transform.rotation = rotZ * newModule.transform.rotation;
        Quaternion fixedRot = Quaternion.LookRotation(newModule.transform.forward, oldExit.transform.up);
        newModule.transform.rotation = fixedRot;

        Vector3 translation = oldExit.position - newEntry.position;
        newModule.transform.position += translation;
    }
}
