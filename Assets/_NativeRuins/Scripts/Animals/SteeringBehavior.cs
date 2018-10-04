using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SteeringBehavior : MonoBehaviour {

    [SerializeField]
    private Transform noseTransform;

    // Variables for the wander
    public float WanderDistance { get; set; }
    public float dWanderRadius { get; set; }
    public Vector3 vWanderTarget;

    public Vector3 target_p;
    public Vector3 desiredVelocity { get; set; }

    public float minDetectionBoxLength;

    private float timer = 0;

    public bool wallAvoidanceOn;
    public bool obstacleAvoidanceOn;
    public bool wanderOn;
    public bool fleeOn;
    public bool seekOn;

    float theta;
    public float enthropy = 0.2f;

    // Variables for the obstacles avoidance
    public float boundingSphereRadius;
    public float obstacleMaxDistance;
    public Dictionary<Ray, float> senseurs;

    private AgentProperties properties;
    private Rigidbody rigidbody;

    private void Awake()
    {
        // Get the animal properties
        properties = GetComponent<AgentProperties>();

        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // Set Items
        dWanderRadius = 3.0f;
        WanderDistance = 7.0f;

        boundingSphereRadius = 1.0f;
        obstacleMaxDistance = 10.0f;

        senseurs = new Dictionary<Ray, float>();

        // Create first random point
        /*
        theta = Random.value * 2 * Mathf.PI;
        vWanderTarget = new Vector3(dWanderRadius * Mathf.Cos(theta), 0f,
                                    dWanderRadius * Mathf.Sin(theta));
        vWanderTarget += noseTransform.position + noseTransform.forward * WanderDistance;
        vWanderTarget.y = noseTransform.position.y;*/
    }

    public void FixedUpdate()
    {
        UpdateBehavior();
    }

    /* -----------------------------------------
     * Update the steering behavior of the agent
     * ----------------------------------------- */
    public void UpdateBehavior() {
        ComputeYValue();
        //StartCoroutine(UpdateSteer());
        Vector3 m_vSteeringForce = CalculatePrioritized() * properties.maxSpeed;

        // The force has to be on a plane (Right now)
        m_vSteeringForce.y = 0;

        Debug.Log(m_vSteeringForce);

        //make sure vehicle does not exceed maximum velocity
        m_vSteeringForce = Vector3.ClampMagnitude(m_vSteeringForce, properties.maxForce);

        //update the heading if the vehicle has a non zero velocity
        if (m_vSteeringForce.sqrMagnitude > 0.000001)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(m_vSteeringForce), Time.deltaTime * 10f);
        }

        // Normalise the movement vector and make it proportional to the speed per second.
        Vector3 movement = m_vSteeringForce;
        rigidbody.AddForce(m_vSteeringForce);
    }

    private void ComputeYValue()
    {

    }

    public Vector3 CalculatePrioritized() {
        Vector3 force;
        Vector3 steeringForceAverage = Vector3.zero;
        /*
        if (wanderOn) {
            steeringForceAverage = new Vector3(0.0f, 0.0f, 1.0f); ;
        }*/

        if (wallAvoidanceOn) {
            Debug.Log("Avoid");
            force = WallAvoidance() * 10.0f;
            if (force.magnitude < properties.maxForce - steeringForceAverage.magnitude) {
                // We add the value to the vector
                steeringForceAverage += force;
            } else {
                //add it to the steering force
                steeringForceAverage += (force.normalized * (properties.maxForce - steeringForceAverage.magnitude));
                return steeringForceAverage;
            }
        }

        if (obstacleAvoidanceOn) {
            force = ObstaclesAvoidance() * 10.0f;
            if (force.magnitude < properties.maxForce - steeringForceAverage.magnitude) {
                // We add the value to the vector
                steeringForceAverage += force;
            } else {
                //add it to the steering force
                steeringForceAverage += (force.normalized * (properties.maxForce - steeringForceAverage.magnitude));
                return steeringForceAverage;
            }
        }

        if (wanderOn) {
            Debug.Log("Wander");
            force = Wander() * 2.0f;
            if (force.magnitude < properties.maxForce - steeringForceAverage.magnitude) {
                // We add the value to the vector
                steeringForceAverage += force;
            } else {
                //add it to the steering force
                steeringForceAverage += (force.normalized * (properties.maxForce - steeringForceAverage.magnitude));
                return steeringForceAverage;
            }
        }

        if (fleeOn) {
            Debug.Log("Flee");
            force = Flee() * 2.0f;
            if (force.magnitude < properties.maxForce - steeringForceAverage.magnitude) {
                // We add the value to the vector
                steeringForceAverage += force;
            } else {
                //add it to the steering force
                steeringForceAverage += (force.normalized * (properties.maxForce - steeringForceAverage.magnitude));
                return steeringForceAverage;
            }
        }

        if (seekOn) {
            Debug.Log("Seek");
            force = Seek() * 2.0f;
            if (force.magnitude < properties.maxForce - steeringForceAverage.magnitude) {
                // We add the value to the vector
                steeringForceAverage += force;
            } else {
                //add it to the steering force
                steeringForceAverage += (force.normalized * (properties.maxForce - steeringForceAverage.magnitude));
                return steeringForceAverage;
            }
        }
        return steeringForceAverage;
    }

    #region Seek
    public Vector3 Seek()
    {
        Vector3 m_DesiredVelocity = Vector3.zero;
        //NavMeshAgent agent = GetComponent<NavMeshAgent>();
        //if (agent != null) {
        //    agent.SetDestination(target_p.position);
        //} else {
        // Redefinir A* ici ?
        //vWanderTarget = (target_p - transform.position).normalized * WanderDistance;

        m_DesiredVelocity = target_p - noseTransform.position;// * properties.maxSpeed;

        //}
        return m_DesiredVelocity;
    }
    #endregion

    #region Flee
    public Vector3 Flee()
    {
        Vector3 m_DesiredVelocity = Vector3.zero;
        //Vector3 m_DesiredVelocity = (transform.position - target_p.position).normalized * maxSpeed;
        m_DesiredVelocity = noseTransform.position - target_p;
        return m_DesiredVelocity;
    }
    #endregion

    #region Wander
    public Vector3 Wander() {
        Vector3 m_DesiredVelocity;

        timer += Time.deltaTime;
        if (timer > 1.7) {
            CalculateNewWanderTarget();
            timer = 0;
        }
        m_DesiredVelocity = vWanderTarget - noseTransform.position;
        return m_DesiredVelocity;
    }

    private void CalculateNewWanderTarget()
    {
        // get a new direction
        theta += UnityEngine.Random.Range(-enthropy, enthropy) * 2 * Mathf.PI;
        vWanderTarget = noseTransform.position + new Vector3(dWanderRadius * Mathf.Cos(theta), 0f, dWanderRadius * Mathf.Sin(theta));
        vWanderTarget += noseTransform.forward * WanderDistance;
        vWanderTarget.y = noseTransform.position.y;
    }
    #endregion

    #region Avoidance
    public Vector3 WallAvoidance(){ 
        // Get most threatening obstacle
        RaycastHit hitInfo;

        senseurs.Add(new Ray(transform.position, transform.forward), 5);
        senseurs.Add(new Ray(transform.position + transform.forward * 1.5f, Quaternion.Euler(0, 16, 0) * transform.forward), 5);
        senseurs.Add(new Ray(transform.position + transform.forward * 1.5f, Quaternion.Euler(0, -16, 0) * transform.forward), 5);

        //Ray obstacleRay = new Ray(agent.transform.position, agent.transform.forward);
        Vector3 avoidanceForce = Vector3.zero;

        foreach (Ray r in senseurs.Keys)
        {
            // Calculate avoidance force
            if (Physics.Raycast(r, out hitInfo, senseurs[r]))
            {
                avoidanceForce += Vector3.Reflect(transform.forward * properties.getCurrentSpeed(), hitInfo.normal) * (senseurs[r] - hitInfo.distance);
                //avoidanceForce += hitInfo.normal * (m_Senseurs[r] - hitInfo.distance);
            }
        }
        senseurs = new Dictionary<Ray, float>();

        return avoidanceForce.normalized;
    }

    public Vector3 ObstaclesAvoidance() {
        Vector3 m_DesiredVelocity = Vector3.zero;

        RaycastHit hitInfo;
        // Get objects on the way with raycast
        Ray ray = new Ray(transform.position, transform.forward);
        

        Debug.DrawRay(transform.position, transform.forward * minDetectionBoxLength);

        if (Physics.SphereCast(ray, 1.5f, out hitInfo, minDetectionBoxLength)) {
            if (hitInfo.transform != transform && !(hitInfo.collider is TerrainCollider)) {
                m_DesiredVelocity = (hitInfo.point - hitInfo.collider.bounds.center) * 50f;
                //m_DesiredVelocity = hitInfo.normal * properties.maxSpeed;
                //m_DesiredVelocity = new Vector3(m_DesiredVelocity.x, 0f, 0f) / hitInfo.distance;
            }
        }
        return m_DesiredVelocity * properties.getCurrentSpeed();
    }
    #endregion

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(noseTransform.position, noseTransform.position + noseTransform.forward);

        Gizmos.color = Color.black;
        //Gizmos.DrawLine(transform.position, transform.forward + GetComponent<Rigidbody>().velocity.normalized * WanderDistance);
        Gizmos.DrawWireSphere(noseTransform.position + noseTransform.forward * WanderDistance, dWanderRadius);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(vWanderTarget, 0.33f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(noseTransform.position, noseTransform.position + noseTransform.forward * 5);
        Gizmos.DrawLine(noseTransform.position + noseTransform.forward * 1.5f, noseTransform.position + Quaternion.Euler(0, 16, 0) * noseTransform.forward * 5);
        Gizmos.DrawLine(noseTransform.position + noseTransform.forward * 1.5f, noseTransform.position + Quaternion.Euler(0, -16, 0) * noseTransform.forward * 5);

        Gizmos.color = Color.gray;
        Gizmos.DrawLine(noseTransform.position, noseTransform.position + desiredVelocity);
    }

}