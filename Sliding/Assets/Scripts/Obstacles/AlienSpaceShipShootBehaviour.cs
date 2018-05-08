using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienSpaceShipShootBehaviour : MonoBehaviour, IInitializable {

    //Public
    public Transform wormHole;
    public Transform spaceShip;
    public Transform shipCannons;
    public Transform[] missiles;
    public Transform[] missilesLandPositions;
    public Vector3 finalWormHoleScale = new Vector3(120, 120, 120);
    public Vector3 finalShipScale = new Vector3(5, 5, 5);
    public Vector3 finalCannonRotation = new Vector3(0, 0, 30);
    public float missileHeight = 5;
    public float missileTimeToLand = 3;
    public float timeBetweenMissiles = 0.5f;
    public GameObject halo;
    public GameObject[] groundFeedbacks;
    public Transform[] explosions;
    public Vector3 finalExplosionScale = new Vector3(10, 10, 10);
    public Vector3 wormHoleMassiveScale = new Vector3(450, 450, 450);
    public bool[] missileLandPositionStatic;
    public ParticleSystem[] shootParticles;

    //Private
    RigidbodyController player;
    CameraShake cam;
    Transform missilesLandPositionsContainer;
    Vector3 shipInitialLocation;
    Vector3[] initialMissilesPositions = new Vector3[3];
    Vector3[] newMissilesLocalPositions = new Vector3[3];
    bool trigger;


    private void Awake()
    {
        cam = Camera.main.GetComponent<CameraShake>();
        player = cam.GetComponent<ThirdPersonCamera>().target.GetComponent<RigidbodyController>();

        shipInitialLocation = spaceShip.localPosition;
        missilesLandPositionsContainer = missilesLandPositions[0].parent;

        for(int i = 0; i < initialMissilesPositions.Length; i++)
        {
            initialMissilesPositions[i] = missiles[i].localPosition;
        }     
    }

    public void Initialize()
    {
        wormHole.localScale = Vector3.zero;
        spaceShip.localScale = Vector3.zero;
        spaceShip.localPosition = shipInitialLocation;
        halo.SetActive(false);
        trigger = false;

        for(int i = 0; i < missiles.Length; i++)
        {
            missiles[i].parent = spaceShip;
            missiles[i].localPosition = initialMissilesPositions[i];
            explosions[i].localScale = Vector3.zero;
            if(!missileLandPositionStatic[i])
                missilesLandPositions[i].localPosition = new Vector3(Random.Range(-8,8), missilesLandPositions[i].localPosition.y, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!trigger && other.tag == "Player")
        {
            StartCoroutine(WarpShip());
            trigger = true;
        }
    }

    IEnumerator WarpShip()
    {
        float timer = 0;
        float timeToLerp = 1;

        while(timer < timeToLerp)
        {
            timer += Time.deltaTime;
            wormHole.localScale = Vector3.Lerp(Vector3.zero, finalWormHoleScale, timer / timeToLerp);
            yield return null;
        }
        spaceShip.localScale = finalShipScale;
        halo.SetActive(true);

        Vector3 shipFinalPosition = shipInitialLocation + Vector3.right * 150;

        while ((shipFinalPosition - spaceShip.localPosition).sqrMagnitude > 420)
        {
            spaceShip.localPosition = Vector3.Lerp(spaceShip.localPosition, shipFinalPosition, Time.deltaTime * 3);
            yield return null;
        }

        timer = 0;
        timeToLerp = 0.2f;

        Quaternion finalRotation = Quaternion.Euler(shipCannons.rotation.eulerAngles + finalCannonRotation);

        while(timer < timeToLerp)
        {
            timer += Time.deltaTime;
            shipCannons.rotation = Quaternion.Lerp(shipCannons.rotation, finalRotation, timer);
            yield return null;
        }


        for (int i = 0; i < missiles.Length; i++)
        {
            missiles[i].parent = missilesLandPositionsContainer;
            newMissilesLocalPositions[i] = missiles[i].localPosition;

            if (!missileLandPositionStatic[i])
            {
                Vector3 translation = Vector3.forward * player.rigidbody.velocity.magnitude * (2f + timeBetweenMissiles + missileTimeToLand);
                translation += Vector3.forward * player.rigidbody.velocity.magnitude * (timeBetweenMissiles + missileTimeToLand) * i;
                missilesLandPositions[i].localPosition += translation;
            }
            yield return null;
        }

        for (int i = 0; i < missiles.Length; i++)
        {
            timer = 0;
            groundFeedbacks[i].SetActive(true);
            yield return new WaitForSeconds(timeBetweenMissiles);
            shootParticles[i].Play();
            while (timer < missileTimeToLand)
            {
                timer += Time.deltaTime;
                missiles[i].localPosition = MathParabola.Parabola(newMissilesLocalPositions[i], missilesLandPositions[i].localPosition, missileHeight, timer / missileTimeToLand);
                yield return null;
            }
            cam.ShakeCamera(20, 0.5f);
            StartCoroutine(ScaleExplosion(i));
            groundFeedbacks[i].SetActive(false);
        }

        timer = 0;
        timeToLerp = 2f;

        while((wormHoleMassiveScale - wormHole.localScale).sqrMagnitude > 20)
        {
            timer += Time.deltaTime;
            wormHole.localScale = Vector3.Lerp(wormHole.localScale, wormHoleMassiveScale, timer / timeToLerp);
            yield return null;
        }
        spaceShip.localScale = Vector3.zero;
        halo.SetActive(false);

        timer = 0;

        while (wormHole.localScale.sqrMagnitude > 0)
        {
            timer += Time.deltaTime;
            wormHole.localScale = Vector3.Lerp(wormHole.localScale, Vector3.zero, timer / timeToLerp);
            yield return null;
        }

        yield return null;
    }

    IEnumerator ScaleExplosion(int i)
    {
        while ((finalExplosionScale - explosions[i].localScale).sqrMagnitude > 5)
        {
            explosions[i].localScale = Vector3.Lerp(explosions[i].localScale, finalExplosionScale, Time.deltaTime * 5);
            yield return null;
        }
        StartCoroutine(ExplosionSine(i));
    }

    IEnumerator ExplosionSine(int i)
    {
        float timer = 0;
        float time = 10;
        float speed = 20;
        float radius = 0.5f;

        Vector3 startingScale = explosions[i].localScale;

        while(timer < time)
        {
            timer += Time.deltaTime;

            explosions[i].localScale = startingScale + new Vector3(Mathf.Sin(timer * speed), Mathf.Sin(timer * speed), Mathf.Sin(timer * speed)) * radius;
            yield return null;
        }
    }
}
