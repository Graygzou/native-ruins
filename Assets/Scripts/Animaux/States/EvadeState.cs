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

        // Set the animation variables
        properties.setSpeed(properties.maxSpeed);
        FSM.animator.SetFloat("Speed_f", 2.0f);
        FSM.animator.Play("Locomotion");

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
            FSM.ChangeState(WalkingState.Instance);
        }

    }

    override public void Exit(GameObject o) {
        StateMachine FSM = o.GetComponent<StateMachine>();
        AgentProperties properties = o.GetComponent<AgentProperties>();

        properties.setSpeed(22f);
        FSM.animator.SetFloat("Speed_f", 0.0f);
        FSM.behavior.target_p = Vector3.zero;
        FSM.behavior.fleeOn = false;
        FSM.behavior.obstacleAvoidanceOn = false;
    }
}
