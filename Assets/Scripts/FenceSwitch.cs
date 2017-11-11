using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceSwitch : Switch {

	// Use this for initialization
	void Start () {
		
	}

    // Used to launch a mechanism
    override public void Activate() {
        Debug.Log("fence up");
    }
}
