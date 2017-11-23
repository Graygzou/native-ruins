using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackState : State<GameObject>
{
    private static AttackState instance;
    private float timer;

    private AttackState() { }

    public static AttackState Instance {
        get {
            if (instance == null) {
                instance = new AttackState();
            }
            return instance;
        }
    }

    override public void Enter(GameObject o) {
        // Get the current agent variables
        StateMachine FSM = o.GetComponent<StateMachine>();
        FSM.animator.Play("Attack");

        // Set the number of the the state is play
        FSM.timeIdle = 0.7f;
    }

    override public void Execute(GameObject o) {
        // Get the current agent variables
        StateMachine FSM = o.GetComponent<StateMachine>();

        float currTime = o.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (currTime >= FSM.timeIdle - 0.06) {
            AgentProperties properties = o.GetComponent<AgentProperties>();
            GameObject playerRoot = GameObject.Find("Player");
            GameObject lifeBar = GameObject.Find("Gauges/Life");

            RaycastHit hitInfo;
            if (Physics.SphereCast(new Ray(properties.getFront().position, properties.getFront().forward), 5f, out hitInfo, 0.5f)) {
                // check if we can attack the player
                if (hitInfo.transform.tag == "Player") {
                    if (lifeBar.GetComponent<LifeBar>().GetComponent<Scrollbar>().size != 0) {
                        // Decrease the current life of the player
                        lifeBar.GetComponent<LifeBar>().TakeDamages(properties.damages / 100);
                        Debug.Log("Take that ! dmg:" + properties.damages);
                    }
                }
            }
            FSM.ChangeState(TauntState.Instance);
        }
        
    }

    override public void Exit(GameObject o) {
        StateMachine FSM = o.GetComponent<StateMachine>();
    }

}
