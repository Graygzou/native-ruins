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
    

    [Header("Others scripts")]
    [SerializeField] private MovementController movementController;

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

        GetComponent<FormsController>().Transformation(0);

        // UnSubscribe all events.
        movementController.DeRegisterInputs();
    }

    public void RevivePlayer()
    {
        isDead = false;

        // Subscribe events back.
        movementController.RegisterInputs();
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

        // UnSubscribe some events.
        movementController.DeregisterCameraMovementsInputs();
    }

    public void CloseDialogue()
    {
        dialogueOn = true;

        // Subscribe back events
        movementController.RegisterCameraMovementsInputs();
    }
    #endregion

    #region Saving
    public void EnableSaving()
    {
        isSaving = true;

        movementController.DeregisterPlayerMovementsInputs();
    }

    public void DisableSaving()
    {
        isSaving = false;

        movementController.DeregisterPlayerMovementsInputs();
    }
    #endregion


}
