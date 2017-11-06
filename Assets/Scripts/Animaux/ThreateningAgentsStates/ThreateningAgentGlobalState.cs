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

        if (properties.isAlert) {
            // Get the ref of the play
            GameObject player = GameObject.FindWithTag("player");

            // check if the player does have a wierd behavior
            if(player.GetComponent<Rigidbody>().velocity.magnitude > 5.0f) {
                o.GetComponent<StateMachine>().ChangeState(EvadeState.Instance);
            }
        }
        if (properties.playerTooClose) {
            o.GetComponent<StateMachine>().ChangeState(EvadeState.Instance);
        }
    }

    override public void Exit(GameObject o) { /* Empty */ }

}