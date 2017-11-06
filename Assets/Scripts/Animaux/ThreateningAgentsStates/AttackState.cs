using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<GameObject>
{
    private static AttackState instance;

    private AttackState() { }

    public static AttackState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AttackState();
            }
            return instance;
        }
    }

    override public void Enter(GameObject o)
    {
        // recupere le joueur a attaquer
    }

    override public void Execute(GameObject o)
    {
        // Tant que vivant && vie > 50% && joueur proche
        // Jouer l'animation (fumé qui sort de la tete / narines, par ex)
        // Stocke la position du joueur et FONCE DEDANS !
        // Se replacer pour reattaquer.
        // Si vie < 50%
        // => Evade
        // Si !vivant
        // => Death
        // sinon
        // => Eat
    }

    override public void Exit(GameObject o)
    {
        // Desalocate le joueur
    }

}
