using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntState : State<GameObject>
{
    private static TauntState instance;
    // Do not add more variables here

    private TauntState() { }

    public static TauntState Instance {
        get {
            if (instance == null) {
                instance = new TauntState();
            }
            return instance;
        }
    }

    override public void Enter(GameObject o) {
        // Get the current agent variables
        StateMachine FSM = o.GetComponent<StateMachine>();

        // Set the animation variables
        FSM.animator.SetBool("Taunt", true);
        FSM.animator.Play("Taunt");
        AnimatorStateInfo animationState = FSM.animator.GetCurrentAnimatorStateInfo(0);

        AnimatorClipInfo[] an = FSM.animator.GetCurrentAnimatorClipInfo(0);

        // Set the number of the the state is play
        FSM.timeIdle = 1f;
    }

    override public void Execute(GameObject o)
    {
        // Get the current agent variables
        StateMachine FSM = o.GetComponent<StateMachine>();
        AgentProperties properties = o.GetComponent<AgentProperties>();

        AnimatorStateInfo animationState = FSM.animator.GetCurrentAnimatorStateInfo(0);

        AnimatorClipInfo[] an = FSM.animator.GetCurrentAnimatorClipInfo(0);

        float currTime = o.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (currTime >= FSM.timeIdle - 0.06) {
            // Change state
            RaycastHit hitInfo;
            if (Physics.Raycast(new Ray(properties.getFront().position, properties.getFront().forward), out hitInfo, 0.5f)) {
                FSM.ChangeState(AttackState.Instance);
            } else {
                FSM.ChangeState(ChargeState.Instance);
            }
        }
    }

    override public void Exit(GameObject o) {
        StateMachine FSM = o.GetComponent<StateMachine>();
        FSM.animator.SetBool("Taunt", false);
    }
}
