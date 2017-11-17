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
        AgentProperties properties = o.GetComponent<AgentProperties>();
        GameObject player = GameObject.FindWithTag("Player");
        StateMachine FSM = o.GetComponent<StateMachine>();
        // Set the animation variables

        properties.setSpeed(properties.maxSpeed);
        FSM.animator.SetFloat("Speed_f", 2f);

        FSM.animator.Play("Locomotion");
        target_charge = GameObject.FindWithTag("Player").transform.position;

        // Rush into the player
        FSM.behavior.target_p = target_charge;
        FSM.behavior.seekOn = true;
    }

    override public void Execute(GameObject o) {
        StateMachine FSM = o.GetComponent<StateMachine>();
        AgentProperties properties = o.GetComponent<AgentProperties>();
        Transform nose = o.transform.GetChild(5).transform;
        GameObject player = GameObject.FindWithTag("Player");
        GameObject playerRoot = GameObject.Find("Player");

        RaycastHit hitInfo;
        if (Physics.Raycast(new Ray(nose.position, nose.forward), out hitInfo, 0.5f)) {
            // check if we can attack the player
            if (hitInfo.transform.tag == "Player") {
                FSM.ChangeState(AttackState.Instance);
            }
        } else if((FSM.behavior.target_p - o.transform.position).magnitude < 5.0f) {
            FSM.ChangeState(PursuitState.Instance);
        }
    }

    override public void Exit(GameObject o) {
        AgentProperties properties = o.GetComponent<AgentProperties>();
        StateMachine FSM = o.GetComponent<StateMachine>();

        properties.setSpeed(22f);
        FSM.animator.SetFloat("Speed_f", 0.0f);
        FSM.behavior.seekOn = false;
    }

}
