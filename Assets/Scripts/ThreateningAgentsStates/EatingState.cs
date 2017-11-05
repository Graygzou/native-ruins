using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingState : State<GameObject>
{
    private static EatingState instance;

    private Animator anim;
    public Animation animation;
    private AgentProperty properties;

    private AnimatorStateInfo animationState;
    private AnimatorClipInfo[] animationClips;
    private float timeIdle;
    private float time;

    private EatingState() { }

    public static EatingState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EatingState();
            }
            return instance;
        }
    }

    override public void Enter(GameObject o)
    {
        anim = o.GetComponent<Animator>();
        properties = o.GetComponent<AgentProperty>();

        // Stop the previous animation
        anim.Play("Eat");

        // Set the number of time that the state will be play
        timeIdle = (int)Mathf.Round(Random.Range(2.0f, 4.0f));
    }

    override public void Execute(GameObject o)
    {

        // Update of the variable if needed
        if (properties.hungryIndicator > 0.0f) {
            properties.hungryIndicator -= 1f;
        }

        float currTime = o.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (currTime >= timeIdle - 0.06) {
            // Launch a coroutine to accelerate POLISH
            // o.GetComponent<AgentProperty>().StartCoroutine("AccelerateWalk");
            // ---anim.SetBool("IsHungry", false);
            // Change state
            o.GetComponent<StateMachine>().ChangeState(WalkingState.Instance);
            // Make the transition for the animation
            anim.SetFloat("Speed_f", 0.5f);
        }

    }

    override public void Exit(GameObject o) {
        anim.SetBool("IsHungry", false);
    }
}
