using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : State<GameObject> {
    private static EvadeState instance;

    private EvadeState() { }

    public static EvadeState Instance {
        get {
            if (instance == null) {
                instance = new EvadeState();
            }
            return instance;
        }
    }

    override public void Enter(GameObject o) {
        StateMachine FSM = o.GetComponent<StateMachine>();
        AgentProperties properties = o.GetComponent<AgentProperties>();

        FSM.animator.Play("Locomotion");
        // Set the animation variables
        FSM.animator.SetFloat("Speed_f", 1f);
        FSM.behavior.target_p = GameObject.FindWithTag("Player").transform.position;
        FSM.behavior.obstacleAvoidanceOn = true;
        FSM.behavior.fleeOn = true;

        // Change the maxSpeed of the animal
        //  properties.maxSpeed = 

    }

    override public void Execute(GameObject o) {
        StateMachine FSM = o.GetComponent<StateMachine>();
        AgentProperties properties = o.GetComponent<AgentProperties>();
        if (!properties.isAlert) {
            FSM.RevertToPreviousState();
        }

    }

    override public void Exit(GameObject o) {
        StateMachine FSM = o.GetComponent<StateMachine>();
        FSM.behavior.fleeOn = false;
        FSM.behavior.obstacleAvoidanceOn = false;
    }
}
