using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceDoorTrigger : MonoBehaviour {

    public Transform doorUp;
    public Transform doorDown;

    bool trigger;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !trigger)
        {
            StartCoroutine(OpenDoor());
        }
    }

    IEnumerator OpenDoor()
    {
        float timer = 0;
        float time = 1;

        Vector3 initialUpPos = doorUp.position;
        Vector3 finalUpPos = doorUp.position + new Vector3(0, 12, 0);
        Vector3 initialDownPos = doorDown.position;
        Vector3 finalDownPos = doorDown.position + new Vector3(0, -12, 0);

        while (timer < time)
        {
            timer += Time.deltaTime;
            doorUp.position = Vector3.Lerp(initialUpPos, finalUpPos, timer / time);
            doorDown.position = Vector3.Lerp(initialDownPos, finalDownPos, timer / time);

            yield return null;
        }
    }
}
