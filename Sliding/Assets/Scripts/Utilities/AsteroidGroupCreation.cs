using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AsteroidGroupCreation : MonoBehaviour {

    public List<GameObject> asteroids;

    public int asteroidAmount;
    public Vector3 spawnArea;
    public float minScale;
    public float maxScale;

    

    List<GameObject> oldAsteroids = new List<GameObject>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void OnEnable () {

        if (oldAsteroids.Count > 0)
        {
            foreach(GameObject go in oldAsteroids)
            {
                DestroyImmediate(go);
            }
            oldAsteroids.Clear();
        }

        for(int i = 0; i < asteroidAmount; i++)
        {
            GameObject clone = Instantiate(asteroids[Random.Range(0,asteroids.Count)], transform.position, Quaternion.identity);
            clone.transform.parent = this.transform;
            float randomScale = Random.Range(minScale, maxScale);
            clone.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            clone.transform.localPosition = new Vector3(Random.Range(-spawnArea.x, spawnArea.x), Random.Range(-spawnArea.y, spawnArea.y), Random.Range(-spawnArea.z, spawnArea.z));
            clone.transform.localEulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            oldAsteroids.Add(clone);
        }
        
        
	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, .3f);
        Gizmos.DrawCube(transform.position, spawnArea * 2);
    }
}
