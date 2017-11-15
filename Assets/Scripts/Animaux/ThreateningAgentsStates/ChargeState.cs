using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeState : State<GameObject>
{
    private static ChargeState instance;
    private Vector3 target_charge;

    private ChargeState() { }

    public static ChargeState Instance {
        get {
            if (instance == null) {
                instance = new ChargeState();
            }
            return instance;
        }
    }

    override public void Enter(GameObject o) {
        GameObject player = GameObject.FindWithTag("Player");
        StateMachine FSM = o.GetComponent<StateMachine>();

        // Rush into the player
        FSM.animator.SetFloat("Speed_f", 2f);
        FSM.animator.Play("Locomotion");
        target_charge = GameObject.FindWithTag("Player").transform.position;
        FSM.behavior.target_p = target_charge;
        FSM.behavior.seekOn = true;
    }

    override public void Execute(GameObject o) {
        StateMachine FSM = o.GetComponent<StateMachine>();
        AgentProperties properties = o.GetComponent<AgentProperties>();
        GameObject player = GameObject.FindWithTag("Player");

        if ((o.transform.position - target_charge).magnitude < 1.0f) {
            // do something ?
            o.GetComponent<StateMachine>().ChangeState(IdleState.Instance);
        }
    }

    override public void Exit(GameObject o) {
        StateMachine FSM = o.GetComponent<StateMachine>();
        FSM.animator.SetFloat("Speed_f", 0f);
        FSM.behavior.seekOn = false;
    }

}
