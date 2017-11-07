using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class AgentProperty : MonoBehaviour {

    // Attributes of the agent
    public int health;
    public int maxSpeed;
    public int mass;

    // To enable some transition
    public bool isMean;
    public float hungryIndicator = 0.0f;

    public bool isAlert;
    public bool playerTooClose;

    public SphereCollider visionRange;
    public SphereCollider awarenessRange;

    //public Animaux(float hea, float maxSpe, float mas) : this(hea, maxSpe, mas, 5) {
    //}

    public AgentProperty(float hea, float maxSpe, float mas) {
        
    }

    void Start() {
        //animal = GetComponent<NavMeshAgent>();
    }

    void OnTriggerEnter(Collider other) {

        // If the entering collider is the player...
        if (other.gameObject.tag == "Player") {
            if (!isAlert) {
                // The animal enter the "State" Alert
                isAlert = true;
            } else if (isAlert && !playerTooClose) {
                playerTooClose = true;
            }

        }
    }

    void OnTriggerExit(Collider other) {
        // If the exiting collider is the player...
        if (other.gameObject.tag == "Player") {
            if (isAlert && !playerTooClose) {
                // The animal quit the "State" Alert
                isAlert = false;
            } else if (isAlert && playerTooClose) {
                playerTooClose = false;
            }
        }
    }

    // POLISH
    //IEnumerator AccelerateWalk() {
    //    for (float i = 0f; i < 0.5f; i += 0.1f) {
    //        GetComponent<Animator>().SetFloat("Speed_f", i);
    //        yield return null;
    //    }
    //}

}

[CustomEditor(typeof(AgentProperty))]
public class EditorAgentProperty : Editor
{
    override public void OnInspectorGUI()
    {
        var myScript = target as AgentProperty;

        // For the behavior
        myScript.health = EditorGUILayout.IntField("Health:", myScript.health);
        myScript.maxSpeed = EditorGUILayout.IntField("Max Speed:", myScript.maxSpeed);
        myScript.mass = EditorGUILayout.IntField("Mass:", myScript.mass);
        myScript.isMean = EditorGUILayout.Toggle("IsMean:", myScript.isMean);
        myScript.hungryIndicator = EditorGUILayout.FloatField("Hunger:", myScript.hungryIndicator);

        myScript.visionRange = EditorGUILayout.ObjectField("VisionRange:", myScript.visionRange, typeof(SphereCollider), true) as SphereCollider;
        myScript.awarenessRange = EditorGUILayout.ObjectField("AwarenessRange:", myScript.awarenessRange, typeof(SphereCollider), true) as SphereCollider;
    }
}
