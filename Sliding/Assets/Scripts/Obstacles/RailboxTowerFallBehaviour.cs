using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailboxTowerFallBehaviour : MonoBehaviour, IInitializable {

    public ParticleSystem sparkles;
    public Rigidbody tower;
    public float fallForce = 5;
    public float torqueForce = 5;
    public Vector3 fallDirection;
    public Vector3 torqueDirection;
    public bool relativeDirection = true;
    bool trigger = false;
    Vector3 towerPosition;

    void Awake()
    {
        towerPosition = tower.transform.localPosition;
        sparkles.Stop();
    }

    public void Initialize()
    {
        tower.isKinematic = true;
        tower.transform.localPosition = towerPosition;
        tower.transform.localEulerAngles = Vector3.zero;
        trigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!trigger  && other.tag == "Player")
        {
            StartCoroutine(TowerFall());
            trigger = true;
        }
    }

    IEnumerator TowerFall()
    {
        sparkles.Play();
        tower.isKinematic = false;
        Vector3 actualForce = relativeDirection ? tower.transform.TransformDirection(fallDirection) * fallForce * tower.mass : fallDirection * fallForce * tower.mass;
        Vector3 actualTorque = relativeDirection ? tower.transform.TransformDirection(torqueDirection) * torqueForce * tower.mass : torqueDirection * torqueForce * tower.mass;
        tower.AddForce(actualForce);
        tower.AddTorque(actualTorque);

        float timer = 0;
        float timeToApplyGravity = 10;

        while(timer < timeToApplyGravity)
        {
            timer += Time.deltaTime;
            tower.AddForce(-transform.up * 9.8f * tower.mass);
            yield return null;
        }
    }
}
