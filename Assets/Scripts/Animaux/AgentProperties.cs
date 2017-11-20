using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

[System.Serializable]
public class AgentProperties : MonoBehaviour
{

    // Attributes of the agent
    public float maxHealth;
    public float maxForce;
    public int maxSpeed;

    public int mass;
    public float damages;
    public float hungryIndicator = 0.0f;
    public bool isMean;
    public bool isDead;

    public float tauntRange;

    public SphereCollider visionRange;
    public SphereCollider awarenessRange;
    public bool isAlert;
    public bool playerTooClose;

    private float currentHealth;
    private float currentSpeed;

    [SerializeField]
    private float attackRangeVsPlayer;
    [SerializeField]
    private float attackRangeVsBear;
    [SerializeField]
    private float attackRangeVsWolf;

    private AudioSource sonCri;

    public float[] attackRange;

    void Awake()
    {
        isDead = false;
        maxForce = 200f;
        currentSpeed = 22f;
    }

    void Start()
    {
        sonCri = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        attackRange = new float[3];
        attackRange[0] = attackRangeVsPlayer;
        attackRange[1] = attackRangeVsBear;
        attackRange[2] = attackRangeVsWolf;

        transform.GetComponentInChildren<ParticleSystem>().Stop();
    }

    void OnTriggerEnter(Collider other)
    {

        // If the entering collider is the player...
        if (other.gameObject.tag == "Player")
        {
            if (!isAlert)
            {
                sonCri.Play();
                // The animal enter the "State" Alert
                isAlert = true;
            }
            else if (isAlert && !playerTooClose)
            {
                playerTooClose = true;
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        // If the exiting collider is the player...
        if (other.gameObject.tag == "Player")
        {
            if (isAlert && playerTooClose)
            {
                playerTooClose = false;
            }
            else if (isAlert && !playerTooClose)
            {
                // The animal quit the "State" Alert
                isAlert = false;
            }
        }
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public float getCurrentSpeed()
    {
        return currentSpeed;
    }

    public void setSpeed(float speed)
    {
        currentSpeed = speed;
    }

    public void takeDamages(float amount)
    {
        sonCri.Play();

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
        if (currentHealth <= 0)
        {
            Debug.Log("Dead");
            // the enemy is dead.
            isDead = true;
        }
    }

    public void MakeAgentDisappear(GameObject o)
    {
        StartCoroutine(SmokeAnimation(o));
    }

    void DropItems()
    {
        transform.GetChild(4).gameObject.SetActive(true);
    }

    private IEnumerator SmokeAnimation(GameObject o)
    {
        yield return new WaitForSeconds(seconds: 1.0f);
        transform.GetComponentInChildren<ParticleSystem>().Play();
        yield return new WaitForSeconds(seconds: 1.0f);
        transform.GetComponentInChildren<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        // After 2 seconds destory the enemy.
        transform.GetComponentInChildren<SkinnedMeshRenderer>().gameObject.SetActive(false);
        DropItems();
    }

    void OnDrawGizmos() {
        //Transform nose = transform.GetChild(5).transform;

        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(nose.position, nose.position + nose.forward * 0.5f);
    }
}

[CustomEditor(typeof(AgentProperties))]
[CanEditMultipleObjects]
public class EditorAgentProperty : Editor
{
    SerializedProperty m_isMean, m_maxHealth, m_maxSpeed, m_mass;
    SerializedProperty m_damages, m_hungryIndicator;
    SerializedProperty m_visionRange, m_awarenessRange;
    SerializedProperty m_attackRangePlayer, m_attackRangeBear, m_attackRangeWolf, m_tauntRange;

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
        m_attackRangePlayer = serializedObject.FindProperty("attackRangeVsPlayer");
        m_attackRangeBear = serializedObject.FindProperty("attackRangeVsBear");
        m_attackRangeWolf = serializedObject.FindProperty("attackRangeVsWolf");
        m_tauntRange = serializedObject.FindProperty("tauntRange");
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
        EditorGUILayout.PropertyField(m_attackRangePlayer, new GUIContent("Attack Range Player:"));
        EditorGUILayout.PropertyField(m_attackRangeBear, new GUIContent("Attack Range Bear:"));
        EditorGUILayout.PropertyField(m_attackRangeWolf, new GUIContent("Attack Range Wolf:"));
        EditorGUILayout.PropertyField(m_tauntRange, new GUIContent("Taunt Range:"));

        // Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }
}
