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

        FSM.animator.Play("Locomotion");
        // Set the animation variables
        FSM.animator.SetFloat("Speed_f", 1f);
        FSM.behavior.target_p = GameObject.FindWithTag("player").transform;
        FSM.behavior.obstacleAvoidanceOn = true;
        FSM.behavior.fleeOn = true;

        // TODO : Change the speed of the animal
    }

    override public void Execute(GameObject o) {
        StateMachine FSM = o.GetComponent<StateMachine>();
        AgentProperty properties = o.GetComponent<AgentProperty>();
        if (!properties.isAlert) {
            FSM.RevertToPreviousState();
        }

    }

    override public void Exit(GameObject o) {
        StateMachine FSM = o.GetComponent<StateMachine>();
        FSM.behavior.obstacleAvoidanceOn = false;
        FSM.behavior.fleeOn = false;
    }
}
