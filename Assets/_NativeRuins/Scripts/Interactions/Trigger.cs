using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trigger : MonoBehaviour
{
    public virtual void Fire()
    {

    }

    public virtual void Interrupt()
    {
        StopAllCoroutines();
    }
}
