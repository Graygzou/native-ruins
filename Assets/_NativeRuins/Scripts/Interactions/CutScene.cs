using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CutScene : MonoBehaviour
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
    private CutsceneName cutsceneName;

    [SerializeField]
    private Dialogue _dialogue;
    public Dialogue Dialogue { get { return _dialogue; } }

    [SerializeField]
    public Camera defaultCamera;

    [SerializeField]
    public Canvas overlayCanvas;

    [SerializeField]
    public Animator whiteScreenAnimator;

    [SerializeField]
    protected List<Phase> cutscenePhases;
    #endregion

    public delegate void CutsceneEnd(CutsceneName name);
    public static event CutsceneEnd OnCutsceneEnd;

    private List<Trigger>[] dialogueSentenceTriggers;

    private List<Phase> activePhases;
    private Camera mainCamera;

    /*
    private Camera playerCamera;*/

    private bool isTyping = false;

    private int actualSentenceIndex;

    // For the editor..
    public void Awake()
    {
        foreach(Phase phase in cutscenePhases)
        {
            phase.maxNumberDialogue = _dialogue.dialogue.Count;
            phase.Awake();
        }
    }

    public virtual void Init()
    {
        Debug.Log("Info: Cutscene init");

        // Disable the main camera
        mainCamera = Camera.main;
        mainCamera.enabled = false;
        // Enable the default camera for the cutscene
        defaultCamera.enabled = true;

        if (_dialogue != null && _dialogue.dialogue != null)
        {
            // Structure that will hold all the trigger for one dialogue sentence
            dialogueSentenceTriggers = new List<Trigger>[_dialogue.dialogue.Count];
        }

        for (int dialogueIndex = 0; dialogueIndex < _dialogue.dialogue.Count; dialogueIndex++)
        {
            dialogueSentenceTriggers[dialogueIndex] = new List<Trigger>();

            // Tidy up Phase to accelerate the activation process.
            foreach (Phase phase in cutscenePhases)
            {
                for (int actionIndex = 0; actionIndex < phase.Actions.Length; actionIndex++)
                {
                    if(phase.DialogueSentenceReferences[actionIndex].Equals(dialogueIndex+1))
                    {
                        Debug.Log("Trigger found for the dialogue " + dialogueIndex + 1 + ", trigger :" + phase.Actions[actionIndex]);
                        // This current action want to belong to the current dialogue
                        dialogueSentenceTriggers[dialogueIndex].Add(phase.Actions[actionIndex]);
                    }
                }
            }
        }
        activePhases = new List<Phase>();
        actualSentenceIndex = -1;

        // Custom setup for this cutscene.
        SetupPlayerState();
    }

    private void SetupPlayerState()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerProperties>().Sleep();
    }

    public virtual void Activate()
    {
        DialogueManager.OnClicked += Display;
        // Call only once!
        FindObjectOfType<DialogueManager>().InitDialogueUI();

        Display();
    }

    private void Display()
    {
        actualSentenceIndex++;

        if(actualSentenceIndex < _dialogue.dialogue.Count)
        {
            // Check if phase need to be launched or need to be stopped
            foreach (Phase phase in cutscenePhases)
            {
                if (phase != null)
                {
                    //Debug.Log("Proceed phase :" + phase.startDialogueIndex);
                    //Debug.Log("actualSentenceIndex :" + actualSentenceIndex);
                    // Phase actually in progress
                    for (int phaseIndex = 0; phaseIndex < activePhases.Count; phaseIndex++)
                    {
                        Phase activePhase = activePhases[phaseIndex];
                        if (activePhase.endDialogueIndex < (actualSentenceIndex + 1))
                        {
                            //Debug.Log("Info: Remove Phase" + activePhase.attachedCamera);
                            // Stop the phase
                            activePhase.StopCutSceneEnd(defaultCamera);
                            activePhases.Remove(activePhase);
                        }
                    }

                    // Active some more if needed
                    if (phase.startDialogueIndex <= (actualSentenceIndex + 1) && phase.endDialogueIndex >= (actualSentenceIndex + 1) &&
                        !activePhases.Contains(phase))
                    {
                        // Start the phase
                        activePhases.Add(phase);

                        Activate(phase);
                    }
                }
            }

            // Launch the current dialogue's sentence with associated triggers
            // TODO : Pass the entire List and not just the first element.... dialogueSentenceTriggers[i] et not dialogueSentenceTriggers[i][0]
            if (dialogueSentenceTriggers.Length > actualSentenceIndex && dialogueSentenceTriggers[actualSentenceIndex] != null &&
                dialogueSentenceTriggers[actualSentenceIndex].Count > 0)
            {
                foreach (Trigger trigger in dialogueSentenceTriggers[actualSentenceIndex])
                {
                    if (trigger != null)
                    {
                        Debug.Log("Info: Start trigger :" + trigger);
                        SwitchManager.ExecuteAction(trigger);
                    }
                }
            }
            FindObjectOfType<DialogueManager>().DisplayNextSentence2(_dialogue.dialogue[actualSentenceIndex]);
        }
        else
        {
            Finish();
            Disable();
        }
    }
    
    #region Phase workflow
    private void Activate(Phase currentPhase)
    {
        // Setup
        currentPhase.SetupCutScene(defaultCamera, overlayCanvas);

        // Call a cut-scene to start the switch
        currentPhase.PlayCutSceneAnimation(defaultCamera);

        // Simply activate the desired switch
        currentPhase.TriggerActions();
    }

    private void Finish()
    {
        DialogueManager.OnClicked -= Display;

        FindObjectOfType<DialogueManager>().EndDialogue();

        // Fire the end event
        OnCutsceneEnd(cutsceneName);
    }

    public void Disable()
    {
        mainCamera.enabled = true;
        defaultCamera.enabled = false;
    }

    /*
    private void PlayCutSceneAnimation()
    {
        if (_dialogue != null)
        {
            // Launch the dialogue
            FindObjectOfType<DialogueManager>().StartDialogue(_dialogue, null);
        }
    }*/
    #endregion

    public void Interrupt()
    {
        // Fade the cutscene with a white screen
        // Should trigger the Diable method in the stateBehavior..
        whiteScreenAnimator.SetTrigger("Fade");

        // Escape all the current active phases
        for (int phaseIndex = 0; phaseIndex < activePhases.Count; phaseIndex++)
        {
            Phase activePhase = activePhases[phaseIndex];

            activePhase.StopCutSceneEnd(defaultCamera);
            activePhases.Remove(activePhase);
        }

        Finish();
    }
}
