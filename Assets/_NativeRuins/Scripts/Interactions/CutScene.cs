using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    protected List<MonoBehaviourPhase> cutscenePhases;

    [SerializeField]
    private bool canPlayerMove = false;
    [SerializeField]
    private bool useHardCodedPhases = true;
    #endregion

    public delegate void CutsceneEnd(CutsceneName name);
    public static event CutsceneEnd OnCutsceneEnd;

    protected Camera playerCamera;

    protected virtual void Awake()
    {
        // Get the camera of the player
        playerCamera = Camera.main;

        Debug.Log(cutscenePhases.Count);
        /* TODO
        MonoBehaviourPhase t = new MonoBehaviourPhase(new Dialogue()
        {
            name = "Judy",
            sentences = new string[] {
                    ".........           ",
                    "  ... aaah..  aaah.... ma tête...",
                }
        });
        cutscenePhases.Add(t);*/
        Debug.Log(cutscenePhases.Count);
    }

    protected virtual void Start()
    {
        
    }

    public virtual void Activate() {
        Debug.Log(cutscenePhases.Count);
        for(int i = 0; i < cutscenePhases.Count; i++)
        {
            IPhase phase = cutscenePhases[i];
            Debug.Log(phase);
            if (phase != null)
            {
                Debug.Log("Info: start Phase " + phase);
                Activate(phase);
            }
        }
    }


    #region IPhase workflow
    private void Activate(IPhase currentPhase)
    {
        // Setup
        currentPhase.SetupCutScene();

        // Call a cut-scene to start the switch
        PlayCutSceneAnimation(currentPhase);

        // Simply activate the desired switch
        currentPhase.TriggerActions();
    }

    private void PlayCutSceneAnimation(IPhase currentPhase)
    {
        Dialogue currentDialogue = null;
        // Retrieve the dialogue
        if(currentPhase is Phase)
        {
            currentDialogue = (currentPhase as Phase).Dialogue;
        }
        else if(currentPhase is MonoBehaviourPhase)
        {
            currentDialogue = (currentPhase as MonoBehaviourPhase).Dialogue;
        }
        
        if (currentPhase != null)
        {
            Debug.Log(currentDialogue);
            // Launch the dialogue
            FindObjectOfType<DialogueManager>().StartDialogue(currentDialogue, null);
        }
    }
    #endregion

    public void Interrupt()
    {
        // Escape all the cutscenePhases
        // todo
    }
}
