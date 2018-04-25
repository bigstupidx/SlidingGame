using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGravity : MonoBehaviour {

    bool trigger;

    private void OnTriggerEnter(Collider other)
    {
        if(!trigger && other.gameObject.tag == "Player")
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, transform.up);
            //other.transform.rotation = targetRotation;
            StartCoroutine(RotateTarget(other.transform, targetRotation));
            trigger = true;
        }
    }

    IEnumerator RotateTarget(Transform target, Quaternion targetRotation)
    {
        Quaternion startingRotation = Quaternion.Euler(target.rotation.eulerAngles);

        float timer = 0;
        float time = 0.5f;

        while(timer < time)
        {
            timer += Time.deltaTime;
            target.rotation = Quaternion.Lerp(startingRotation, targetRotation, timer / time);
            yield return null;
        }
    }
}
