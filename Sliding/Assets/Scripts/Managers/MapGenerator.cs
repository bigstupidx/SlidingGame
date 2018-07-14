using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MapGenerator : MonoBehaviour {

    //Public
    public Module startingModule;
    public Module tutorialModule;
    public List<Module> platforms;
    public Transform player;
    public ObjectPooler pooler;

    public int modulesBeforeLevel2 = 20;
    public int modulesBeforeLevel3 = 40;

    public Text text;

    //Private
    Queue<Module> queuedModules = new Queue<Module>();
    Module currentModule;
    Module newModule;
    Module[] chosenModules;

    int currentLevel = 1;
    public int moduleCounter = 0;

    bool firstModule = true;

    // Use this for initialization
    void Awake () {
        if(DataManager.gameData.options.playTutorial)
            currentModule = Instantiate(tutorialModule, Vector3.zero, Quaternion.identity);
        else currentModule = Instantiate(startingModule, Vector3.zero, Quaternion.identity);

        currentModule.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update () {

        if ((player.position - currentModule.transform.position).sqrMagnitude < 80 * 80)
        {
            newModule = pooler.GetObject(currentLevel);
            newModule.transform.rotation = currentModule.GetExit().rotation;
            newModule.transform.position = currentModule.GetExit().position;
            queuedModules.Enqueue(currentModule);
            currentModule = newModule;

            if(queuedModules.Count > 3)
            {
                if (firstModule)
                {
                    queuedModules.Dequeue().gameObject.SetActive(false);
                    firstModule = false;
                }
                else pooler.AddObject(queuedModules.Dequeue());
            }

            moduleCounter++;
            if(moduleCounter > modulesBeforeLevel3)
            {
                currentLevel = 3;
            }
            else if(moduleCounter > modulesBeforeLevel2)
            {
                currentLevel = 2;
            }
        }

        text.text = "lvl " + currentLevel + " md " + moduleCounter;
    }

}
