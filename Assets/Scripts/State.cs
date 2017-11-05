using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T> {

    private bool isFinished = false;

    //this will execute when the state is entered
    public abstract void Enter(T o);

    //this is the states normal update function
    public abstract void Execute(T o);

    //this will execute when the state is exited. 
    public abstract void Exit(T o);

    public void AnimationDone() {
        isFinished = true;
    }
}
