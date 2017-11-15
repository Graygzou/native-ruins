using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class RegularGlobalState : State<GameObject>
{
    private static RegularGlobalState instance;

    private RegularGlobalState() { }

    public static RegularGlobalState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new RegularGlobalState();
            }
            return instance;
        }
    }

    override public void Enter(GameObject o) { /* Empty */ }

    override public void Execute(GameObject o)
    {
        AgentProperties properties = o.GetComponent<AgentProperties>();
        GameObject player = GameObject.FindWithTag("Player");

        if (properties.isDead) {
            o.GetComponent<StateMachine>().ChangeState(DeathState.Instance);
        }

        // check if the player is too close or that he has a weird behavior
        if (properties.playerTooClose || (properties.isAlert &&
        player.GetComponent<Rigidbody>().velocity.magnitude > 5.0f))
        {
            if (properties.isMean)
            {
                o.GetComponent<StateMachine>().ChangeGlobalState(ThreatenedGlobalState.Instance);
                o.GetComponent<StateMachine>().ChangeState(PursuitState.Instance);
            }
            else if (!properties.isMean && o.GetComponent<StateMachine>().getCurrentState() != EvadeState.Instance)
            {
                o.GetComponent<StateMachine>().ChangeState(EvadeState.Instance);
            }
        }
    }

    override public void Exit(GameObject o) { /* Empty */ }

}