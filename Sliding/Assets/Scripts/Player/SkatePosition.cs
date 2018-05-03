using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkatePosition : MonoBehaviour {

    public Transform feetPivot;
    public Vector3 offset;
    public float timeToLerp;
    public float timeToAdjustTransform;
    public float timeBetweenLerps;

    public Vector3 feetOffset;
    [HideInInspector]
    public Vector3 startingPosition;

    IEnumerator currentRoutine;

	// Use this for initialization
	void Start () {
        startingPosition = transform.localPosition;
	}

    public void BounceSkate()
    {
        if(currentRoutine != null)
            StopCoroutine(currentRoutine);
        StartCoroutine(currentRoutine = SkateLerpBounce());
    }

    public void SetSkateBelowFeet()
    {
        transform.parent = feetPivot;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);
        StartCoroutine(currentRoutine = LerpAtFeet());
    }

    IEnumerator LerpAtFeet()
    {
        Vector3 startingPosition = transform.localPosition;
        Quaternion startingRotation = transform.localRotation;
        float timer = 0;
        float time = .2f;

        while (timer < time)
        {
            timer += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startingPosition, Vector3.zero, timer / time);
            transform.localRotation = Quaternion.Lerp(startingRotation, Quaternion.Euler(Vector3.zero), timer / time);
            yield return null;
        }
    }

    IEnumerator SkateLerpBounce()
    {
        float timer = 0;
        Vector3 currentPosition = transform.localPosition;
        Quaternion currentRotation = transform.localRotation;

        while (timer < timeToAdjustTransform)
        {
            timer += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(currentPosition, startingPosition, timer / timeToAdjustTransform);
            transform.localRotation = Quaternion.Lerp(currentRotation, Quaternion.Euler(Vector3.zero), timer / timeToAdjustTransform);
            yield return null;
        }


        timer = 0;
        Vector3 endPosition = startingPosition + offset;

        while (timer < timeToLerp)
        {
            timer += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startingPosition, endPosition, timer / timeToLerp);
            yield return null;
        }

        timer = 0;

        yield return new WaitForSeconds(timeBetweenLerps);

        while (timer < timeToLerp)
        {
            timer += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(endPosition, startingPosition, timer / timeToLerp);
            yield return null;
        }
    }

    
}
