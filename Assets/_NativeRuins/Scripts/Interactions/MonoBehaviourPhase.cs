using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourPhase : MonoBehaviour, IPhase
{
    public const int NUM_ACTIONS_MAX = 10;

    [SerializeField]
    private Camera attachedCamera;

    [SerializeField]
    private Dialogue _dialogue;
    public Dialogue Dialogue { get { return _dialogue; } }

    [SerializeField]
    private Trigger[] actions;

    [SerializeField]
    private bool canPlayerMove;

    /*
    public void Awake()
    {
        if (attachedCamera != null)
        {
            attachedCamera.enabled = false;
        }
    }*/

    public MonoBehaviourPhase(Dialogue presetDialogue)
    {
        _dialogue = presetDialogue;
        actions = null;
    }

    void IPhase.Interrupt()
    {
        StopCutSceneEnd();

        // Fire the finish event
    }

    void IPhase.SetupCutScene()
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
        Debug.Log(_dialogue);
        FindObjectOfType<DialogueManager>().StartDialogue(_dialogue, null);
    }

    private void StopCutSceneEnd()
    {

    }

    void IPhase.TriggerActions()
    {
        foreach(Trigger action in actions)
        {
            action.Fire();
        }
    }
}
