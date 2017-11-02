using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animaux : MonoBehaviour {

    public NavMeshAgent animal;     // See if useless

    [HideInInspector]
    public SteeringBehavior steering;

    // Attributes of animals
    public float health { get; set; }
    public float maxSpeed { get; set; }
    public float mass { get; set; }

    //public Animaux(float hea, float maxSpe, float mas) : this(hea, maxSpe, mas, 5) {
    //}

    public Animaux(float hea, float maxSpe, float mas) {
        health = hea;
        maxSpeed = maxSpe;
        mass = mas;
    }

    void Start() {
        steering = new SteeringBehavior(this);
        animal = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {
        steering.UpdateBehavior(this);
    }

}
