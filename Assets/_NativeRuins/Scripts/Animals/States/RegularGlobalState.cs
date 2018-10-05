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

    override public void Execute(GameObject o) {
        AgentProperties properties = o.GetComponent<AgentProperties>();
        GameObject player = GameObject.FindWithTag("Player");
        StateMachine FSM = o.GetComponent<StateMachine>();

        if (properties.isDead) {
            if (FSM.getCurrentState() != DeathState.Instance) {
                FSM.ChangeState(DeathState.Instance);
            }
        } else {
            if (FSM.getCurrentState() != PursuitState.Instance && FSM.getCurrentState() != EvadeState.Instance) {
                // check if the player is too close or that he has a weird behavior
                if (properties.playerTooClose || (properties.isAlert &&
                player.GetComponent<MovementController>().getCurrentSpeed() > 30.0f)) {
                    if (properties.IsMean) {
                        FSM.ChangeGlobalState(ThreatenedGlobalState.Instance);
                        FSM.ChangeState(PursuitState.Instance);
                    }
                    else if (FSM.getCurrentState() != EvadeState.Instance)
                    {
                        FSM.ChangeState(EvadeState.Instance);
                    }
                }
            }
        }
    }

    override public void Exit(GameObject o) { /* Empty */ }

}