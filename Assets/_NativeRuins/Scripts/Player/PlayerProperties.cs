using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
    // Actual State of the player
    [Header("Player States (read only)")]
    [SerializeField] private bool isDead;
    [SerializeField] private bool isSaving;
    [SerializeField] private bool dialogueOn;

    [Header("Childrens Information")]
    [SerializeField] private MovementController[] childrenMovementController;
    [SerializeField] private Animator[] childrenAnimator;

    private void Awake()
    {
        isDead = false;
        dialogueOn = false;
        isSaving = false;
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
        dialogueOn = true;
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
        Debug.Log(FormsController.Instance.GetCurrentForm());
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
