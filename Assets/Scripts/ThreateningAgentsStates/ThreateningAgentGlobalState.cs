using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;



public class ThreateningAgentGlobalState : State<GameObject>
{
    private static ThreateningAgentGlobalState instance;

    private ThreateningAgentGlobalState() { }

    public static ThreateningAgentGlobalState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ThreateningAgentGlobalState();
            }
            return instance;
        }
    }

    override public void Enter(GameObject o) { /* Empty */ }

    override public void Execute(GameObject o)
    {
        // checker random
            // Si good => Idle
        // checker si le joueur est en vue
            // Si oui => Attack
        // ------------------------------------------
        // OU utiliser Coroutine pour eviter cet état.
    }

    override public void Exit(GameObject o) { /* Empty */ }

}