using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour {

    public GameObject Energy;
    public Color BarColor;
	private GameObject Player;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player");
        Energy.transform.Find("Mask").Find("Sprite").GetComponent<Image>().color = Color.yellow;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.LeftShift) && !Player.GetComponent<MovementController>().energyIsAt0)
        {
            Energy.GetComponent<Scrollbar>().size -= 0.001f;
        }
        else
        {
            Energy.GetComponent<Scrollbar>().size += 0.002f;
            if(Energy.GetComponent<Scrollbar>().size >= 1f)
            {
                Player.GetComponent<MovementController>().energyIsAt0 = false;
            }
        }
	}
}
