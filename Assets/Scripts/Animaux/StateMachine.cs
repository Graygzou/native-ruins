using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class StateMachine : MonoBehaviour {

    GameObject owner;                       // Reference to the agent that owns this instance
    public SteeringBehavior behavior;       // Reference to the behavior of the agent
    public Animator animator;
    //public Animation animation;
    //private AgentProperty properties;

    private AnimatorStateInfo animationState;
    private AnimatorClipInfo[] animationClips;
    public float timeIdle = 1.0f;
    public float time = 0.0f;

    public State<GameObject> currentState;
    public State<GameObject> previousState;
    public State<GameObject> globalState;

    void Awake() {
        // Pre-process
        behavior = GetComponent<SteeringBehavior>();
        animator = GetComponent<Animator>();

        owner = transform.root.gameObject;
        currentState = IdleState.Instance;
        previousState = null;
        globalState = ThreateningAgentGlobalState.Instance;
    }

    void Start() {
        //StateMachineAnimation state = anim.GetBehaviour<StateMachineAnimation>();
        //state.stateMachine = this;
    }

    //use these methods to initialize the FSM
    public void SetCurrentState(State<GameObject> s) {
        currentState = s;
    }
    public void SetGlobalState(State<GameObject> s) {
        globalState = s;
    }
    public void SetPreviousState(State<GameObject> s) {
        previousState = s;
    }

   //call this to update the FSM
   void Update()
   {
        //if a global state exists, call its execute method, else do nothing
        if(globalState != null) {
            globalState.Execute(owner);
        }

        //same for the current state
        if (currentState != null) {
            currentState.Execute(owner);
        }

        //same for the steering behavior
        if (behavior != null) {
            behavior.UpdateBehavior();
        }
    }

    //change to a new state
    public void ChangeState(State<GameObject> newState) {
        //keep a record of the previous state
        previousState = currentState;

        //call the exit method of the existing state
        currentState.Exit(owner);

        //change state to the new state
        currentState = newState;

        //call the entry method of the new state
        currentState.Enter(owner);
    }

    //change state back to the previous state
    public void RevertToPreviousState() {
        ChangeState(previousState);
    }

    // Accessors
    public State<GameObject> getCurrentState() {
        return currentState;
    }
    public State<GameObject> getGlobalState() {
        return globalState;
    }
    public State<GameObject> getPreviousState() {
        return previousState;
    }
}