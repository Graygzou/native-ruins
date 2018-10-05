using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuitState : State<GameObject> {

    private static PursuitState instance;

    private PursuitState() { }

    public static PursuitState Instance {
        get {
            if (instance == null) {
                instance = new PursuitState();
            }
            return instance;
        }
    }

    override public void Enter(GameObject o) {
        StateMachine FSM = o.GetComponent<StateMachine>();
        GameObject player = GameObject.FindWithTag("Player");
        AgentProperties properties = o.GetComponent<AgentProperties>();

        properties.setSpeed(properties.MaxSpeed);
        FSM.animator.SetFloat("Speed_f", 2f);
        FSM.animator.Play("Locomotion");

        // Look and Run in the direction of the player
        Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - o.transform.position);
        o.transform.rotation = Quaternion.Slerp(o.transform.rotation, targetRotation, 5.0f * Time.deltaTime);
        FSM.behavior.target_p = player.transform.position;
        FSM.behavior.seekOn = true;
        FSM.behavior.obstacleAvoidanceOn = true;
    }

    override public void Execute(GameObject o) {
        StateMachine FSM = o.GetComponent<StateMachine>();
        AgentProperties properties = o.GetComponent<AgentProperties>();
        GameObject player = GameObject.FindWithTag("Player");

        // If the player is not far away
        if ((player.transform.position - o.transform.position).magnitude < properties.TauntRange) {
            FSM.ChangeState(TauntState.Instance);
            //FSM.animator.SetBool("ReadyToCharge", true);
        }
    }

    override public void Exit(GameObject o) {
        StateMachine FSM = o.GetComponent<StateMachine>();
        AgentProperties properties = o.GetComponent<AgentProperties>();

        properties.setSpeed(22);
        FSM.animator.SetFloat("Speed_f", 0.0f);
        FSM.behavior.seekOn = false;
        FSM.behavior.obstacleAvoidanceOn = false;
    }
}
