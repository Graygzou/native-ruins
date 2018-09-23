using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour {

    #region Consts
    private const float MAX_LIFE_PLAYER = 300f;

    private const int timeMaxFaim = 3600;
    private int currentTimeFaim = 0;
    private const int timeMaxHeart = 60;
    private int currentTimeHeart = 0;
    #endregion

    #region Components
    [SerializeField]
    private RectTransform lifeSprite;
    [SerializeField]
    private HungerBar hungerBar;

    private AudioSource audio;
    #endregion

    #region FX audioClips
    [Header("Component settings")]
    [SerializeField]
    private AudioClip sonCri;
    [SerializeField]
    private AudioClip sonManger;
    [SerializeField]
    private AudioClip sonVieBasse;
    #endregion

    public GameObject Hunger;
    public Animator animator;

    #region Scale parameters
    [Header("Health settings")]
    [SerializeField]
    private float hurtByFallScale = 0.005f;
    [SerializeField]
    private float hurtByHungerScale = 0.1f;
    #endregion

    private GameObject judy;
    private ActionsNew actions;
    private bool isWeak;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
		judy = GameObject.FindWithTag ("Player");
		actions = judy.GetComponent ("ActionsNew") as ActionsNew;
        isWeak = false;
     }

    // Update is called once per frame (60 frames)
    void FixedUpdate () {
        //*****************PERTE DE VIE******************//
        //Si Judy a sa barre de vie à 0 : Mort
        if (this.GetCurrentSizeLifeBar() <= 0f)
        {
            judy.GetComponent<MovementController>().setDeath(true);
            GameObject playerRoot = GameObject.Find("Player");
            playerRoot.GetComponent<FormsController>().Transformation(0);
            actions.Death();
            audio.Stop();
            GameObject.Find("Affichages/Menus/Menu_game_over").SetActive(!GameObject.Find("Affichages/Menus/Menu_game_over").activeSelf);
        }

        // Si Judy chute 
        if (judy.GetComponent<Rigidbody>().velocity.y < 0 && judy.GetComponent<Rigidbody>().velocity.magnitude > 100f)
        {
            Debug.Log(lifeSprite.sizeDelta.y);
            Debug.Log("Player Fall. Old life =" + lifeSprite.sizeDelta.x + ", New life = " + (lifeSprite.sizeDelta.x - hurtByFallScale));
            ChangeLifeBar(-hurtByFallScale);
        }

        //Si la barre de faim est vide
        if(hungerBar.GetSizeHungerBar() == 0f)
        {
            if(currentTimeFaim == timeMaxFaim)
            {
                ChangeLifeBar(-hurtByHungerScale); //valeur en pourcent (0.1f = 10%)
                currentTimeFaim = 0;
            }
            currentTimeFaim++;
        }

        Weak();

    }

    /*
     * Should be 100 max ? Works ?
     */
    public float GetCurrentSizeLifeBar()
    {
        return lifeSprite.sizeDelta.x;
    }

    public void SetSizeLifeBar(float size)
    {
        float newLifeValue = Mathf.Clamp(size, 0f, MAX_LIFE_PLAYER);
        lifeSprite.sizeDelta = new Vector2(newLifeValue, lifeSprite.sizeDelta.y);
    }

    private void ChangeLifeBar(float amount)
    {
        SetSizeLifeBar(lifeSprite.sizeDelta.x + amount);
    }

    public void Weak()
    {
        if (this.GetCurrentSizeLifeBar() <= 0.3f)
        {
            if (currentTimeHeart == timeMaxHeart)
            {
                isWeak = !isWeak;
                if(!isWeak)
                {
                    audio.PlayOneShot(sonVieBasse);
                }
                animator.SetBool("isWeak", isWeak);
                currentTimeHeart = 0;
            }
            currentTimeHeart++;
               
        }
    }

    /*
     * Recuperation de vie
     */
    public void Eat(float lifeBack)
    {
        audio.PlayOneShot(sonManger);
        if (Hunger.GetComponent<Scrollbar>().size >= 1f)
        {
            ChangeLifeBar(lifeBack);
        }
        else
        {
            hungerBar.SetSizeHungerBar(lifeBack);
        }
    }

    /* 
     * Se Fait attaquer
     */
    public void TakeDamages(float lifeLoosed)
    {
        GameObject playerRoot = GameObject.Find("Player");
        audio.PlayOneShot(sonCri);
        //si forme puma, 50% de degats en plus
        if (playerRoot.GetComponent<FormsController>().getCurrentForm() == (int)Forms.id_puma)
        {
            ChangeLifeBar(-(lifeLoosed + lifeLoosed * 0.5f));
        }
        else if (playerRoot.GetComponent<FormsController>().getCurrentForm() == (int)Forms.id_human)
        {
            actions.Damage();
            ChangeLifeBar(-lifeLoosed);
        } //si forme ours, 25% de degats en moins
        else if (playerRoot.GetComponent<FormsController>().getCurrentForm() == (int)Forms.id_bear)
        {
            ChangeLifeBar(-(lifeLoosed - lifeLoosed * 0.25f));
        }
    }
}
