using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : State<GameObject> {

    private static WalkingState instance;
    // Do not add more variables here

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
        StateMachine FSM = o.GetComponent<StateMachine>();
        AgentProperties properties = o.GetComponent<AgentProperties>();

        properties.setSpeed(40.0f);

        // Set the animation variables
        FSM.animator.SetFloat("Speed_f", 0.5f);
        FSM.behavior.wanderOn = true;
        //FSM.behavior.obstacleAvoidanceOn = true;
        FSM.behavior.wallAvoidanceOn = true;
        FSM.animator.Play("Locomotion");

        // make sure we're in the good state animation
        AnimatorStateInfo animationState = FSM.animator.GetCurrentAnimatorStateInfo(0);
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
        FSM.timeIdle = (int)Mathf.Round(Random.Range(4.0f, 6.0f));
        FSM.time = animationState.normalizedTime;
    }

    override public void Execute(GameObject o) {
        StateMachine FSM = o.GetComponent<StateMachine>();
        AgentProperties properties = o.GetComponent<AgentProperties>();

        // Udpate variables
        properties.hungryIndicator += 0.1f;

        // Check for transitions
        if (properties.hungryIndicator >= 50.0f) {
            // Launch a coroutine to accelerate POLISH
            // o.GetComponent<AgentProperty>().StartCoroutine("DecelerateWalk");
            FSM.animator.SetBool("IsHungry", true);
            FSM.animator.SetFloat("Speed_f", 0.0f);
            o.GetComponent<StateMachine>().ChangeState(EatingState.Instance);
        }
        else {
            float currTime = o.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (currTime >= FSM.time + FSM.timeIdle - 0.06) {
                FSM.ChangeState(IdleState.Instance);
            }
        }
    }

    override public void Exit(GameObject o) {
        StateMachine FSM = o.GetComponent<StateMachine>();

        FSM.animator.SetBool("IsHungry", false);
        FSM.animator.SetFloat("Speed_f", 0.0f);
        FSM.behavior.wanderOn = false;
        FSM.behavior.obstacleAvoidanceOn = false;
    }

}
