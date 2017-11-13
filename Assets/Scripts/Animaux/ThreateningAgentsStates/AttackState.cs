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
        AgentProperty properties = o.GetComponent<AgentProperty>();
        GameObject player = GameObject.FindWithTag("Player");
        // Tant que vivant && vie > 50% && joueur proche

        // Set the animation variables
        FSM.animator.Play("Locomotion");
        FSM.animator.SetFloat("Speed_f", 2f);

        // Run in the direction of the player
        FSM.behavior.target_p = GameObject.FindWithTag("Player").transform;
        FSM.behavior.seekOn = true;

        if (properties.health <= 0) {
            o.GetComponent<StateMachine>().ChangeState(DeathState.Instance);
        }
        if ((properties.health * 100) / properties.maxHealth < 50) {
            o.GetComponent<StateMachine>().ChangeState(EvadeState.Instance);
        }
        if (!properties.isAlert) {
            o.GetComponent<StateMachine>().ChangeState(EatingState.Instance);
        }
    }

    override public void Exit(GameObject o) {
        // Change music ?
    }

}
