using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBase : MonoBehaviour {

    public float cooldown;
    public bool used;

    public virtual void Use()
    { }
}
