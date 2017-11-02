using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SteeringBehavior : MonoBehaviour {

    // Variables for the wander
    public float m_WanderDistance { get; set; }
    public float m_dWanderRadius { get; set; }
    public Vector3 m_vWanderTarget;
    public Vector3 m_DesiredVelocity { get; set; }
    public float m_MaxSpeed { get; set; }

    private float m_Timer = 0;

    // Variables for the obstacles avoidance
    public float m_BoundingSphereRadius;
    public float m_ObstacleMaxDistance;
    public List<Ray> m_Senseurs;

    // Maybe useless
    NavMeshAgent animal;

    public SteeringBehavior(Animaux agent) {

        // Get the animal
        animal = agent.GetComponent<NavMeshAgent>();

        m_MaxSpeed = agent.maxSpeed;

        // set Items
        m_dWanderRadius = 3.0f;
        m_WanderDistance = 7.0f;

        m_BoundingSphereRadius = 1.0f;
        m_ObstacleMaxDistance = 10.0f;

        m_Senseurs.Add(new Ray(agent.transform.position, agent.transform.forward));
        m_Senseurs.Add(new Ray(agent.transform.position, Quaternion.Euler(0, 25, 0) * agent.transform.forward));
        m_Senseurs.Add(new Ray(agent.transform.position, Quaternion.Euler(0, -25, 0) * agent.transform.forward));

        // Set velocity
        Vector3 velocity = agent.transform.forward * m_MaxSpeed;
        velocity.y = agent.GetComponent<Rigidbody>().velocity.y;
        agent.GetComponent<Rigidbody>().velocity = velocity;

        // Create first random point
        float theta = Random.value * 2 * Mathf.PI;
        m_vWanderTarget = new Vector3(m_dWanderRadius * Mathf.Cos(theta), 0f,
                                    m_dWanderRadius * Mathf.Sin(theta));
        m_vWanderTarget += agent.transform.position + agent.transform.forward * m_WanderDistance;

        //Debug.Log("first: " + agent.transform.position + " et " + m_vWanderTarget2);
    }

    public void UpdateBehavior(Animaux agent) {
        //StartCoroutine(UpdateSteer());
        Vector3 steeringForceAverage;
        steeringForceAverage = ObstancleAvoidance(agent) * 2;
        steeringForceAverage += new Vector3(0, 0, 1); //Wander(agent.transform) * 1;

        Debug.Log(steeringForceAverage);

        steeringForceAverage = Vector3.ClampMagnitude(steeringForceAverage, m_MaxSpeed);

        // Add steering force to velocity
        agent.GetComponent<Rigidbody>().velocity = steeringForceAverage;
        //m_Velocity += steeringForceAverage;
        //transform.position += m_Velocity * Time.deltaTime;

        // Update rotation
        if (agent.GetComponent<Rigidbody>().velocity.sqrMagnitude > 1) {
            Debug.Log(agent.GetComponent<Rigidbody>().velocity);

            agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, 
                Quaternion.LookRotation(agent.GetComponent<Rigidbody>().velocity), Time.deltaTime * 5);
        }

        agent.transform.Translate(steeringForceAverage * Time.deltaTime);

    }

    public Vector3 Wander(Transform transformAgent) {
        UpdateTimer(transformAgent);
        Vector3 m_DesiredVelocity = (m_vWanderTarget - transformAgent.position).normalized * m_MaxSpeed;
        return m_DesiredVelocity;
    }

    public Vector3 ObstancleAvoidance(Animaux agent) {

        Vector3 m_DesiredVelocity = (m_vWanderTarget - agent.transform.position).normalized * m_MaxSpeed;

        // Get most threatening obstacle
        RaycastHit hitInfo;
        Ray obstacleRay = new Ray(agent.transform.position, agent.transform.forward);
        Vector3 avoidanceForce = Vector3.zero;


        // Calculate avoidance force
        if (Physics.Raycast(obstacleRay, out hitInfo, m_ObstacleMaxDistance)) {
            avoidanceForce = Vector3.Reflect(agent.transform.forward * m_MaxSpeed, hitInfo.normal);
            Vector3 t = m_DesiredVelocity;
            Debug.Log("test");
        }

        // Calculate avoidance force
        //if (Physics.SphereCast(ray, m_BoundingSphereRadius, out hitInfo, m_ObstacleMaxDistance, m_LayerMask))
        //{
        //    if (Vector3.Angle(hitInfo.normal, transform.up) > 45)
        //    {
        //        avoidanceForce = Vector3.Reflect(SteeringCore.Velocity, hitInfo.normal);

        //        if (Vector3.Dot(avoidanceForce, SteeringCore.Velocity) < -0.9f)
        //        {
        //            avoidanceForce = transform.right;
        //        }
        //    }
        //}

        return avoidanceForce;
    }

    private void UpdateTimer(Transform transformAgent) {
        m_Timer += Time.deltaTime;

        if (m_Timer > 1) {
            // choisie une nouvelle direction
            float theta = Random.value * 2 * Mathf.PI;
            m_vWanderTarget = transformAgent.position + new Vector3(m_dWanderRadius * Mathf.Cos(theta), 0f,
                                m_dWanderRadius * Mathf.Sin(theta));
            m_vWanderTarget += transformAgent.forward * m_WanderDistance;
            m_Timer = 0;
        }
    }

}
