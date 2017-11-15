using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<GameObject>
{
    private static AttackState instance;
    private float timer;

    private AttackState() { }

    public static AttackState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AttackState();
            }
            return instance;
        }
    }

    override public void Enter(GameObject o) {
        timer = 0.0f;
        // change the music ?
        // Jouer l'animation (fumé qui sort de la tete / narines, par ex)
    }

    override public void Execute(GameObject o) {
        StateMachine FSM = o.GetComponent<StateMachine>();
        AgentProperties properties = o.GetComponent<AgentProperties>();
        GameObject player = GameObject.FindWithTag("Player");
        // Tant que vivant && vie > 50% && joueur proche

        if (properties.getCurrentHealth() <= 0) {
            FSM.ChangeState(DeathState.Instance);
        }
        if ((properties.getCurrentHealth() * 100) / properties.maxHealth < 50) {
            FSM.ChangeState(EvadeState.Instance);
        }
        if (!properties.isAlert) {
            FSM.ChangeState(WalkingState.Instance);
            FSM.ChangeGlobalState(ThreateningAgentGlobalState.Instance);
        }
    }

    override public void Exit(GameObject o) {
        // Change music ?
    }

}
