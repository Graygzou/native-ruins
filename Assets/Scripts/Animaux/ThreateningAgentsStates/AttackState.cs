using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<GameObject>
{
    private static AttackState instance;

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
        // change the music ?
        // Jouer l'animation (fumé qui sort de la tete / narines, par ex)
    }

    override public void Execute(GameObject o) {
        StateMachine FSM = o.GetComponent<StateMachine>();
        AgentProperties properties = o.GetComponent<AgentProperties>();
        GameObject player = GameObject.FindWithTag("Player");
        // Tant que vivant && vie > 50% && joueur proche

        FSM.behavior.seekOn = false;

        // If the player is far away
        if ((player.transform.position - o.transform.position).magnitude > 30.0f) {
            // Run in the direction of the player
            FSM.behavior.target_p = GameObject.FindWithTag("Player").transform.position;
            FSM.behavior.seekOn = true;
        } else {
            o.GetComponent<StateMachine>().ChangeState(ChargeState.Instance);
        }

        //AttackPlayer(FSM);

        if (properties.getCurrentHealth() <= 0) {
            o.GetComponent<StateMachine>().ChangeState(DeathState.Instance);
        }
        if ((properties.getCurrentHealth() * 100) / properties.maxHealth < 50) {
            o.GetComponent<StateMachine>().ChangeState(EvadeState.Instance);
        }
        if (!properties.isAlert) {
            o.GetComponent<StateMachine>().ChangeState(EatingState.Instance);
        }
    }

    //IEnumerator AttackPlayer(StateMachine FSM) {
        
    //}

    override public void Exit(GameObject o) {
        // Change music ?
    }

}
