using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OursAgentScript : MonoBehaviour {

    private Vector3 m_vWanderTarget; // Can be player or another position.

    //This is an example of an auto-implemented
    //property
    public int Health { get; set; }

    //public Transform target;
    NavMeshAgent agent;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();

        //stuff for the wander behavior
        float theta = Random.value * 2 * Mathf.PI;
        float m_dWanderRadius = 10f;
        //create a vector to a target position on the wander circle
        m_vWanderTarget = new Vector3(m_dWanderRadius * Mathf.Cos(theta), 0f,
                                    m_dWanderRadius * Mathf.Sin(theta));

    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(gameObject.transform.forward);
        //print(transform.position.x);
        Wander();
	}

    public void Wander()
    {
        Vector3 test = new Vector3(Random.Range(-1.0f, 1.0f), 0f, Random.Range(-1.0f, 1.0f));
        Debug.Log(test);
        Vector3 wanderTarget = m_vWanderTarget + test;

        //reproject this new vector back on to a unit circle
        wanderTarget.Normalize();

        wanderTarget *= 3;
        wanderTarget += agent.transform.forward * 6f;
        //Debug.Log(target);
        m_vWanderTarget = wanderTarget;

        agent.SetDestination(m_vWanderTarget);

        //cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = target;
        //cube.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
    }
}
