using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour {

    LayerMask slideMask;

    private void Awake()
    {
        slideMask = GetComponent<RigidbodyController>().slideMask;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(slideMask != (slideMask | (1 << collision.gameObject.layer)))
        {
            GameManager.Instance.isDead = true;
        }
        else
        {
            Vector3 collisionPoint = collision.contacts[0].point;
            Vector3 dir = (collisionPoint - transform.position).normalized;

            if (Vector3.Dot(dir, -transform.up) < 0.8f)
            {
                GameManager.Instance.isDead = true;
            }           
        }
    }
}
