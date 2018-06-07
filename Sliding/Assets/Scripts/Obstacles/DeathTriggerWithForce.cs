using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTriggerWithForce : MonoBehaviour, IInitializable {

    public float force;
    public float delay = 0.1f;

    RigidbodyController player;

    bool trigger;

    private void OnTriggerEnter(Collider other)
    {
        if(!trigger && other.tag == "Player")
        {
            player = other.GetComponent<RigidbodyController>();
            StartCoroutine(AddForceWithDelay());
            trigger = true;
        }
    }

    IEnumerator AddForceWithDelay()
    {
        yield return new WaitForSeconds(delay);
        Vector3 dir = (player.transform.position - transform.position).normalized;
        player.AddForce(dir * force, false, true);
        GameManager.Instance.isDead = true;
    }

    public void Initialize()
    {
        trigger = false;
    }
}
