using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forms : MonoBehaviour {

    public enum forms { human, bear, puma };
    public int currentForm;

	// Use this for initialization
	void Start () {
        currentForm = (int)forms.bear;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
