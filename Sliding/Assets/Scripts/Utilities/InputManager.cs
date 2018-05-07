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

        if(Input.GetMouseButton(0))
        {
            if (Input.mousePosition.x > (Screen.width / 3) * 2)
            {
                horizontalValue += Time.deltaTime * sensivity;
            }
            else if (Input.mousePosition.x < (Screen.width / 3))
            {
                horizontalValue -= Time.deltaTime * sensivity;
            }
        }
        else
        {
            horizontalValue = Mathf.Lerp(horizontalValue, 0, Time.deltaTime * gravity);
            if(Mathf.Abs(horizontalValue) < dead)
            {
                horizontalValue = 0;
            }
        }
        horizontalValue = Mathf.Clamp(horizontalValue, -1, 1);        
    }

    public static float GetHorizontalInput()
    {
        return horizontalValue;
    }

    public static bool GetJumpInput()
    {
        if (Input.GetMouseButtonDown(0))
        { 
            if (Input.mousePosition.x <= (Screen.width / 3) * 2 && Input.mousePosition.x >= (Screen.width / 3))
            {
                return true;
            }
        }
        return false;
    }
}
