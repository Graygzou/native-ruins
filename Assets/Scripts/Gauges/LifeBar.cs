using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour {

    public GameObject Life;
    public GameObject Hunger;
    public GameObject Terrain;
    public GameObject Judy;
    public Color BarColor;

    private int timeMaxFaim = 3600;
    private int currentTimeFaim = 0;


    // Use this for initialization
    void Start () {
        Life.transform.Find("Mask").Find("Sprite").GetComponent<Image>().color = Color.red;
	}

    // Update is called once per frame (60 frames)
    void FixedUpdate () {
        //*****************PERTE DE VIE******************//
        //Si Judy chute 
       if (Judy.GetComponent<Rigidbody>().velocity.y < 0 && Judy.GetComponent<Rigidbody>().velocity.magnitude > 100f)
        {
            Life.GetComponent<Scrollbar>().size -= 0.005f; 
        }

        //Si la barre de faim est vide
        if(Hunger.GetComponent<Scrollbar>().size == 0f)
        {
            if(currentTimeFaim == timeMaxFaim)
            {
                Life.GetComponent<Scrollbar>().size -= 0.1f; //valeur en pourcent (0.1f = 10%)
                currentTimeFaim = 0;
            }
            currentTimeFaim++;
        }

        //Pour une attaque animale, cela ce trouve dans les scripts des animaux quand ils se trouvent dans l'etat attaque

        //*****************RECUPERATION DE VIE******************//

    }
}
