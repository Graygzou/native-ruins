using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour {

    #region Consts
    private const float MAX_LIFE_PLAYER = 300f;
    #endregion

    #region Components setting
    [SerializeField]
    private RectTransform lifeSprite;

    [SerializeField]
    private HungerBar hungerBar;
    #endregion

    private float currentTimeFaim = 0.0f;
    private float currentTimeHeart = 0.0f;

    #region FX audioClips
    [Header("Component settings")]
    [SerializeField]
    private AudioClip sonCri;
    [SerializeField]
    private AudioClip sonManger;
    [SerializeField]
    private AudioClip sonVieBasse;
    #endregion

    private AudioSource audio;
    public GameObject Hunger;
    public Animator animator;

    #region Scale parameters
    [Header("Health settings")]
    [SerializeField]
    private int timeMaxBeforeHungerHurt = 15;

    [SerializeField]
    private int timeMaxBeforeHeartBeat = 1;

    [SerializeField]
    private float hurtByFallScale = 0.5f;

    [SerializeField]
    private float hurtByHungerScale = 3f;
    #endregion

    private GameObject playerRoot;
    private ActionsNew actions;
    private bool isWeak;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
        playerRoot = GameObject.FindWithTag ("Player");
		actions = playerRoot.GetComponent ("ActionsNew") as ActionsNew;
        isWeak = false;
     }

    private void Update()
    {
        //Si la barre de faim est vide
        if (hungerBar.GetSizeHungerBar() <= 0.0f)
        {
            currentTimeFaim += Time.deltaTime;
            if (currentTimeFaim >= timeMaxBeforeHungerHurt)
            {
                ChangeLifeBar(-hurtByHungerScale); //valeur en pourcent (0.1f = 10%)
                currentTimeFaim = 0.0f;
            }
            
        }
        else
        {
            currentTimeFaim = 0.0f;
        }
        Weak();
    }

    void FixedUpdate () {
        // Si Judy chute 
        // Should not be in the FixedUpdate
        if (playerRoot.GetComponent<Rigidbody>().velocity.y < 0 && playerRoot.GetComponent<Rigidbody>().velocity.magnitude > 100f)
        {
            Debug.Log(lifeSprite.sizeDelta.y);
            Debug.Log("Player Fall. Old life =" + lifeSprite.sizeDelta.x + ", New life = " + (lifeSprite.sizeDelta.x - hurtByFallScale));
            ChangeLifeBar(-hurtByFallScale);
        }
    }

    public float GetCurrentSizeLifeBar()
    {
        return lifeSprite.sizeDelta.x;
    }

    private void SetSizeLifeBar(float size)
    {
        float newLifeValue = Mathf.Clamp(size, 0f, MAX_LIFE_PLAYER);
        lifeSprite.sizeDelta = new Vector2(newLifeValue, lifeSprite.sizeDelta.y);

        //Si Judy a sa barre de vie à 0 : MORT
        if (GetCurrentSizeLifeBar() <= 0f)
        {
            Death();
        }
    }

    private void ChangeLifeBar(float amount)
    {
        SetSizeLifeBar(lifeSprite.sizeDelta.x + amount);
    }

    public void RestoreLifeFromData(float amount)
    {
        SetSizeLifeBar(amount * MAX_LIFE_PLAYER);
    }

    public bool IsFull()
    {
        return GetCurrentSizeLifeBar() >= MAX_LIFE_PLAYER;
    }

    public void Weak()
    {
        if (this.GetCurrentSizeLifeBar() <= 0.3f)
        {
            currentTimeHeart += Time.deltaTime;
            if (currentTimeHeart >= timeMaxBeforeHeartBeat)
            {
                isWeak = !isWeak;
                if(!isWeak)
                {
                    audio.PlayOneShot(sonVieBasse);
                }
                animator.SetBool("isWeak", isWeak);
                currentTimeHeart = 0.0f;
            }
        }
        else
        {
            currentTimeHeart = 0.0f;
        }
    }

    private void Death()
    {
        playerRoot.GetComponent<PlayerProperties>().KillPlayer();
        actions.Death();
        audio.Stop();
        GameObject.Find("Affichages/Menus/Menu_game_over").SetActive(true);
    }

    /*
     * Recuperation de vie
     */
    public void Eat(float lifeBack)
    {
        audio.PlayOneShot(sonManger);
        if (!IsFull())
        {
            ChangeLifeBar(lifeBack * MAX_LIFE_PLAYER / 100f);
        }
        else
        {
            hungerBar.ChangeHungerBar(lifeBack * MAX_LIFE_PLAYER / 100f);
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
        if (playerRoot.GetComponent<FormsController>().GetCurrentForm() == (int)Forms.id_puma)
        {
            ChangeLifeBar(-(lifeLoosed + lifeLoosed * 0.5f));
        }
        else if (playerRoot.GetComponent<FormsController>().GetCurrentForm() == (int)Forms.id_human)
        {
            actions.Damage();
            ChangeLifeBar(-lifeLoosed);
        } //si forme ours, 25% de degats en moins
        else if (playerRoot.GetComponent<FormsController>().GetCurrentForm() == (int)Forms.id_bear)
        {
            ChangeLifeBar(-(lifeLoosed - lifeLoosed * 0.25f));
        }
    }
}
