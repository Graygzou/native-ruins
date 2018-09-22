using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State<GameObject>
{
    private static IdleState instance;
    // Do not add more variables here

    private IdleState() { }

    public static IdleState Instance {
        get {
            if (instance == null) {
                instance = new IdleState();
            }
            return instance;
        }
    }

    override public void Enter(GameObject o)
    {
        // Get the current agent variables
        StateMachine FSM = o.GetComponent<StateMachine>();

        // Set the animation variables
        FSM.animator.SetFloat("Speed_f", 0.0f);
        AnimatorStateInfo animationState = FSM.animator.GetCurrentAnimatorStateInfo(0);

        // make sure we're in the good state animation
        //do
        //{
        //    animationState = anim.GetCurrentAnimatorStateInfo(0);
        //    animationClips = anim.GetCurrentAnimatorClipInfo(0);
        //} while (animationClips == null ||  !animationState.tagHash.Equals(Animator.StringToHash("Locomotion")) ||
        //         !animationClips[0].clip.name.Equals("Deer_Idle"));

        // Set the number of the the state is play
        FSM.timeIdle = (int)Mathf.Round(Random.Range(1.0f, 2.0f));
        FSM.time = animationState.normalizedTime;
    }

    override public void Execute(GameObject o)
    {
        // Get the current agent variables
        StateMachine FSM = o.GetComponent<StateMachine>();

        float currTime = o.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (currTime >= FSM.time + FSM.timeIdle - 0.06) {
            // Launch a coroutine to accelerate POLISH
            // o.GetComponent<AgentProperty>().StartCoroutine("AccelerateWalk");

            // Change state
            FSM.ChangeState(WalkingState.Instance);
        }
    }

    override public void Exit(GameObject o)
    {
        // Nothing        
    }
}
