using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchObject : MonoBehaviour {

    private List<Switch> components;

	// Use this for initialization
	void Start () {
        components = new List<Switch>();
        foreach (Transform child in gameObject.transform)
            components.Add(child.GetComponent<Switch>());
    }
	
	// Method used to launch the mecanism.
	void Activate () {
        // Get all the children of the current component and activate all of them.
        foreach (Switch child in components)
            child.Activate();
	}
}
