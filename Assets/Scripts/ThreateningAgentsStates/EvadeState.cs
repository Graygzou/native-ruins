using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : State<GameObject> {
    private static EvadeState instance;

    private EvadeState() { }

    public static EvadeState Instance {
        get {
            if (instance == null) {
                instance = new EvadeState();
            }
            return instance;
        }
    }

    override public void Enter(GameObject o) {
        // Lance coroutine 
        // joue l'animation
        // 
    }

    override public void Execute(GameObject o) {
        // checker le boolean
    }

    override public void Exit(GameObject o) {
        // Desalocate le joueur
    }
}
