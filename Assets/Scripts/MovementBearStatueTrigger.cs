using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBearStatueTrigger : MonoBehaviour
{

    public MovementBearStatue ScriptMovement;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "BearPlayer")
        {
            ScriptMovement.SetCanBeMoved(true);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "BearPlayer")
        {
            ScriptMovement.SetCanBeMoved(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        ScriptMovement.SetCanBeMoved(false);
    }

}
