using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OursAgentScript : Animaux {

    //This is an example of an auto-implemented
    //property
    public int Health { get; set; }

    //public Transform target;

    public OursAgentScript() : base(100, 10, 50) {
    }

    // Use this for initialization
    //void Start () {
        
    //}

    // Update is called once per frame
 //   void Update () {
 //       //Debug.Log(gameObject.transform.forward);
 //       //print(transform.position.x);
 //       //Wander();
	//}
}
