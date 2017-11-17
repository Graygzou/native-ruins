using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using System;

public class StateMachine : MonoBehaviour {

    GameObject owner;                       // Reference to the agent that owns this instance
    [NonSerialized]
    public SteeringBehavior behavior;       // Reference to the behavior of the agent
    [NonSerialized]
    public Animator animator;
    private State<GameObject> currentState;
    private State<GameObject> previousState;
    private State<GameObject> globalState;

    [NonSerialized]
    public float timeIdle = 1.0f;
    [NonSerialized]
    public float time = 0.0f;

    void Awake() {
        // Pre-process
        behavior = GetComponent<SteeringBehavior>();
        animator = GetComponent<Animator>();
        owner = transform.root.gameObject;

        currentState = IdleState.Instance;
        previousState = null;
        globalState = RegularGlobalState.Instance;
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
   void FixedUpdate()
   {
        //if a global state exists, call its execute method, else do nothing
        if (globalState != null) {
            globalState.Execute(owner);
        }

        //same for the current state
        if (currentState != null) {
            currentState.Execute(owner);
        }

        //same for the steering behavior
        if (behavior != null && behavior.enabled) {
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

    public void DestroyFSM() {
        currentState = null;
        globalState = null;
    }

    //change to a new global state
    public void ChangeGlobalState(State<GameObject> newState)
    {
        //call the exit method of the existing global state
        globalState.Exit(owner);

        //change state to the new global state
        globalState = newState;

        //call the entry method of the new global state
        globalState.Enter(owner);
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

//[CustomEditor(typeof(StateMachine))]
//public class EditorStateMachine : Editor
//{
//    override public void OnInspectorGUI()
//    {
//        var myScript = target as StateMachine;

//        //myScript.currentState = EditorGUILayout.ObjectField("CurrentState", myScript.currentState, typeof(State<GameObject>), true) as State<GameObject>;
//        //myScript.previousState = EditorGUILayout.ObjectField("PreviousState", myScript.previousState, typeof(State<GameObject>), true) as State<GameObject>;
//        //myScript.globalState = EditorGUILayout.ObjectField("GlobalState", myScript.globalState, typeof(State<GameObject>), true) as State<GameObject>;
//    }
//}