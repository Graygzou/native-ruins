using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;



public class ThreateningAgentGlobalState : State<GameObject>
{
    private static ThreateningAgentGlobalState instance;

    private ThreateningAgentGlobalState() { }

    public static ThreateningAgentGlobalState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ThreateningAgentGlobalState();
            }
            return instance;
        }
    }

    override public void Enter(GameObject o) { /* Empty */ }

    override public void Execute(GameObject o)
    {
        AgentProperty properties = o.GetComponent<AgentProperty>();
        GameObject player = GameObject.FindWithTag("Player");

        // check if the player is too close or that he has a wierd behavior
        if (properties.playerTooClose || (properties.isAlert && player.GetComponent<Rigidbody>().velocity.magnitude > 5.0f)) {
            if (properties.isMean) {
                o.GetComponent<StateMachine>().ChangeState(AttackState.Instance);
            } else {
                o.GetComponent<StateMachine>().ChangeState(EvadeState.Instance);
            }
        }
    }

    override public void Exit(GameObject o) { /* Empty */ }

}