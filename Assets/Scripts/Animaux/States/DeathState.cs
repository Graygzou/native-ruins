using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State<GameObject>
{
    private static DeathState instance;

    private DeathState() { }

    public static DeathState Instance {
        get {
            if (instance == null) {
                instance = new DeathState();
            }
            return instance;
        }
    }

    override public void Enter(GameObject o) {
        StateMachine FSM = o.GetComponent<StateMachine>();

        // Tell the animator that the enemy is dead.
        FSM.animator.SetBool("Dead", true);
        FSM.animator.SetFloat("Speed_f", 0.0f);
        FSM.animator.Play("Death");

        // Change the audio clip of the audio source to the death clip and play it (this will stop the hurt clip playing).
        //enemyAudio.clip = deathClip;
        //enemyAudio.Play();
    }

    override public void Execute(GameObject o)
    {
        StateMachine FSM = o.GetComponent<StateMachine>();

        AnimatorClipInfo[] t = o.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
        float currTime = o.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (currTime >= 1.0 - 0.06)
        {
            Exit(o);

            // Stop the Finite State Machine
            FSM.DestroyFSM();
        }
    }

    override public void Exit(GameObject o)
    {
        AgentProperties properties = o.GetComponent<AgentProperties>();
        StateMachine FSM = o.GetComponent<StateMachine>();

        // Turn the collider into a trigger so shots can pass through it.
        if (o.GetComponent<MeshCollider>()) {
            o.GetComponent<MeshCollider>().isTrigger = true;
        } else {
            o.GetComponentInChildren<MeshCollider>().isTrigger = true;
        }

        // disable his steering behavior
        FSM.behavior = null;

        // Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
        o.GetComponent<Rigidbody>().isKinematic = true;

        properties.MakeAgentDisappear(o);

        // DROP Items
        // Increase the score by the enemy's score value.
        //ScoreManager.score += scoreValue;

        // Generer de la poussiere
        // Generer l'item avec une animation "bas-haut"
    }
}
