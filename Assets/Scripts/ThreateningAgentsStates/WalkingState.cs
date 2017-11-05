using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : State<GameObject> {

    private static WalkingState instance;
    private Animator anim;
    private SteeringBehavior behavior;
    private AgentProperty properties;

    private AnimatorStateInfo animationState;
    private AnimatorClipInfo[] animationClips;
    private float timeIdle;
    private float time;

    private WalkingState() { }

    public static WalkingState Instance {
        get {
            if (instance == null) {
                instance = new WalkingState();
            }
            return instance;
        }
    }

    override public void Enter(GameObject o) {
        anim = o.GetComponent<Animator>();
        behavior = o.GetComponent<SteeringBehavior>();
        properties = o.GetComponent<AgentProperty>();

        //o.GetComponent<StateMachine>().StartCoroutine("WaitGoodAnimation");
        anim.Play("Locomotion");

        // Set the animation variables
        anim.SetFloat("Speed_f", 0.5f);
        behavior.wanderOn = true;
        behavior.obstacleAvoidanceOn = true;

        // make sure we're in the good state animation
        animationState = anim.GetCurrentAnimatorStateInfo(0);
        animationClips = anim.GetCurrentAnimatorClipInfo(0);
        // Force the state animation

        //do
        //{
        //    Debug.Log("ERROR ICI");
        //    animationState = anim.GetCurrentAnimatorStateInfo(0);
        //    animationClips = anim.GetCurrentAnimatorClipInfo(0);
        //} while (!animationState.tagHash.Equals(Animator.StringToHash("Locomotion")) ||
        //    !animationClips[0].clip.name.Equals("Deer_Walk"));

        //anim.Play("Deer_Walk");

        // TODO: voir ce qui peut etre fait au niveau de la classe State mere. (refactoring)
        // Set the number of time that the state will be play
        timeIdle = (int)Mathf.Round(Random.Range(4.0f, 6.0f));
        time = animationState.normalizedTime;
    }

    override public void Execute(GameObject o) {
        // Udpate variables
        properties.hungryIndicator += 0.1f;

        // Check for transitions
        if (properties.hungryIndicator >= 50.0f) {
            // Launch a coroutine to accelerate POLISH
            // o.GetComponent<AgentProperty>().StartCoroutine("DecelerateWalk");
            anim.SetBool("IsHungry", true);
            anim.SetFloat("Speed_f", 0.0f);
            o.GetComponent<StateMachine>().ChangeState(EatingState.Instance);
        }
        else {
            float currTime = o.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (currTime >= time + timeIdle - 0.06) {
                o.GetComponent<StateMachine>().ChangeState(IdleState.Instance);
            }
        }
    }

    override public void Exit(GameObject o) {
        behavior.wanderOn = false;
        behavior.obstacleAvoidanceOn = false;
    }

}
