using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour {

    public GameObject Hunger;
    public Color BarColor;

    private int currentTimeFaim = 0;
    private int timeMaxFaim = 3600;

	// Use this for initialization
	void Start () {
        Hunger.transform.Find("Mask").Find("Sprite").GetComponent<Image>().color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
        if (currentTimeFaim == timeMaxFaim)
        {
            Hunger.GetComponent<Scrollbar>().size -= 0.02f;
            currentTimeFaim = 0;
        }
        currentTimeFaim++;
    }
}
