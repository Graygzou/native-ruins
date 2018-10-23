using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase : IPhase {

    public const int NUM_ACTIONS_MAX = 10;

    private Camera attachedCamera;

    private Dialogue _dialogue;
    public Dialogue Dialogue { get { return _dialogue; } }

    private Trigger[] actions;

    private bool canPlayerMove;

    public Phase() : this(new Dialogue(), new Trigger[NUM_ACTIONS_MAX])
    { }

    public Phase(Dialogue presetDialogue) : this(presetDialogue, new Trigger[NUM_ACTIONS_MAX])
    { }

    public Phase(Dialogue presetDialogue, Trigger[] presetActions)
    {
        _dialogue = presetDialogue;
        actions = presetActions;
    }

    void IPhase.Interrupt()
    {

        // Fire the finish event
    }

    public void SetupCutScene()
    {
        // nothing yet
    }

    public void TriggerActions()
    {
        // nothing yet
    }
}
