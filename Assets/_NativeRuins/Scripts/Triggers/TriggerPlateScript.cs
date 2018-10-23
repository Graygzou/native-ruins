using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlateScript : MonoBehaviour {

    public LogSwitch ScriptRondin;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "BearPlayer")
        {
            ScriptRondin.StartRotation();
        }
        
    }

    public void OnTriggerStay(Collider other)
    {
        
    }

    public void OnTriggerExit(Collider other)
    {
        ScriptRondin.CancelRotation();
    }
}
