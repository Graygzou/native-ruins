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

    #region Cutscene methods
    public void Sleep()
    {
        childrenAnimator[(int)FormsController.TransformationType.Human].SetTrigger("Sleep");
    }
    #endregion

}
