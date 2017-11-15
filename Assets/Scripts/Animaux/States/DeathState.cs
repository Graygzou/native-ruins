using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State<GameObject>
{
    private static DeathState instance;

    private DeathState() { }

    public static DeathState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DeathState();
            }
            return instance;
        }
    }

    override public void Enter(GameObject o)
    {
        StateMachine FSM = o.GetComponent<StateMachine>();
        // Turn the collider into a trigger so shots can pass through it.
        if (o.GetComponent<MeshCollider>()) {
            o.GetComponent<MeshCollider>().isTrigger = true;
        } else {
            o.GetComponentInChildren<MeshCollider>().isTrigger = true;
        }

        // Tell the animator that the enemy is dead.
        FSM.animator.SetTrigger("Dead");
        FSM.animator.Play("Death");

        // Change the audio clip of the audio source to the death clip and play it (this will stop the hurt clip playing).
        //enemyAudio.clip = deathClip;
        //enemyAudio.Play();
    }

    override public void Execute(GameObject o)
    {
        // Get the current agent variables
        StateMachine FSM = o.GetComponent<StateMachine>();

        float currTime = o.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (currTime >= 0.94)
        {
            // Stop the Finite State Machine
            FSM.DestroyFSM();
        }
    }

    override public void Exit(GameObject o)
    {
        // Find and disable the Nav Mesh Agent.
        o.GetComponent<SteeringBehavior>().enabled = false;

        // Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
        o.GetComponent<Rigidbody>().isKinematic = true;

        // The enemy should no sink.
        //isSinking = true;

        // DROP Items
        // Increase the score by the enemy's score value.
        //ScoreManager.score += scoreValue;

        // After 2 seconds destory the enemy.
        GameObject.Destroy(o, 2f);

        // Generer de la poussiere
        // Generer l'item avec une animation "bas-haut"
    }
}
