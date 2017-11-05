using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State<GameObject>
{
    private static IdleState instance;
    private Animator anim;
    private AnimatorStateInfo animationState;
    private float timeIdle;

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
        anim = o.GetComponent<Animator>();

        // Set the animation variables
        anim.SetFloat("Speed_f", 0.0f);
        animationState = anim.GetCurrentAnimatorStateInfo(0);

        AnimatorClipInfo[] animationClips;
        // make sure we're in the good state animation
        do
        {
            animationState = anim.GetCurrentAnimatorStateInfo(0);
            animationClips = anim.GetCurrentAnimatorClipInfo(0);
        } while (!animationState.tagHash.Equals(Animator.StringToHash("Locomotion")) &&
                 !animationClips[0].clip.name.Equals("Deer_Idle"));

        // Set the number of the the state is play
        timeIdle = (int)Mathf.Round(Random.Range(1.0f, 2.0f));

    }

    override public void Execute(GameObject o)
    {
        if(animationState.normalizedTime >= timeIdle - 0.4) {
            // Launch a coroutine to accelerate POLISH
            // o.GetComponent<AgentProperty>().StartCoroutine("AccelerateWalk");

            // Change state
            o.GetComponent<StateMachine>().ChangeState(WalkingState.Instance);
        }
    }

    override public void Exit(GameObject o)
    {
        // Nothing        
    }
}
