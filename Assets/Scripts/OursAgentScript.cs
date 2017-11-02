using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OursAgentScript : Animaux {

    public OursAgentScript() : base(100, 5, 50) {
    }

    // Use for debugging
    void OnDrawGizmos()
    {

        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(transform.position, transform.position + transform.forward * 10);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);


        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.position + GetComponent<Rigidbody>().velocity.normalized * steering.m_WanderDistance);
        Gizmos.DrawWireSphere(transform.position + transform.forward * steering.m_WanderDistance, steering.m_dWanderRadius);
        Gizmos.DrawWireSphere(steering.m_vWanderTarget, 0.33f);

        //Gizmos.color = Color.green;

        //Gizmos.DrawLine(transform.position + GetComponent<Rigidbody>().velocity.normalized * steering.m_WanderDistance, steering.m_vWanderTarget);

        // -----------------------------------------

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5);
        Gizmos.DrawLine(transform.position + transform.forward * 1.5f, transform.position + Quaternion.Euler(0, 16, 0) * transform.forward * 5);
        Gizmos.DrawLine(transform.position + transform.forward * 1.5f, transform.position + Quaternion.Euler(0, -16, 0) * transform.forward * 5);


        Gizmos.color = Color.gray;
        Gizmos.DrawLine(transform.position, transform.position + steering.m_DesiredVelocity);
    }

}
