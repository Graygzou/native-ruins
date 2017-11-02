using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OursAgentScript : Animaux {

    public OursAgentScript() : base(100, 5, 50) {
    }

    // Use for debugging
    void OnDrawGizmos() {

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 10);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        

        Gizmos.color = Color.black;
        //Gizmos.DrawLine(transform.position, transform.position + GetComponent<Rigidbody>().velocity.normalized * steering.m_WanderDistance);
        //Gizmos.DrawWireSphere(transform.position + transform.forward * steering.m_WanderDistance, steering.m_dWanderRadius);

        Gizmos.color = Color.green;
    
        //Gizmos.DrawLine(transform.position + GetComponent<Rigidbody>().velocity.normalized * steering.m_WanderDistance, steering.m_vWanderTarget);

        // -----------------------------------------

        Gizmos.color = Color.magenta;
        //foreach(Ray r in steering.m_Senseurs) {
        //    Gizmos.DrawLine(transform.position, transform.position + r.direction * steering.m_ObstacleMaxDistance);
        //}
        

        Gizmos.color = Color.gray;
        Gizmos.DrawLine(transform.position, transform.position + steering.m_DesiredVelocity);
    }

    // Update is called once per frame
    //   void Update () {
    //       //Debug.Log(gameObject.transform.forward);
    //       //print(transform.position.x);
    //       //Wander();
    //}
}
