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

    private float m_dWanderRadius;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();

        //stuff for the wander behavior
        float theta = Random.value * 2 * Mathf.PI;
        m_dWanderRadius = 3f;
        //create a vector to a target position on the wander circle
        m_vWanderTarget = new Vector3(m_dWanderRadius * Mathf.Cos(theta), 0f,
                                    m_dWanderRadius * Mathf.Sin(theta));
        // Translation
        m_vWanderTarget += transform.position + new Vector3(0,0,10);

        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = m_vWanderTarget;
        //cube.GetComponent<Renderer>().material.color = new Color(0, 255, 52);
        Debug.Log("first: " + transform.position + " et " + m_vWanderTarget);
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log(gameObject.transform.forward);
        //print(transform.position.x);
        Wander();
	}

    public void Wander()
    {
        Vector3 test = new Vector3(Random.Range(-20.0f, 20.0f), 0f, Random.Range(-20.0f, 20.0f));
        Vector3 wanderTarget = m_vWanderTarget + test;

        //reproject this new vector back on to a unit circle
        wanderTarget.Normalize();

        // Relative vector
        Vector3 relativeVector = transform.position - m_vWanderTarget;
        relativeVector.Normalize();
        // Multiplie par le rayon du cercle wander
        wanderTarget *= m_dWanderRadius;
        // translation
        wanderTarget += transform.position + relativeVector * 10f;
        ////Debug.Log(target);
        m_vWanderTarget = wanderTarget;


        Debug.Log(transform.position + " et " + m_vWanderTarget);
        agent.SetDestination(m_vWanderTarget);

        //cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = target;
        //cube.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
    }
}
