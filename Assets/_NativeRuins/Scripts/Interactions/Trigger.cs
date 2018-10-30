using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trigger : MonoBehaviour
{
    public delegate void TriggerFinished();
    public static event TriggerFinished OnTriggerFinish;

    public virtual void Fire() { }

    public virtual void Interrupt()
    {
        StopAllCoroutines();
        NoticeSubscribers();
    }

    protected void NoticeSubscribers()
    {
        // Call the end event
        if (OnTriggerFinish != null)
        {
            OnTriggerFinish();
        }
    }
}
