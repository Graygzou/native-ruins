using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OursAgentScript : MonoBehaviour {

    private Vector3 target; // Can be player or another position.

    //This is an example of an auto-implemented
    //property
    public int Health { get; set; }

    //public Transform target;
    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        target = gameObject.transform.forward * 2;
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(gameObject.transform.forward);
        print(transform.position.x);
        //agent.SetDestination(transform.forward * 3);  
        //agent.Move(transform.forward);
        Wander();
	}

    public void Wander()
    {
        int i = 0;
        while (i < 2) {
            Vector2 vectorChange = Random.insideUnitCircle * 3;
            target += new Vector3(vectorChange.x, 0f, vectorChange.y);

            //reproject this new vector back on to a unit circle
            target.Normalize();

            target *= 12;

            Debug.Log(gameObject.transform.forward);
            agent.SetDestination(target);
            //agent.Move(target);
            i++;
        }
    }


}
