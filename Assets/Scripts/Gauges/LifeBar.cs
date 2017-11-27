using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour {

    public GameObject Life;
    public GameObject Hunger;
    public Color BarColor;

    private AudioSource sonCri;
    private AudioSource sonManger;
    private int timeMaxFaim = 3600;
    private int currentTimeFaim = 0;
	private GameObject Judy;
	private ActionsNew actions;
    private bool setterDemo;

    // Use this for initialization
    void Start () {
        AudioSource[] sons = GetComponents<AudioSource>();
        sonCri = sons[0];
        sonManger = sons[1];
		Judy = GameObject.FindWithTag ("Player");
		actions = Judy.GetComponent ("ActionsNew") as ActionsNew;
        Life.transform.Find("Mask").Find("Sprite").GetComponent<Image>().color = Color.red;
        
        setterDemo = true;
    }

    // Update is called once per frame (60 frames)
    void FixedUpdate () {
        if (setterDemo)
        {
            Life.GetComponent<Scrollbar>().size = 0.7f;
            setterDemo = false;
        }
        //*****************PERTE DE VIE******************//
        //Si Judy a sa barre de vie à 0 : Mort
        if (Life.GetComponent<Scrollbar>().size <= 0f) {
            Judy.GetComponent<MovementController>().setDeath(true);
            GameObject playerRoot = GameObject.Find("Player");
            playerRoot.GetComponent<FormsController>().Transformation(0);
            actions.Death();
            sonCri.Stop();
            GameObject.Find("Affichages/Menus/Menu_game_over").SetActive(!GameObject.Find("Affichages/Menus/Menu_game_over").activeSelf);
        }

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

    }

    public float getSizeLifeBar()
    {
        return Life.GetComponent<Scrollbar>().size;
    }

    public void setSizeLifeBar(float size)
    {
        Life.GetComponent<Scrollbar>().size = size;
    }

    //*****************RECUPERATION DE VIE******************//
    public void Eat(float lifeBack)
    {
		sonManger.Play ();
        if (Hunger.GetComponent<Scrollbar>().size >= 1f)
        {
            Life.GetComponent<Scrollbar>().size += lifeBack;
        } else
        {
            Hunger.GetComponent<Scrollbar>().size += lifeBack;
        }
    }

    //*****************SE FAIT ATTAQUER******************//
    public void TakeDamages(float lifeLoosed)
    {
        GameObject playerRoot = GameObject.Find("Player");
        sonCri.Play();
        //si forme puma, 50% de degats en plus
        if (playerRoot.GetComponent<FormsController>().getCurrentForm() == (int)Forms.id_puma) {
            Life.GetComponent<Scrollbar>().size -= lifeLoosed + lifeLoosed *0.5f;
        }
        else if (playerRoot.GetComponent<FormsController>().getCurrentForm() == (int)Forms.id_human) {
            actions.Damage();
            Life.GetComponent<Scrollbar>().size -= lifeLoosed;
        } //si forme ours, 25% de degats en moins
        else if (playerRoot.GetComponent<FormsController>().getCurrentForm() == (int)Forms.id_bear) {
            Life.GetComponent<Scrollbar>().size -= lifeLoosed - lifeLoosed*0.25f;
        }
    }
}
