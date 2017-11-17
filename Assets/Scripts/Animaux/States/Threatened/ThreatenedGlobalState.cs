using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThreatenedGlobalState : State<GameObject>
{
    private static ThreatenedGlobalState instance;

    private ThreatenedGlobalState() { }

    public static ThreatenedGlobalState Instance {
        get
        {
            if (instance == null) {
                instance = new ThreatenedGlobalState();
            }
            return instance;
        }
    }

    override public void Enter(GameObject o) {
        // change the music ?
        // Jouer l'animation (fumé qui sort de la tete / narines, par ex)
    }

    override public void Execute(GameObject o) {
        StateMachine FSM = o.GetComponent<StateMachine>();
        AgentProperties properties = o.GetComponent<AgentProperties>();
        GameObject lifeBar = GameObject.Find("Gauges/Life");

        // Tant que vivant && vie > 50% && joueur proche
        if (properties.isDead) {
            if(o.GetComponent<StateMachine>().getCurrentState() != DeathState.Instance) {
                FSM.ChangeState(DeathState.Instance);
            }
        } else if (lifeBar.GetComponent<LifeBar>().GetComponent<Scrollbar>().size == 0) {
            FSM.ChangeState(WalkingState.Instance);
        } else if ((properties.getCurrentHealth() * 100) / properties.maxHealth < 50) {
            FSM.ChangeState(EvadeState.Instance);
        } else if (!properties.isAlert) {
            FSM.ChangeGlobalState(RegularGlobalState.Instance);
            FSM.ChangeState(WalkingState.Instance);
        }
    }

    override public void Exit(GameObject o) {
        // Change music ?
    }

}
