using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Phase
{
    public const int NUM_ACTIONS_MAX = 10;

    [SerializeField]
    public Camera attachedCamera;

    [SerializeField]
    public int startDialogueIndex;

    [SerializeField]
    public int endDialogueIndex;

    [SerializeField]
    public float min = 0;
    [SerializeField]
    public float max = 10;

    [SerializeField]
    private Trigger[] _actions;
    public Trigger[] Actions { get { return _actions; } }

    [SerializeField]
    private List<int> _dialogueSentenceReferences = new List<int>();
    public List<int> DialogueSentenceReferences { get { return _dialogueSentenceReferences; } }

    [SerializeField]
    private bool canPlayerMove;

    public int maxNumberDialogue = 0;

    private Camera mainCamera;

    public void Awake()
    {
        if (attachedCamera != null)
        {
            attachedCamera.enabled = false;
        }
    }

    /*
    public void Interrupt()
    {
        StopCutSceneEnd();

        // Fire the finish event
    }*/

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
            attachedCamera.enabled = true;
            canvas.worldCamera = attachedCamera;
            if(defaultCamera != null)
            {
                defaultCamera.enabled = false;
            }
        }
        else
        {
            canvas.worldCamera = defaultCamera;
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

    public void StopCutSceneEnd(Camera defaultCamera)
    {
        foreach (Trigger action in _actions)
        {
            action.Interrupt();
        }

        if (attachedCamera != null)
        {
            attachedCamera.enabled = false;
            defaultCamera.enabled = true;
        }
        
    }

    public void TriggerActions()
    {
        /*
        foreach(Trigger action in actions)
        {
            action.Fire();
        }*/
    }
}
