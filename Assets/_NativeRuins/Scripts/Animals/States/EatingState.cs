using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingState : State<GameObject>
{
    private static EatingState instance;
    // Do not add more variables here

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
        // Get the current agent variables
        StateMachine FSM = o.GetComponent<StateMachine>();
        AgentProperties properties = o.GetComponent<AgentProperties>();

        properties.setSpeed(0.0f);

        // Stop the previous animation
        FSM.animator.Play("Eat");

        // Set the number of time that the state will be play
        // FOR the current agent
        FSM.timeIdle = (int)Mathf.Round(Random.Range(2.0f, 4.0f));
    }

    override public void Execute(GameObject o)
    {
        // Get the current agent variables
        StateMachine FSM = o.GetComponent<StateMachine>();
        AgentProperties properties = o.GetComponent<AgentProperties>();


        // Update of the variable if needed
        if (properties.hungryIndicator > 0.0f) {
            properties.hungryIndicator -= 2f;
        }

        float currTime = o.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (currTime >= FSM.timeIdle - 0.06) {
            // Launch a coroutine to accelerate POLISH
            // o.GetComponent<AgentProperty>().StartCoroutine("AccelerateWalk");
            // ---anim.SetBool("IsHungry", false);
            // Change state
            o.GetComponent<StateMachine>().ChangeState(WalkingState.Instance);
            // Make the transition for the animation
            FSM.animator.SetFloat("Speed_f", 0.5f);
        }
    }

    override public void Exit(GameObject o) {
        // Get the current agent variables
        StateMachine FSM = o.GetComponent<StateMachine>();

        FSM.animator.SetBool("IsHungry", false);
    }
}
