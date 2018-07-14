using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPooler : MonoBehaviour {

    public List<Module> platforms;
    public MapGenerator mapGen;
    public int spawnAmountPerPlatform = 3;

    Module[] chosenModules;

    public Module GetObject(int level)
    {
        chosenModules = platforms.Where(x => x.level <= level).ToArray();
        Module selectedModule = chosenModules[Random.Range(0, chosenModules.Length)];
        platforms.Remove(selectedModule);
        selectedModule.gameObject.SetActive(true);
        //selectedModule.transform.parent = null;
        selectedModule.Initialize();
        return selectedModule;
    }

    public void AddObject(Module m)
    {
        m.transform.position = transform.position;
        //m.transform.parent = transform;
        m.gameObject.SetActive(false);
        platforms.Add(m);
    }

    public void InstantiatePlatforms()
    {
        var children = new List<GameObject>();

        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }

        foreach(GameObject g in children)
        {
            DestroyImmediate(g);
        }


        platforms.Clear();

        foreach (Module m in mapGen.platforms)
        {
            for (int i = 0; i < spawnAmountPerPlatform; i++)
            {
                Module newModule = Instantiate(m);
                newModule.transform.position = transform.position;
                newModule.transform.parent = this.transform;
                platforms.Add(newModule);
            }
        }
    }
}
