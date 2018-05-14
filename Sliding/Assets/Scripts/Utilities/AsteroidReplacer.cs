using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AsteroidReplacer : MonoBehaviour {

    public GameObject[] asteroids;

    bool coroutineRunning = false;

    List<Transform> newAsteroids = new List<Transform>();

    public void Replace()
    {
        Replacee();
    }

    void Replacee()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject go = transform.GetChild(i).gameObject;

            for (int j = 0; j < asteroids.Length; j++)
            {
                if (go.name.Contains(asteroids[j].name))
                {
                    GameObject newAsteroid = Instantiate(asteroids[j]);
                    newAsteroid.transform.parent = go.transform;
                    newAsteroid.transform.localScale = Vector3.one;
                    newAsteroid.transform.localPosition = Vector3.zero;
                    newAsteroid.transform.localEulerAngles = Vector3.zero;
                    if(newAsteroid.transform.lossyScale.x > 1)
                        newAsteroid.transform.GetChild(0).localScale = new Vector3(1 + 0.10f/ newAsteroid.transform.lossyScale.x, 
                            1 + 0.10f / newAsteroid.transform.lossyScale.y, 1 + 0.10f / newAsteroid.transform.lossyScale.z);
                    newAsteroids.Add(newAsteroid.transform);
                    break;
                }
            }
        }

        foreach(Transform t in newAsteroids)
        {
            t.parent = this.transform;
        }

        DestroyImmediate(this);
    }
}
