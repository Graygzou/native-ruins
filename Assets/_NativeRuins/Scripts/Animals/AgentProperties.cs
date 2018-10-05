using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentProperties : MonoBehaviour
{
    #region Serialize Fields
    // Attributes of the agent
    [Header("Basic properties")]
    [SerializeField]
    private float _maxHealth;
    [SerializeField]
    private float _damages;
    [SerializeField]
    private bool _isMean;
    [SerializeField]
    private float _tauntRange;
    [SerializeField]
    private SphereCollider _visionRange;
    [SerializeField]
    private SphereCollider _awarenessRange;
    [SerializeField]
    private AudioClip _sonCri;
    [SerializeField]
    private AudioClip _sonCombat;

    [Header("Steering behavior")]
    [SerializeField]
    private float _maxForce = 300f;
    [SerializeField]
    private float _maxSpeed = 300f;
    [SerializeField]
    private Transform front;

    [Header("Indicators")]
    public float hungryIndicator = 0.0f;
    public bool isDead;
    public bool isAlert;
    public bool playerTooClose;
    [SerializeField]
    private float currentSpeed = 22f;
    [SerializeField]
    private float currentHealth;

    #endregion

    #region
    [SerializeField]
    public float MaxHealth { get { return _maxHealth; } set {} }
    public float MaxForce { get { return _maxForce; } set {} }
    public float MaxSpeed { get { return _maxSpeed; } set {} }
    public float Damages { get { return _damages; } set { _damages = value;  } }
    public bool IsMean { get { return _isMean; } set {} }
    public float TauntRange { get { return _tauntRange; } set {} }

    #endregion

    private AudioSource audioSource;
    public static bool soundIsPlaying;

    void Awake() {
        isDead = false;
        // Get the child collider
        _visionRange = gameObject.GetComponentInChildren<SphereCollider>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start() {
        AudioSource[] sons = GetComponents<AudioSource>();
        //sonCri = sons[0];
        //sonCombat = sons[1];
        currentHealth = MaxHealth;

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
                audioSource.PlayOneShot(_sonCri);
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
        audioSource.PlayOneShot(_sonCri);

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
        audioSource.clip = _sonCombat;
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
