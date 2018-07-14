using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    static float horizontalValue = 0;
    public float gravity = 0;
    public float dead = 0.01f;
    public float sensivity = 1;

	// Update is called once per frame
	void Update () {

        foreach(Touch t in Input.touches)
        {
            if (t.position.x > (Screen.width / 3) * 2)
            {
                horizontalValue += Time.unscaledDeltaTime * sensivity;
            }
            else if (t.position.x < (Screen.width / 3))
            {
                horizontalValue -= Time.unscaledDeltaTime * sensivity;
            }
        }

        if(Input.touchCount == 0)
        {
            horizontalValue = Mathf.Lerp(horizontalValue, 0, Time.unscaledDeltaTime * gravity);
            if (Mathf.Abs(horizontalValue) < dead)
            {
                horizontalValue = 0;
            }
        }

        horizontalValue = Mathf.Clamp(horizontalValue, -1, 1);

    }

    public static void ResetInputAxis()
    {
        horizontalValue = 0;
    }

    public static float GetHorizontalInput()
    {
        return horizontalValue;
    }

    public static bool GetJumpInput()
    {
        foreach(Touch t in Input.touches)
        {
            if (t.position.x <= (Screen.width / 3) * 2 && t.position.x >= (Screen.width / 3) && t.phase == TouchPhase.Began)
                return true;
        }
        return false;
    }
}
