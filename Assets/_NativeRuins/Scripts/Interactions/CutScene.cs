using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public abstract class CutScene : MonoBehaviour
{
    #region Enums
    public enum CutsceneName : int
    {
        IntroductionCutscene = 0,
        BearTotemCutscene = 1,
        WolfTotemCutscene = 2,
        RopeCutscene = 3,
        SailCutscene = 4,
    }
    #endregion

    #region Serialize Fields
    [SerializeField]
    private Dialogue _dialogue;
    public Dialogue Dialogue { get { return _dialogue; } }

    [SerializeField]
    protected List<Phase> cutscenePhases;
    #endregion

    public delegate void CutsceneEnd(CutsceneName name);
    public static event CutsceneEnd OnCutsceneEnd;

    protected Camera playerCamera;

    private List<Trigger>[] dialogueSentenceTriggers;

    protected virtual void Awake()
    {
        foreach(Phase phase in cutscenePhases)
        {
            phase.maxNumberDialogue = _dialogue.dialogue.Count;
        }
        // Structure that will hold all the trigger for one dialogue sentence
        dialogueSentenceTriggers = new List<Trigger>[_dialogue.dialogue.Count];

        // Get the camera of the player
        playerCamera = Camera.main;
    }

    public virtual void Init()
    {
        for (int i = 0; i < _dialogue.dialogue.Count; i++)
        {
            // Tidy up Phase to accelerate the activation process.
            foreach (Phase phase in cutscenePhases)
            {
                if (phase.min >= i && phase.max < i)
                {
                    // The phase want to belong to that dialogue
                    dialogueSentenceTriggers[i].AddRange(phase.Actions);
                }
            }
        }
    }

    public virtual void Activate() {
        for (int i = 0; i < _dialogue.dialogue.Count; i++)
        {
            // Retrieve all the Trigger events that needs to occur for the next dialogue sentence.

            
            // Launch the dialogue
            FindObjectOfType<DialogueManager>().StartDialogue(_dialogue, null);
        }




            Debug.Log(cutscenePhases.Count);
        for(int i = 0; i < cutscenePhases.Count; i++)
        {
            Phase phase = cutscenePhases[i];
            Debug.Log(phase);
            if (phase != null)
            {
                Debug.Log("Info: start Phase " + phase);
                Activate(phase);
            }
        }
    }

    #region Phase workflow
    private void Activate(Phase currentPhase)
    {
        // Setup
        currentPhase.SetupCutScene();

        // Call a cut-scene to start the switch
        PlayCutSceneAnimation();

        // Simply activate the desired switch
        currentPhase.TriggerActions();
    }

    private void PlayCutSceneAnimation()
    {
        if (_dialogue != null)
        {
            // Launch the dialogue
            FindObjectOfType<DialogueManager>().StartDialogue(_dialogue, null);
        }
    }
    #endregion

    public void Interrupt()
    {
        // Escape all the cutscenePhases
        // todo
    }
}
