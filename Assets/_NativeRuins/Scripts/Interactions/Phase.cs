using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Phase
{
    public const int NUM_ACTIONS_MAX = 10;

    [SerializeField]
    private Camera attachedCamera;

    [SerializeField]
    private int startDialogueIndex;

    [SerializeField]
    private int endDialogueIndex;

    [SerializeField]
    public float min = 0;
    [SerializeField]
    public float max = 10;

    [MinMaxSlider (0, 100)]
    public Vector2 myVector;

    [SerializeField]
    private Trigger[] actions;

    [SerializeField]
    private bool canPlayerMove;

    public void Awake()
    {
        if (attachedCamera != null)
        {
            attachedCamera.enabled = false;
        }
    }

    public void Interrupt()
    {
        StopCutSceneEnd();

        // Fire the finish event
    }

    public void SetupCutScene()
    {
        if (!canPlayerMove)
        {
            // Get all component needed form the player

        }

        // Activate the camera
        attachedCamera.enabled = true;
    }

    private void PlayCutSceneAnimation()
    {
        // Launch the dialogues
        //Debug.Log(_dialogue);
        //FindObjectOfType<DialogueManager>().StartDialogue(_dialogue, null);
    }

    private void StopCutSceneEnd()
    {

    }

    public void TriggerActions()
    {
        foreach(Trigger action in actions)
        {
            action.Fire();
        }
    }
}
