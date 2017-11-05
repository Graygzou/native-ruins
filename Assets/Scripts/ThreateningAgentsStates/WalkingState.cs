using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : State<GameObject> {

    private static WalkingState instance;
    private Animator anim;
    private SteeringBehavior behavior;
    private AgentProperty properties;

    private AnimatorStateInfo animationState;
    private float timeIdle;

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

        // Set the animation variables
        anim.SetFloat("Speed_f", 0.5f);
        behavior.wanderOn = true;
        behavior.obstacleAvoidanceOn = true;

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

    override public void Execute(GameObject o) {
        // Udpate variables
        properties.hungryIndicator += 0.1f;

        // Check for transitions
        //if (properties.hungryIndicator >= 50.0f) {
        //    // Launch a coroutine to accelerate POLISH
        //    // o.GetComponent<AgentProperty>().StartCoroutine("DecelerateWalk");
        //    o.GetComponent<StateMachine>().ChangeState(EatingState.Instance);
        //}
        if (animationState.normalizedTime >= timeIdle - 0.4) {
            o.GetComponent<StateMachine>().ChangeState(IdleState.Instance);
        }
    }

    override public void Exit(GameObject o) {
        behavior.wanderOn = false;
        behavior.obstacleAvoidanceOn = false;
    }

}
