using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpSystem : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Escape) && !this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
        }
        if (Input.GetKey(KeyCode.Escape) && this.gameObject.activeSelf) {
            this.gameObject.SetActive(false);
        }

    }
}
