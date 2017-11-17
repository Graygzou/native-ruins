using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlateScript : MonoBehaviour {

    public LogSwitch ScriptRondin;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Object Entered the trigger");
        if (other.gameObject.name == "BearPlayer")
        {
            ScriptRondin.StartRotation();
        }
        
    }

    public void OnTriggerStay(Collider other)
    {
        Debug.Log("Object is within trigger");
        
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("Object exited the trigger");
        ScriptRondin.CancelRotation();
    }
}
