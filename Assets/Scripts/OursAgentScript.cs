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

public abstract class Singleton {
    private static Singleton instance;

    private Singleton() { }

    public static Singleton Instance {
        get {
            if (instance == null) {
                instance = new Singleton();
            }
            return instance;
        }
    }

    public abstract void Enter(GameObject o);

    public abstract void Execute(GameObject o);

    public abstract void Exit(GameObject o);

}
