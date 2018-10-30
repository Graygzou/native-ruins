using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
    #region Consts
    public const float MAX_LIFE_PLAYER = 300f;
    public const float MAX_HUNGER_PLAYER = 300f;
    public const float MAX_ENERGY_PLAYER = 180f;
    #endregion

    // Actual State of the player
    [Header("Player States (read only)")]
    [SerializeField] private bool isDead;
    [SerializeField] private bool isSaving;
    [SerializeField] private bool dialogueOn;
    [SerializeField] private bool canRun;

    #region Health settings
    [Header("Health settings")]
    [SerializeField] private int timeMaxBeforeHungerHurt = 15;
    [SerializeField] private int timeMaxBeforeHeartBeat = 1;
    [SerializeField] private float hurtByFallScale = 0.5f;
    [SerializeField] private float hurtByHungerScale = 3f;
    #endregion

    #region Hunger settings
    [Header("Hunger settings")]
    [SerializeField] private int timeMaxBeforeHungerDecrease = 5;
    [SerializeField] private float hungerDecreasingFactor = 0.04f;
    [SerializeField] private float hungerDecreasingFactorPuma = 0.08f;
    #endregion

    #region Energy settings
    [Header("Energy settings")]
    [SerializeField] private float energyDecreasingFactor = 1f;
    [SerializeField] private float energizingBackFactor = 2f;
    [SerializeField] private float energizingBackAfterEmptyFactor = 2f;
    #endregion

    [Header("Childrens Information")]
    [SerializeField] private MovementController[] childrenMovementController;
    [SerializeField] private Animator[] childrenAnimator;

    private float currentTimeHeart = 0.0f;
    private float currentTimeFaim = 0.0f;
    private bool energyIsAt0 = false;

    private MenuManager menuManager;

    private void Awake()
    {
        isDead = false;
        dialogueOn = false;
        isSaving = false;
    }

    void Update()
    {
        if(!dialogueOn && menuManager != null)
        {
            currentTimeFaim += Time.deltaTime;
            if (currentTimeFaim >= timeMaxBeforeHungerDecrease)
            {
                GameObject playerRoot = GameObject.Find("Player");
                if (FormsController.Instance.GetCurrentForm() == (int)Forms.id_puma)
                {
                    menuManager.UpdateHungerBar(-hungerDecreasingFactorPuma);
                }
                else
                {
                    menuManager.UpdateHungerBar(-hungerDecreasingFactor);
                }
                currentTimeFaim = 0.0f;
            }
            if (menuManager.GetSizeHungerBar() <= 0.0f)
            {
                currentTimeFaim += Time.deltaTime;
                if (currentTimeFaim >= timeMaxBeforeHungerHurt)
                {
                    menuManager.UpdateLifeBar(-hurtByHungerScale); //valeur en pourcent (0.1f = 10%)
                    currentTimeFaim = 0.0f;
                }

            }
            else
            {
                currentTimeFaim = 0.0f;
            }
            // Weak(); Included in the animation ??
            MovementController activeMovementController = childrenMovementController[(int)FormsController.TransformationType.Human];
            if (canRun && activeMovementController.IsShiftHold)
            {
                energyIsAt0 = !(menuManager.GetSizeEnergyBar() >= 1f);
                canRun = !energyIsAt0;
                if (Input.GetKey(KeyCode.LeftShift) && canRun)
                {
                    menuManager.UpdateEnergyBar(-energyDecreasingFactor);
                }
                else
                {
                    menuManager.UpdateEnergyBar(energizingBackFactor);
                }
            }
            else
            {
                menuManager.UpdateEnergyBar(energizingBackAfterEmptyFactor);
                canRun = (menuManager.GetSizeEnergyBar() == PlayerProperties.MAX_ENERGY_PLAYER);
            }

            childrenAnimator[FormsController.Instance.GetCurrentForm()].SetFloat("Health", menuManager.GetCurrentSizeLifeBar());
        }
    }

    public void SetMenuManager(MenuManager activeMenuManager)
    {
        menuManager = activeMenuManager;
    }

    /*
    void FixedUpdate()
    {
        // Si Judy chute 
        // Should not be in the FixedUpdate
        if (playerRoot.GetComponent<Rigidbody>().velocity.y < 0 && playerRoot.GetComponent<Rigidbody>().velocity.magnitude > 100f)
        {
            Debug.Log(lifeSprite.sizeDelta.y);
            Debug.Log("Player Fall. Old life =" + lifeSprite.sizeDelta.x + ", New life = " + (lifeSprite.sizeDelta.x - hurtByFallScale));
            ChangeLifeBar(-hurtByFallScale);
        }
    }*/

    /*
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
    }*/



    /*
     * Recuperation de vie
     */
    public void Eat(float lifeBack)
    {
        //audio.PlayOneShot(sonManger);
        if (!menuManager.IsLifeBarFull())
        {
            menuManager.UpdateLifeBar(lifeBack * MAX_LIFE_PLAYER / 100f);
        }
        else
        {
            menuManager.UpdateHungerBar(lifeBack * MAX_LIFE_PLAYER / 100f);
        }
    }

    /* 
     * Se Fait attaquer
     */
    public void TakeDamages(float lifeLoosed)
    {
        GameObject playerRoot = GameObject.Find("Player");
        //audio.PlayOneShot(sonCri);
        //si forme puma, 50% de degats en plus
        if (playerRoot.GetComponent<FormsController>().GetCurrentForm() == (int)Forms.id_puma)
        {
            menuManager.UpdateLifeBar(-(lifeLoosed + lifeLoosed * 0.5f));
        }
        else if (playerRoot.GetComponent<FormsController>().GetCurrentForm() == (int)Forms.id_human)
        {
            //actions.Damage();
            menuManager.UpdateLifeBar(-lifeLoosed);
        } //si forme ours, 25% de degats en moins
        else if (playerRoot.GetComponent<FormsController>().GetCurrentForm() == (int)Forms.id_bear)
        {
            menuManager.UpdateLifeBar(-(lifeLoosed - lifeLoosed * 0.25f));
        }

        //Si Judy a sa barre de vie à 0 : MORT
        if (menuManager.GetCurrentSizeLifeBar() <= 0f)
        {
            KillPlayer();
        }
    }

    #region Death
    public void KillPlayer()
    {
        isDead = true;

        // Retransform Judy in Human
        GetComponent<FormsController>().Transformation(0);

        // UnSubscribe all events.
        foreach(MovementController controller in childrenMovementController)
        {
            controller.DeRegisterInputs();
        }
        /*
        actions.Death();
        audio.Stop();
        GameObject.Find("Affichages/Menus/Menu_game_over").SetActive(true);*/
    }

    public void RevivePlayer()
    {
        isDead = false;

        // Subscribe human movements events back.
        childrenMovementController[(int)FormsController.TransformationType.Human].RegisterInputs();
    }

    public bool IsDeath()
    {
        return isDead;
    }
    #endregion

    #region Dialogues
    public void LaunchDialogue()
    {
        dialogueOn = true;
        childrenMovementController[FormsController.Instance.GetCurrentForm()].DeregisterCameraMovementsInputs();
    }

    public void CloseDialogue()
    {
        dialogueOn = false;
        childrenMovementController[FormsController.Instance.GetCurrentForm()].RegisterCameraMovementsInputs();
    }
    #endregion

    #region Saving
    public void EnableSaving()
    {
        isSaving = true;
        childrenMovementController[FormsController.Instance.GetCurrentForm()].DeregisterPlayerMovementsInputs();
    }

    public void DisableSaving()
    {
        isSaving = false;
        childrenMovementController[FormsController.Instance.GetCurrentForm()].RegisterPlayerMovementsInputs();
    }
    #endregion

    #region Animator methods
    public void Idle()
    {
        childrenAnimator[FormsController.Instance.GetCurrentForm()].SetTrigger("Idle");
    }

    #region Cutscenes methods
    public void Sleep()
    {
        childrenAnimator[(int)FormsController.TransformationType.Human].SetTrigger("Sleep");
    }

    public void StandUp()
    {
        childrenAnimator[(int)FormsController.TransformationType.Human].SetTrigger("StandUp");
    }

    public void LookAround()
    {
        childrenAnimator[(int)FormsController.TransformationType.Human].SetTrigger("LookAround");
    }

    public void Mad()
    {
        childrenAnimator[(int)FormsController.TransformationType.Human].SetTrigger("Mad");
    }

    public void Focus()
    {
        childrenAnimator[(int)FormsController.TransformationType.Human].SetTrigger("Focus");
    }
    #endregion
    #endregion

    #region MovementsController methods
    public void EnableMovementController(FormsController.TransformationType type)
    {
        childrenMovementController[(int)type].enabled = true;
    }

    public void DisableMovementController(FormsController.TransformationType type)
    {
        childrenMovementController[(int)type].enabled = false;
    }
    #endregion

}
