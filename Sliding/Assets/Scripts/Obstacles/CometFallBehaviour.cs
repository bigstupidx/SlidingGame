using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometFallBehaviour : MonoBehaviour, IInitializable {

    public Transform cometLanding;
    public Transform comet;
    public GameObject explosion;
    public RotateBehaviour cometRotation;
    public Vector3 finalExplosionScale;
    public float timeToLand = 1;
    public bool landPositionStatic;

    public AudioClip[] sounds;
    public AudioSource source;

    Rigidbody player;
    Vector3 cometLandingOriginalPos;
    Vector3 cometOriginalPos;
    float timer = 0;
    bool trigger;

    private void Awake()
    {
        player = Camera.main.GetComponent<ThirdPersonCamera>().target.GetComponent<Rigidbody>();

        cometLandingOriginalPos = cometLanding.localPosition;

        cometOriginalPos = comet.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!trigger && other.tag == "Player")
        {
            StartCoroutine(CometCoroutine());
            trigger = true;
        }

    }

    IEnumerator CometCoroutine()
    {
        source.PlayOneShot(sounds[0]);

        if(!landPositionStatic)
        {
            Vector3 traslation = transform.forward * player.velocity.magnitude * timeToLand;
            traslation += transform.forward * 2;
            traslation += transform.right * Random.Range(-8, +8);
            cometLanding.position += traslation;
        }        
        Vector3 startingCometPosition = comet.position;
        cometLanding.gameObject.SetActive(true);

        while (timer < timeToLand)
        {
            timer += Time.deltaTime;
            comet.position = Vector3.Lerp(startingCometPosition, cometLanding.position, timer / timeToLand);
            yield return null;
        }

        cometRotation.enabled = false;
        cometLanding.gameObject.SetActive(false);

        explosion.SetActive(true);

        source.Stop();
        source.PlayOneShot(sounds[1]);

        while ((finalExplosionScale - explosion.transform.localScale).sqrMagnitude > 5)
        {
            explosion.transform.localScale = Vector3.Lerp(explosion.transform.localScale, finalExplosionScale, Time.deltaTime * 5);
            yield return null;
        }

        timer = 0;
        float time = 5;
        float speed = 20;
        float radius = 0.5f;

        Vector3 startingScale = explosion.transform.localScale;

        while (timer < time)
        {
            timer += Time.deltaTime;

            explosion.transform.localScale = startingScale + new Vector3(Mathf.Sin(timer * speed), Mathf.Sin(timer * speed), Mathf.Sin(timer * speed)) * radius;
            yield return null;
        }

        explosion.SetActive(false);
    }

    public void Initialize()
    {
        cometLanding.localPosition = cometLandingOriginalPos;
        comet.localPosition = cometOriginalPos;
        trigger = false;
        timer = 0;
        cometRotation.enabled = true;
        explosion.transform.localScale = Vector3.zero;
    }
}
