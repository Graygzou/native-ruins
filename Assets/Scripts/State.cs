using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour {

    //this will execute when the state is entered
    public abstract void Enter(GameObject o);

    //this is the states normal update function
    public abstract void Execute(GameObject o);

    //this will execute when the state is exited. 
    public abstract void Exit(GameObject o);
}
