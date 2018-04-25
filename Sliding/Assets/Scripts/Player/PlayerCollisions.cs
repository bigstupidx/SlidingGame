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
    }
}
