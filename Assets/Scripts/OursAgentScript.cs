using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class BearScript : Animaux {

    public float degat = 30.0f; // Can be tweak
    public int nbRessourcesDropped = 2;

    public BearScript() : base(80, 5, 50) {
    }

    public void Update() {
        // TODO
    }

    public void Attack() {
        // TODO
    }

    public void Eat() {
        // TODO
    }

    public void Death() {
        // TODO
    }

}

public class Death {
    private static Death instance;

    private Death() { }

    public static Death Instance {
        get {
            if (instance == null) {
                instance = new Death();
            }
            return instance;
        }
    }

    public void Enter(GameObject o)
    {

    }

    public void Execute(GameObject o)
    {

    }

    public void Exit(GameObject o)
    {

    }

}
