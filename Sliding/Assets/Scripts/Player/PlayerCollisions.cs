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
            foreach(ContactPoint c in collision.contacts)
            {
                var relativePosition = transform.InverseTransformPoint(c.point);

                if (relativePosition.y >= -0.5f)
                {
                    GameManager.Instance.isDead = true;
                }
            }
            

        }
    }
}
