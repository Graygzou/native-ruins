using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentProperties : MonoBehaviour
{
    // Attributes of the agent
    public float maxHealth;
    public float maxForce = 200f;
    public float maxSpeed;

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
    [SerializeField]
    private float currentSpeed = 22f;
    private Transform front;

    public AudioClip sonCri;
    public AudioClip sonCombat;

    private AudioSource audioSource;

    public static bool soundIsPlaying;

    void Awake() {
        isDead = false;
        // Get the child collider
        visionRange = gameObject.GetComponentInChildren<SphereCollider>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start() {
        AgentProperties.soundIsPlaying = false;
        AudioSource[] sons = GetComponents<AudioSource>();
        //sonCri = sons[0];
        //sonCombat = sons[1];
        currentHealth = maxHealth;

        front = transform.GetChild(transform.childCount-1).transform;
        transform.GetComponentInChildren<ParticleSystem>().Stop();
    }

    void OnTriggerEnter(Collider other)
    {
        // If the entering collider is the player...
        if (other.gameObject.tag == "Player" && !isDead)
        {
            if (!isAlert) {
                // The animal enter the "State" Alert
                isAlert = true;
            } else if (isAlert && !playerTooClose) {
                playerTooClose = true;
                audioSource.PlayOneShot(sonCri);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If the exiting collider is the player...
        if (other.gameObject.tag == "Player" && !isDead)
        {
            if (isAlert && playerTooClose) {
                playerTooClose = false;
            } else if (isAlert && !playerTooClose) {
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

    public Transform getFront() {
        return front;
    }

    public void takeDamages(float amount) {

        // If the enemy is dead...
        if (isDead)
            // ... no need to take damage so exit the function.
            return;

        // Play the hurt sound effect.
        audioSource.PlayOneShot(sonCri);

        // Reduce the current health by the amount of damage sustained.
        currentHealth -= amount;

        // If the current health is less than or equal to zero...
        if (currentHealth <= 0) {
            Debug.Log("Dead");
            // the enemy is dead.
            isDead = true;
        }
    }

    public void PlayFightSong()
    {
        audioSource.clip = sonCombat;
        audioSource.Play();
    }

    public void StopFightSong()
    {
        audioSource.Stop();
    }

    public void MakeAgentDisappear(GameObject o) {
        StartCoroutine(SmokeAnimation(o));
    }

    void DropItems() {
        // Active food
        for (int i = 0; i < transform.Find("Items").childCount; i++) {
            transform.Find("Items").GetChild(i).gameObject.SetActive(true);
        }
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
