using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using System;

public class SteeringBehavior : MonoBehaviour {

    // Variables for the wander
    public float WanderDistance { get; set; }
    public float dWanderRadius { get; set; }
    public Vector3 vWanderTarget;

    public Vector3 target_p;
    public Vector3 desiredVelocity { get; set; }

    public float minDetectionBoxLength = 7.0f;

    private float timer = 0;

    public bool wallAvoidanceOn;
    public bool obstacleAvoidanceOn;
    public bool wanderOn;
    public bool fleeOn;
    public bool seekOn;

    float theta;

    // Variables for the obstacles avoidance
    public float boundingSphereRadius;
    public float obstacleMaxDistance;
    public Dictionary<Ray, float> senseurs;

    private AgentProperties properties;

    // Maybe useless
    //NavMeshAgent animal;

    void Start() {

        // Get the animal properties
        properties = GetComponent<AgentProperties>();
        //animal = GetComponent<NavMeshAgent>();

        //maxSpeed = GetComponent<NavMeshAgent>().speed;

        // set Items
        dWanderRadius = 3.0f;
        WanderDistance = 7.0f;

        boundingSphereRadius = 1.0f;
        obstacleMaxDistance = 10.0f;

        senseurs = new Dictionary<Ray, float>();

        // Set velocity
        Vector3 velocity = transform.forward;
        velocity.y = GetComponent<Rigidbody>().velocity.y;
        GetComponent<Rigidbody>().velocity = velocity;

        // Create first random point
        theta = UnityEngine.Random.value * 2 * Mathf.PI;
        vWanderTarget = new Vector3(dWanderRadius * Mathf.Cos(theta), 0f,
                                    dWanderRadius * Mathf.Sin(theta));
        vWanderTarget += transform.position + transform.forward * WanderDistance;

        //Debug.Log("first: " + agent.transform.position + " et " + m_vWanderTarget2);
    }

    // Update is called once per frame
    //void Update() {
    //    UpdateBehavior();
    //}

    /* -----------------------------------------
     * Update the steering behavior of the agent
     * ----------------------------------------- */
    public void UpdateBehavior() {
        //StartCoroutine(UpdateSteer());
        Vector3 m_vSteeringForce = CalculatePrioritized();

        m_vSteeringForce.y = 0.0f;
        //make sure vehicle does not exceed maximum velocity
        //Vector3.ClampMagnitude(m_vSteeringForce, properties.maxSpeed);

        GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(m_vSteeringForce, properties.maxSpeed);

        //update the heading if the vehicle has a non zero velocity
        if (GetComponent<Rigidbody>().velocity.sqrMagnitude > 0.000001)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(GetComponent<Rigidbody>().velocity), Time.deltaTime);
        }

        //transform.Translate(GetComponent<Rigidbody>().velocity * Time.deltaTime);
        //transform.position = Vector3.Lerp(transform.position, transform.position + GetComponent<Rigidbody>().velocity, Time.deltaTime);

        transform.position = Vector3.Lerp(transform.position, transform.position + GetComponent<Rigidbody>().velocity, Time.deltaTime);

        // -- A TESTER --

        // Normalise the movement vector and make it proportional to the speed per second.
        // movement = movement.normalized * speed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        // playerRigidbody.MovePosition(transform.position + movement);

        // -- Fin a tester ---


        //animal.Move(GetComponent<Rigidbody>().velocity * Time.deltaTime);
        //GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().velocity * GetComponent<NavMeshAgent>().speed);

    }

    public Vector3 CalculatePrioritized() {
        Vector3 force;
        Vector3 steeringForceAverage = Vector3.zero;
        if (wanderOn) {
            steeringForceAverage = new Vector3(0.0f, 0.0f, 1.0f); ;
        }

        if (wallAvoidanceOn) {
            force = WallAvoidance() * 5.0f;
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
            force = ObstaclesAvoidance() * 5.0f;
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
            force = Wander() * 1.0f;
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
            force = Flee() * 1.0f;
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
            force = Seek() * 1.0f;
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

    public Vector3 Wander() {
        Vector3 m_DesiredVelocity;

        timer += Time.deltaTime;
        if (timer > 0.7) {
            // get a new direction
            theta = theta + UnityEngine.Random.Range(-0.2f, 0.2f) * 2 * Mathf.PI;
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
                avoidanceForce += Vector3.Reflect(transform.forward * properties.maxSpeed, hitInfo.normal) * (senseurs[r] - hitInfo.distance);
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
            if (hitInfo.transform != transform) {
                m_DesiredVelocity = hitInfo.point - hitInfo.collider.bounds.center;
                m_DesiredVelocity = new Vector3(m_DesiredVelocity.x, 0f, 0f) / hitInfo.distance;
            }
        }
        return m_DesiredVelocity;
    }

    public Vector3 Seek() {
        Vector3 m_DesiredVelocity = Vector3.zero;
        //NavMeshAgent agent = GetComponent<NavMeshAgent>();
        //if (agent != null) {
        //    agent.SetDestination(target_p.position);
        //} else {
            // Redefinir A* ici ?
        m_DesiredVelocity = (target_p - transform.position) * properties.maxSpeed;
        //}
        return m_DesiredVelocity;
    }

    public Vector3 Flee() {
        Vector3 m_DesiredVelocity = Vector3.zero;
        //Vector3 m_DesiredVelocity = (transform.position - target_p.position).normalized * maxSpeed;
        m_DesiredVelocity = (transform.position - target_p) * properties.maxSpeed;
        return m_DesiredVelocity; // - GetComponent<Rigidbody>().velocity;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);


        Gizmos.color = Color.black;
        //Gizmos.DrawLine(transform.position, transform.forward + GetComponent<Rigidbody>().velocity.normalized * WanderDistance);
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
    SerializedProperty m_wallAvoidanceOn;
    SerializedProperty m_obstacleAvoidanceOn;
    SerializedProperty m_wanderOn;
    SerializedProperty m_fleeOn;
    SerializedProperty m_seekOn;
    SerializedProperty m_boundingSphereRadius;
    SerializedProperty m_obstacleMaxDistance;
    SerializedProperty m_target_p;

    void OnEnable()
    {
        // Fetch the objects from the GameObject script to display in the inspector
        m_wallAvoidanceOn = serializedObject.FindProperty("wallAvoidanceOn");
        m_obstacleAvoidanceOn = serializedObject.FindProperty("obstacleAvoidanceOn");
        m_wanderOn = serializedObject.FindProperty("wanderOn");
        m_fleeOn = serializedObject.FindProperty("fleeOn");
        m_seekOn = serializedObject.FindProperty("seekOn");
        m_boundingSphereRadius = serializedObject.FindProperty("boundingSphereRadius");
        m_obstacleMaxDistance = serializedObject.FindProperty("obstacleMaxDistance");
        m_target_p = serializedObject.FindProperty("target_p");
    }

    override public void OnInspectorGUI()
    {
        //The variables and GameObject from the MyGameObject script are displayed in the Inspector with appropriate labels
        EditorGUILayout.PropertyField(m_wallAvoidanceOn, new GUIContent("Wall Avoidance:"));
        EditorGUILayout.PropertyField(m_obstacleAvoidanceOn, new GUIContent("Obstacle Avoidance:"));
        EditorGUILayout.PropertyField(m_wanderOn, new GUIContent("Wander:"));
        EditorGUILayout.PropertyField(m_fleeOn, new GUIContent("Flee:"));
        EditorGUILayout.PropertyField(m_seekOn, new GUIContent("Seek:"));
        EditorGUILayout.PropertyField(m_target_p, new GUIContent("Target:"));
        // TODO le refaire
        //using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(m_fleeOn || m_seekOn)))
        //{
        //    if (group.visible == true)
        //    {
        //        EditorGUI.indentLevel++;
        //        EditorGUILayout.PropertyField(m_target_p, new GUIContent("Target:"));
        //        EditorGUI.indentLevel--;
        //    }
        //}

        EditorGUILayout.PropertyField(m_boundingSphereRadius, new GUIContent("Detection Lenght:"));
        EditorGUILayout.PropertyField(m_obstacleMaxDistance, new GUIContent("Wander Radius:"));

        // Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }
}