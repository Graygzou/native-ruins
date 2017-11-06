using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State<GameObject>
{
    private static DeathState instance;

    private DeathState() { }

    public static DeathState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DeathState();
            }
            return instance;
        }
    }

    override public void Enter(GameObject o)
    {
        // Enlever le collider / rigidbody ?
    }

    override public void Execute(GameObject o)
    {
        // Jouer l'animation
        // Sink dans le sol
    }

    override public void Exit(GameObject o)
    {
        // Generer de la poussiere
        // Generer l'item avec une animation "bas-haut"
    }
}
