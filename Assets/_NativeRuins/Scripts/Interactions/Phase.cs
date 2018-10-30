using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Phase
{
    public const int NUM_ACTIONS_MAX = 10;

    [SerializeField] public Camera attachedCamera;
    [SerializeField] public int startDialogueIndex;
    [SerializeField] public int endDialogueIndex;
    [SerializeField] public float min = 0;
    [SerializeField] public float max = 10;

    [SerializeField] private Trigger[] _actions;
    public Trigger[] Actions { get { return _actions; } }

    [SerializeField] private List<int> _dialogueSentenceReferences = new List<int>();
    public List<int> DialogueSentenceReferences { get { return _dialogueSentenceReferences; } }

    [SerializeField] private bool canPlayerMove;

    public int maxNumberDialogue = 0;

    private Camera mainCamera;

    public void AwakePhase()
    {
        if (attachedCamera != null)
        {
            attachedCamera.enabled = false;
        }
    }

    public void Activate(Camera defaultCamera, Canvas canvas)
    {
        // Setup
        SetupCutScene(defaultCamera, canvas);

        // Call a cut-scene to start the switch
        PlayCutSceneAnimation(defaultCamera);
    }

    public void SetupCutScene(Camera defaultCamera, Canvas canvas)
    {
        if (!canPlayerMove)
        {
            // Get all component needed form the player
            // TODO
        }

        // Activate the camera if one is specified
        if (attachedCamera != null)
        {
            //canvas.worldCamera = attachedCamera;
            if(defaultCamera != null)
            {
                defaultCamera.enabled = false;
            }
            attachedCamera.enabled = true;
        }
        else
        {
            //canvas.worldCamera = defaultCamera;
        }
    }

    public void PlayCutSceneAnimation(Camera defaultCamera)
    {
        // Launch the animator here
        /*
        Animator animator;
        if ((attachedCamera != null && (animator = attachedCamera.GetComponent<Animator>()) != null) ||
            (defaultCamera != null && (animator = defaultCamera.GetComponent<Animator>()) != null))
        {
            animator.SetTrigger("StartCutscene");
        }*/
    }

    public void StopCutSceneEnd(Camera defaultCamera, Canvas canvas)
    {
        Interrupt();
        if (attachedCamera != null)
        {
            // Set back the camera default in the canvas
            canvas.worldCamera = defaultCamera;

            attachedCamera.enabled = false;
            defaultCamera.enabled = true;
        }
    }

    public void Interrupt()
    {
        // Stop the previous action if they were not completed
        foreach (Trigger action in _actions)
        {
            action.Interrupt();
        }
    }
}
