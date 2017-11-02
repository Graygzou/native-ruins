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

    // Maybe useless
    NavMeshAgent animal;

    public SteeringBehavior(Animaux agent) {

        // Get the animal
        animal = agent.GetComponent<NavMeshAgent>();

        m_MaxSpeed = agent.maxSpeed;

        // set Items
        m_dWanderRadius = 3.0f;
        m_WanderDistance = 7.0f;

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
        Vector3 steeringForceAverage = Wander(agent.transform);

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

    //IEnumerator UpdateSteer()
    //{
    //    // TODO watch tuto
    //    // Tire au hasard un nombre entre 0 et 1
    //    float val = Random.value;
    //    if (val > 0.1)
    //    {
    //        Wander();
    //        yield return new WaitForSeconds(.1f);
    //    }
    //    else
    //    {
    //        // choisie une nouvelle direction
    //        float theta = Random.value * 2 * Mathf.PI;
    //        m_vWanderTarget = new Vector3(m_dWanderRadius * Mathf.Cos(theta), 0f,
    //                                    m_dWanderRadius * Mathf.Sin(theta));
    //        m_vWanderTarget += agent.transform.position + new Vector3(0, 0, distanceProjection);

    //        // Marqué une pause.
    //        yield return new WaitForSeconds(3f);
    //    }

    //    //while (Vector3.Distance(transform.position, target) > 0.05f)
    //    //{
    //    //    transform.position = Vector3.Lerp(transform.position, target, smoothing * Time.deltaTime);

    //    //    yield return null;
    //    //}
    //}

    //public void UpdateSteer() {
    //    // TODO watch tuto
    //    // Tire au hasard un nombre entre 0 et 1
    //    //float val = Random.value;
    //    //if (val > 0.005)
    //    //{
    //        Wander2();
    //    //}
    //    //else
    //    //{
    //    //    // choisie une nouvelle direction
    //    //    float theta = Random.value * 2 * Mathf.PI;
    //    //    m_vWanderTarget = new Vector3(m_dWanderRadius * Mathf.Cos(theta), 0f,
    //    //                                m_dWanderRadius * Mathf.Sin(theta));
    //    //    m_vWanderTarget += agent.transform.position + new Vector3(0, 0, -12f);

    //    //    // Marqué une pause.
    //    //}

    //    //while (Vector3.Distance(transform.position, target) > 0.05f)
    //    //{
    //    //    transform.position = Vector3.Lerp(transform.position, target, smoothing * Time.deltaTime);

    //    //    yield return null;
    //    //}
    //}



    public Vector3 Wander(Transform transformAgent)
    {

        UpdateTimer(transformAgent);
        
        //wanderTarget = (transformAgent.position + transformAgent.forward * m_WanderDistance - wanderTarget).normalized * m_dWanderRadius;

        Vector3 m_DesiredVelocity = (m_vWanderTarget - transformAgent.position).normalized * m_MaxSpeed;

        // translation
        ////Debug.Log(target);
        //Vector3 m_vWanderTarget2 = transformAgent.position + new Vector3(0, 0, distanceProjection) + direction;

        //Debug.Log(agent.transform.position + " et " + m_vWanderTarget2);

        return m_DesiredVelocity;
        //animal.SetDestination(m_vWanderTarget2);

    }

    //public Vector3 Wander(Transform transformAgent) {




    //    transformAgent.rotation = Quaternion.LookRotation(m_vWanderTarget);

    //    //transformAgent.Translate(m_vWanderTarget * 0.001f);

    //    // Update timer
    //    UpdateTimer(transformAgent);

    //    // Add a small perturbation to simulate wander
    //    //Vector3 wanderTarget = m_vWanderTarget + new Vector3(Random.Range(-20.0f, 20.0f), 0f, Random.Range(-1.0f, 1.0f));
    //    // 
    //    //wanderTarget = (wanderTarget - (transformAgent.position + -transformAgent.forward) * m_WanderDistance).normalized * m_dWanderRadius;

    //    //wanderTarget = transformAgent.forward + wanderTarget;

    //    //Vector3 desiredVel = (wanderTarget - transformAgent.position).normalized * m_MaxSpeed * Time.deltaTime;

    //    // then it depends if you are rotating it or not, if rotating:
    //    //transformAgent.rotation = Quaternion.LookRotation(wanderTarget);

    //    // Mise a jour 
    //    //m_vWanderTarget = wanderTarget;

    //    //transformAgent.Translate(wanderTarget * Time.deltaTime);
    //    return m_vWanderTarget;
    //}

    private void UpdateTimer(Transform transformAgent)
    {
        m_Timer += Time.deltaTime;

        if (m_Timer > 1)
        {
            // choisie une nouvelle direction
            float theta = Random.value * 2 * Mathf.PI;
            m_vWanderTarget = transformAgent.position + new Vector3(m_dWanderRadius * Mathf.Cos(theta), 0f,
                                m_dWanderRadius * Mathf.Sin(theta));
            m_vWanderTarget += transformAgent.forward * m_WanderDistance;

            m_Timer = 0;
        }
    }

}
