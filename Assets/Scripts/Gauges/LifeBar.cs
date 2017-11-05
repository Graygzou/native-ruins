using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour {

    public GameObject Life;
    public Color BarColor;

    //taille barre de vie est 150 mais on fait les calculs pour qu'elle soit sur 100

	// Use this for initialization
	void Start () {
        Life.transform.Find("Mask").Find("Sprite").GetComponent<Image>().color = Color.red;
	}
	
	// Update is called once per frame
	void Update () {
        Life.GetComponent<Scrollbar>().size -= 0.001f;
	}
}
