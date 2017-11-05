using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingState : State<GameObject>
{
    private static EatingState instance;

    private Animator anim;
    public Animation animation;
    private AgentProperty properties;
    private float timeIdle;
    private float timer;

    private EatingState() { }

    public static EatingState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EatingState();
            }
            return instance;
        }
    }

    override public void Enter(GameObject o)
    {
        anim = o.GetComponent<Animator>();
        properties = o.GetComponent<AgentProperty>();
        // Set the environnement to get the proper animation POLISH
        //for (float i = anim.GetFloat("Speed_f"); i > 0f; i -= 0.1f) {
        //    anim.SetFloat("Speed_f", i);
        //}
        anim.SetFloat("Speed_f", 0f);
        anim.SetBool("IsHungry", true);
        // Get the length of the animation
        var animationState = anim.GetCurrentAnimatorStateInfo(0);
        if (!animationState.IsName("Deer_Eat")) { Debug.Log("WAHTTTTTT"); }

        // TODO FAIRE PAREIL POUR TOUS !!!

        var animationClips = anim.GetCurrentAnimatorClipInfo(0);
        if (animationClips.Length == 0)
        { Debug.Log("WAHTTTTTT"); }

        var animationClip = animationClips[0].clip;
        var animationTime = animationClip.length * animationState.normalizedTime;


        AnimatorStateInfo currInfo = anim.GetCurrentAnimatorStateInfo(0);
        timeIdle = Mathf.Round(Random.Range(1.0f, 3.0f)) * animationTime;
        timer = 0.0f;
    }

    override public void Execute(GameObject o)
    {
        timer += Time.deltaTime;

        // Update of the variable if needed
        if (properties.hungryIndicator > 0.0f) {
            properties.hungryIndicator -= 0.1f;
        }

        if (timer >= timeIdle) {
            // Launch a coroutine to accelerate POLISH
            // o.GetComponent<AgentProperty>().StartCoroutine("AccelerateWalk");
            anim.SetBool("IsHungry", false);
            // Change state
            o.GetComponent<StateMachine>().ChangeState(WalkingState.Instance);
        }

    }

    override public void Exit(GameObject o)
    {
        // nothing
    }
}
