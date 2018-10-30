using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used to create, managed and end an in-game cutscene.
/// </summary>
[ExecuteInEditMode]
public class CutScene : MonoBehaviour
{
    #region Enums
    public enum InGameCutsceneName : int
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
    private InGameCutsceneName _cutsceneName;
    public InGameCutsceneName CutsceneName { get { return _cutsceneName; } }

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

    public delegate void CutsceneEnd(InGameCutsceneName name);
    public static event CutsceneEnd OnCutsceneEnd;

    private List<Trigger>[] dialogueSentenceTriggers;

    private List<Phase> activePhases;
    private Camera mainCamera;

    private bool isTyping = false;
    private bool actionsFinished = false;

    private int actualSentenceIndex;
    private int nbTriggers;
    private int triggerDone;

    // For the editor..
    /*
    public void Awake()
    {
        foreach(Phase phase in cutscenePhases)
        {
            phase.maxNumberDialogue = _dialogue.dialogue.Count;
            phase.AwakePhase();
        }
    }*/

    public virtual void Init()
    {
        Debug.Log("Info: Cutscene init");

        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

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
                        //Debug.Log("Trigger found for the dialogue " + dialogueIndex + 1 + ", trigger :" + phase.Actions[actionIndex]);
                        // This current action want to belong to the current dialogue
                        dialogueSentenceTriggers[dialogueIndex].Add(phase.Actions[actionIndex]);
                    }
                }
            }
        }
        activePhases = new List<Phase>();
        actualSentenceIndex = -1;
        actionsFinished = false;
        triggerDone = 0;

        // Custom setup for this cutscene.
        SetupPlayerState();
    }

    private void SetupPlayerState()
    {
        PlayerProperties playerProperties = GameObject.FindWithTag("Player").GetComponent<PlayerProperties>();
        playerProperties.LaunchDialogue();

        // Should be in a specified cutscene
        // TODO later....
        playerProperties.Sleep();
    }

    public virtual void Activate()
    {
        // Disable the main camera
        if (mainCamera != null)
        {
            mainCamera.enabled = false;
        }
        // Enable the default camera for the cutscene
        defaultCamera.enabled = true;

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
                            activePhase.StopCutSceneEnd(defaultCamera, overlayCanvas);
                            activePhases.Remove(activePhase);
                        }
                    }

                    // Active some more if needed
                    if (phase.startDialogueIndex <= (actualSentenceIndex + 1) && phase.endDialogueIndex >= (actualSentenceIndex + 1) &&
                        !activePhases.Contains(phase))
                    {
                        // Start the phase
                        activePhases.Add(phase);

                        phase.Activate(defaultCamera, overlayCanvas);
                    }
                }
            }

            // Launch the current dialogue's sentence with associated triggers
            if (dialogueSentenceTriggers.Length > actualSentenceIndex && dialogueSentenceTriggers[actualSentenceIndex] != null &&
                dialogueSentenceTriggers[actualSentenceIndex].Count > 0)
            {
                // Reset tmp values
                actionsFinished = false;
                triggerDone = 0;
                nbTriggers = dialogueSentenceTriggers[actualSentenceIndex].Count;
                foreach (Trigger trigger in dialogueSentenceTriggers[actualSentenceIndex])
                {
                    if (trigger != null)
                    {
                        Debug.Log("Info: Start trigger :" + trigger);
                        Trigger.OnTriggerFinish += TriggerEndCallback;
                        SwitchManager.ExecuteAction(trigger);
                    }
                }
            }
            FindObjectOfType<DialogueManager>().DisplayNextSentence2(_dialogue.dialogue[actualSentenceIndex]);
        }
        else
        {
            if(actionsFinished)
            {
                EndCutscene();
            }
            else
            {
                FindObjectOfType<DialogueManager>().EndDialogue();
            }
        }
    }

    public void TriggerEndCallback()
    {
        triggerDone++;
        actionsFinished = triggerDone >= nbTriggers;
        if (actionsFinished && actualSentenceIndex >= _dialogue.dialogue.Count)
        {
            EndCutscene();
        }
    }

    private void EndCutscene()
    {
        Finish();
        Disable();
        // Fire the end event
        OnCutsceneEnd(_cutsceneName);
    }

    private void Finish()
    {
        DialogueManager.OnClicked -= Display;

        FindObjectOfType<DialogueManager>().EndDialogue();

        GameObject.FindWithTag("Player").GetComponent<PlayerProperties>().CloseDialogue();
    }

    public void Disable()
    {
        if(mainCamera != null)
        {
            mainCamera.enabled = true;
        }
        defaultCamera.enabled = false;
    }

    public IEnumerator Interrupt()
    {
        Finish();

        // Fade the cutscene with a white screen
        whiteScreenAnimator.SetTrigger("QuickFade");

        yield return new WaitForSeconds(1.0f);

        // Escape all the current active phases
        for (int phaseIndex = 0; phaseIndex < activePhases.Count; phaseIndex++)
        {
            Phase activePhase = activePhases[phaseIndex];

            activePhase.Interrupt();
            activePhases.Remove(activePhase);
        }

        Disable();

        // Fire the end event
        OnCutsceneEnd(_cutsceneName);
    }
}
