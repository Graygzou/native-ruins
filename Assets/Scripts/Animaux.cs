using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animaux : MonoBehaviour {

    //private NavMeshAgent animal;     // See if useless
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
        //target = gameObject.transform.forward * 2;   // The animal follow his current heading.
    }

    void Start() {
        steering = new SteeringBehavior(this);
    }

    // Update is called once per frame
    void Update() {
        steering.UpdateBehavior(this);
        //Debug.Log(gameObject.transform.forward);
        //print(transform.position.x);
        //Wander();
    }

    //// Use to make the animal behave like he has no particular goals
    //public void Wander () {

    //    //this behavior is dependent on the update rate, so this line must
    //    //be included when using time independent framerate.
    //    //double JitterThisTimeSlice = m_dWanderJitter * m_pVehicle->TimeElapsed();

    //    //first, add a small random vector to the target's position
    //    Vector2 vectorChange = Random.insideUnitCircle;
    //    target += new Vector3(vectorChange.x, 0f, vectorChange.y);

    //    //reproject this new vector back on to a unit circle
    //    target.Normalize();

    //    //increase the length of the vector to the same as the radius
    //    //of the wander circle
    //    target *= 5;

    //    //move the target into a position WanderDist in front of the agent
    //    //target = m_vWanderTarget + Vector2D(m_dWanderDistance, 0);

    //    //and steer towards it
    //    animal.Move(target);
    //}

    // Use this for initialization

}
