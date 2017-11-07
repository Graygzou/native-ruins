using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour {

    public GameObject Energy;
    public Color BarColor;
    public GameObject Judy;

	// Use this for initialization
	void Start () {
        Energy.transform.Find("Mask").Find("Sprite").GetComponent<Image>().color = Color.yellow;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.LeftShift) && !Judy.GetComponent<MovementController>().energyIsAt0)
        {
            Energy.GetComponent<Scrollbar>().size -= 0.001f;
        }
        else
        {
            Energy.GetComponent<Scrollbar>().size += 0.001f;
        }
	}
}
