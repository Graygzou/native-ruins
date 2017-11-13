using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchObject : MonoBehaviour {

    // Use this for initialization
    void Start () {
    }
	
	// Method used to launch the mecanism.
	public void Activate () {
        // If the currentObject has a mecanism, active it.
        if (gameObject.GetComponent<Switch>() != null) {
            gameObject.GetComponent<Switch>().Activate();
        } else {
            ActivateChildren();
        }
	}

    public void ActivateChildren() {
        // Get all the children of the current component and activate all of them.
        Switch elem = null;
        foreach (Transform child in gameObject.transform) {
            // Get the child C# script
            elem = child.GetComponent<Switch>();
            if (elem != null) {
                // Call the method to active the mecanism
                elem.Activate();
            }
        }
    }
}
