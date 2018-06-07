using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour {

    LayerMask slideMask;
    PlayerSounds playerSounds;

    bool hasPlayedBoneBreakSound;

    private void Awake()
    {
        slideMask = GetComponent<RigidbodyController>().slideMask;
        playerSounds = GetComponent<PlayerSounds>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if(slideMask != (slideMask | (1 << collision.gameObject.layer)))
        {
            if(!hasPlayedBoneBreakSound)
            {
                playerSounds.PlayBoneCrackSound();
                hasPlayedBoneBreakSound = true;
            }
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
                    if (!hasPlayedBoneBreakSound)
                    {
                        playerSounds.PlayBoneCrackSound();
                        hasPlayedBoneBreakSound = true;
                    }
                }
            }
            

        }
    }
}
