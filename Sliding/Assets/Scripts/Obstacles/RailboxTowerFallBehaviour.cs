using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailboxTowerFallBehaviour : MonoBehaviour, IInitializable {

    public AudioSource source;
    public AudioClip[] clips;
    public ParticleSystem sparkles;
    public Rigidbody tower;
    public Transform targetTransform;
    //public float fallForce = 5;
    //public float torqueForce = 5;
    //public Vector3 fallDirection;
    //public Vector3 torqueDirection;
    //public bool relativeDirection = true;

    public AnimationCurve movement;
    public AnimationCurve rotation;

    public float animationTime = 3;
    bool trigger = false;
    Vector3 initialPosition;
    CameraShake cam;

    //Vector3 actualForce;
    //Vector3 actualTorque;

    //float timer = 0;
    //float timeToApplyGravity = 10;

    Quaternion initialRotation;
    Vector3 initialWorldPosition;

    void Awake()
    {
        initialPosition = tower.transform.localPosition;
        cam = Camera.main.GetComponent<CameraShake>();
    }

    public void Initialize()
    {
        tower.isKinematic = true;
        tower.transform.localPosition = initialPosition;
        tower.transform.localEulerAngles = Vector3.zero;
        trigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!trigger  && other.tag == "Player")
        {
            StartCoroutine(TowerFall());
            trigger = true;
            cam.ShakeCamera(20, 0.5f);
            sparkles.Play();
            source.PlayOneShot(clips[0]);
        }
    }

    IEnumerator TowerFall()
    {
        tower.isKinematic = false;


        float timer = 0;
        bool playSound = false;

        initialRotation = Quaternion.Euler(tower.transform.rotation.eulerAngles);
        initialWorldPosition = tower.transform.position;

        while(timer < animationTime)
        {
            timer += Time.deltaTime;
            tower.MovePosition(Vector3.Lerp(initialWorldPosition, targetTransform.position, movement.Evaluate(timer / animationTime)));
            tower.MoveRotation(Quaternion.Lerp(initialRotation, targetTransform.rotation, rotation.Evaluate(timer / animationTime)));

            if(!playSound && timer > animationTime - 0.6f)
            {
                playSound = true;
                source.PlayOneShot(clips[1]);
            }
            yield return null;
        }
    }

    //IEnumerator TowerFall()
    //{
    //    tower.isKinematic = false;
    //    yield return null;
    //    actualForce = relativeDirection ? tower.transform.TransformDirection(fallDirection) * fallForce * tower.mass : fallDirection * fallForce * tower.mass;
    //    //actualTorque = relativeDirection ? tower.transform.TransformDirection(torqueDirection) * torqueForce * tower.mass : torqueDirection * torqueForce * tower.mass;
    //    tower.AddForce(actualForce);
    //    //tower.AddTorque(actualTorque);
    //    //source.Play();

    //    //timer = 0;

    //    yield return null;

    //    //while (timer < timeToApplyGravity)
    //    //{
    //    //    timer += Time.deltaTime;
    //    //    tower.AddForce(-transform.up * 9.8f * tower.mass);
    //    //    yield return null;
    //    //}
    //}
}
