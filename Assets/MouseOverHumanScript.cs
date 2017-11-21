using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverHumanScript : MonoBehaviour {

    private GameObject IconSelected;

    void Start()
    {
        IconSelected = GameObject.Find("IconHumanSelected");
    }

	void OnMouseOver () {
        Debug.Log("OMG it's OVER 9000");
        IconSelected.SetActive(true);
    }
	
	void OnMouseExit () {
        IconSelected.SetActive(false);
    }
}
