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

        FSM.animator.Play("Locomotion");
        FSM.animator.SetFloat("Speed_f", 2f);

        // Run in the direction of the player
        FSM.behavior.target_p = player.transform.position;
        FSM.behavior.seekOn = true;
        FSM.behavior.obstacleAvoidanceOn = true;
    }

    override public void Execute(GameObject o) {
        StateMachine FSM = o.GetComponent<StateMachine>();
        GameObject player = GameObject.FindWithTag("Player");

        // If the player is not far away
        if ((player.transform.position - o.transform.position).magnitude < 30.0f) {
            FSM.ChangeState(TauntState.Instance);
            //FSM.animator.SetBool("ReadyToCharge", true);
        }
    }

    override public void Exit(GameObject o) {
        StateMachine FSM = o.GetComponent<StateMachine>();
        FSM.animator.SetFloat("Speed_f", 0.0f);
        FSM.behavior.seekOn = false;
        FSM.behavior.obstacleAvoidanceOn = false;
    }
}
