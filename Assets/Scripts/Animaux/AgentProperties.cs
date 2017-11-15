using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

[System.Serializable]
public class AgentProperties : MonoBehaviour {

    // Attributes of the agent
    public float maxHealth;
    public int maxSpeed;
    public int mass;

    // To enable some transition
    [SerializeField]
    public bool isMean;
    public float hungryIndicator = 0.0f;
    public float damages;

    public bool isAlert;
    public bool playerTooClose;

    private float currentHealth;
    public bool isDead;
    public float maxForce;

    public SphereCollider visionRange;
    public SphereCollider awarenessRange;

    //public Animaux(float hea, float maxSpe, float mas) : this(hea, maxSpe, mas, 5) {
    //}

    //public AgentProperty(float hea, float maxSpe, float mas) {

    //}

    void Awake() {
        maxForce = 200f;
    }

    void Start() {
        currentHealth = maxHealth;
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

    public float getCurrentHealth() {
        return currentHealth;
    }

    public void takeDamages(float amount) {

        Debug.Log("Outch !");

        // If the enemy is dead...
        if (isDead)
            // ... no need to take damage so exit the function.
            return;

        // Play the hurt sound effect.
        //enemyAudio.Play();

        // Reduce the current health by the amount of damage sustained.
        currentHealth -= amount;

        // Set the position of the particle system to where the hit was sustained.
        //hitParticles.transform.position = hitPoint;

        // And play the particles.
        //hitParticles.Play();

        // If the current health is less than or equal to zero...
        if (currentHealth <= 0) {
            Debug.Log("Dead");
            // the enemy is dead.
            isDead = true;
        }
    }

}

[CustomEditor(typeof(AgentProperties))]
[CanEditMultipleObjects]
public class EditorAgentProperty : Editor
{
    SerializedProperty m_isMean;
    SerializedProperty m_maxHealth;
    SerializedProperty m_maxSpeed;
    SerializedProperty m_mass;
    SerializedProperty m_damages;
    SerializedProperty m_hungryIndicator;
    SerializedProperty m_visionRange;
    SerializedProperty m_awarenessRange;

    void OnEnable() {
        // Fetch the objects from the GameObject script to display in the inspector
        m_maxHealth = serializedObject.FindProperty("maxHealth");
        m_maxSpeed = serializedObject.FindProperty("maxSpeed");
        m_mass = serializedObject.FindProperty("mass");
        m_isMean = serializedObject.FindProperty("isMean");
        m_damages = serializedObject.FindProperty("damages");
        m_hungryIndicator = serializedObject.FindProperty("hungryIndicator");
        m_visionRange = serializedObject.FindProperty("isAlert");
        m_awarenessRange = serializedObject.FindProperty("playerTooClose");
    }

    override public void OnInspectorGUI() {
        //The variables and GameObject from the MyGameObject script are displayed in the Inspector with appropriate labels
        EditorGUILayout.PropertyField(m_maxHealth, new GUIContent("Max Health:"));
        EditorGUILayout.PropertyField(m_maxSpeed, new GUIContent("Max Speed:"));
        EditorGUILayout.PropertyField(m_mass, new GUIContent("Mass:"));
        EditorGUILayout.PropertyField(m_isMean, new GUIContent("IsMean:"));
        EditorGUILayout.PropertyField(m_damages, new GUIContent("Damages:"));
        EditorGUILayout.PropertyField(m_hungryIndicator, new GUIContent("Hunger:"));

        EditorGUILayout.PropertyField(m_visionRange, new GUIContent("VisionRange:"));
        EditorGUILayout.PropertyField(m_awarenessRange, new GUIContent("AwarenessRange:"));

        // Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }
}
