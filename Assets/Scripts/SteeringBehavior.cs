using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class SteeringBehavior : MonoBehaviour {

    // TODO : box wander a modifier inspector

    // Variables for the wander
    public float WanderDistance { get; set; }
    public float dWanderRadius { get; set; }
    public Vector3 vWanderTarget;
    public Vector3 desiredVelocity { get; set; }
    public float maxSpeed { get; set; }

    public float minDetectionBoxLength = 7.0f;

    public float maxForce;

    private float timer = 0;

    public bool wallAvoidanceOn;
    public bool obstacleAvoidanceOn;
    public bool wanderOn;

    float theta;

    // Variables for the obstacles avoidance
    public float boundingSphereRadius;
    public float obstacleMaxDistance;
    public Dictionary<Ray, float> senseurs;

    // Maybe useless
    NavMeshAgent animal;

    void Start() {

        // Get the animal
        animal = GetComponent<NavMeshAgent>();

        maxSpeed = GetComponent<NavMeshAgent>().speed;

        // set Items
        dWanderRadius = 3.0f;
        WanderDistance = 7.0f;

        maxForce = 200;

        boundingSphereRadius = 1.0f;
        obstacleMaxDistance = 10.0f;

        senseurs = new Dictionary<Ray, float>();

        // Set velocity
        Vector3 velocity = transform.forward * maxSpeed;
        velocity.y = GetComponent<Rigidbody>().velocity.y;
        GetComponent<Rigidbody>().velocity = velocity;

        // Create first random point
        theta = Random.value * 2 * Mathf.PI;
        vWanderTarget = new Vector3(dWanderRadius * Mathf.Cos(theta), 0f,
                                    dWanderRadius * Mathf.Sin(theta));
        vWanderTarget += transform.position + transform.forward * WanderDistance;

        //Debug.Log("first: " + agent.transform.position + " et " + m_vWanderTarget2);
    }

    // Update is called once per frame
    void Update() {
        UpdateBehavior();
    }

    /* -----------------------------------------
     * Update the steering behavior of the agent
     * ----------------------------------------- */
    public void UpdateBehavior() {
        //StartCoroutine(UpdateSteer());
        Vector3 m_vSteeringForce = CalculatePrioritized();

        //make sure vehicle does not exceed maximum velocity
        Vector3.ClampMagnitude(m_vSteeringForce, maxSpeed);

        GetComponent<Rigidbody>().velocity = m_vSteeringForce;

        //update the heading if the vehicle has a non zero velocity
        if (GetComponent<Rigidbody>().velocity.sqrMagnitude > 0.000001)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(GetComponent<Rigidbody>().velocity), Time.deltaTime);
        }

        transform.position += transform.forward * GetComponent<NavMeshAgent>().speed * Time.deltaTime;
        //animal.Move(GetComponent<Rigidbody>().velocity * Time.deltaTime);
        //GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().velocity * GetComponent<NavMeshAgent>().speed);

    }

    // TODO : rajouter les ponderations
    public Vector3 CalculatePrioritized() {

        // TODO : change
        Vector3 steeringForceAverage = new Vector3(0f, 0f, 1.0f);
        Vector3 force;

        if (wallAvoidanceOn) {
            force = WallAvoidance() * 10.0f;
            if (force.magnitude < maxForce - steeringForceAverage.magnitude) {
                // We add the value to the vector
                steeringForceAverage += force;
            } else {
                //add it to the steering force
                steeringForceAverage += (force.normalized * (maxForce - steeringForceAverage.magnitude));
                return steeringForceAverage;
            }
        }

        if (obstacleAvoidanceOn) {
            force = ObstaclesAvoidance() * 10.0f;
            if (force.magnitude < maxForce - steeringForceAverage.magnitude) {
                // We add the value to the vector
                steeringForceAverage += force;
            } else {
                //add it to the steering force
                steeringForceAverage += (force.normalized * (maxForce - steeringForceAverage.magnitude));
                return steeringForceAverage;
            }
        }

        if (wanderOn) {
            force = Wander() * 1.0f;
            if (force.magnitude < maxForce - steeringForceAverage.magnitude) {
                // We add the value to the vector
                steeringForceAverage += force;
            } else {
                //add it to the steering force
                steeringForceAverage += (force.normalized * (maxForce - steeringForceAverage.magnitude));
                return steeringForceAverage;
            }
        }

        return steeringForceAverage;
    }

    public Vector3 Wander() {
        Vector3 m_DesiredVelocity;

        timer += Time.deltaTime;
        if (timer > 0.7) {
            // get a new direction
            theta = theta + Random.Range(-0.2f, 0.2f) * 2 * Mathf.PI;
            vWanderTarget = transform.position + new Vector3(dWanderRadius * Mathf.Cos(theta), 0f,
                                dWanderRadius * Mathf.Sin(theta));
            vWanderTarget += transform.forward * WanderDistance;
            timer = 0;
        }
        m_DesiredVelocity = vWanderTarget - transform.position;
        return m_DesiredVelocity;
    }

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
                avoidanceForce += Vector3.Reflect(transform.forward * maxSpeed, hitInfo.normal) * (senseurs[r] - hitInfo.distance);
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

            //Vector3 forceLateral = (transform.position - hitInfo.point) / hitInfo.distance;
            //Vector3 forceFreinage = -transform.forward.normalized;
            if (hitInfo.transform != transform) {
                m_DesiredVelocity = hitInfo.point - hitInfo.collider.bounds.center; // hitInfo.normal;//forceLateral + forceFreinage;
                m_DesiredVelocity = new Vector3(m_DesiredVelocity.x, 0f, 0f) / hitInfo.distance;
            }
        }
        return m_DesiredVelocity;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);


        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.position + GetComponent<Rigidbody>().velocity.normalized * WanderDistance);
        Gizmos.DrawWireSphere(transform.position + transform.forward * WanderDistance, dWanderRadius);
        Gizmos.DrawWireSphere(vWanderTarget, 0.33f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5);
        Gizmos.DrawLine(transform.position + transform.forward * 1.5f, transform.position + Quaternion.Euler(0, 16, 0) * transform.forward * 5);
        Gizmos.DrawLine(transform.position + transform.forward * 1.5f, transform.position + Quaternion.Euler(0, -16, 0) * transform.forward * 5);


        Gizmos.color = Color.gray;
        Gizmos.DrawLine(transform.position, transform.position + desiredVelocity);
    }

}

[CustomEditor(typeof(SteeringBehavior))]
public class MyScriptEditor : Editor
{
    override public void OnInspectorGUI()
    {
        var myScript = target as SteeringBehavior;

        myScript.wallAvoidanceOn = EditorGUILayout.Toggle("Wall Avoidance", myScript.wallAvoidanceOn);
        myScript.obstacleAvoidanceOn = EditorGUILayout.Toggle("Obstacle Avoidance", myScript.obstacleAvoidanceOn);
        myScript.wanderOn = EditorGUILayout.Toggle("Wander", myScript.wanderOn);

        myScript.minDetectionBoxLength = EditorGUILayout.FloatField("Detection Lenght", myScript.minDetectionBoxLength);

    }
}