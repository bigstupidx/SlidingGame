using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionTrigger : MonoBehaviour {

    Vector3 forward;
    public Transform A;
    public Transform B;

    private void Start()
    {
        forward = B.position - A.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.transform.rotation = Quaternion.LookRotation(forward, other.transform.up);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(A.position, B.position);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(A.position, .5f);
        Gizmos.DrawSphere(B.position, .5f);
    }
}
