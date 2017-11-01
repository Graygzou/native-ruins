using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SteeringBehavior {

    private GameObject agent;

    private float m_dWanderRadius;
    private float distanceProjection;
    private Vector3 m_vWanderTarget; // Can be player or another position.

    NavMeshAgent animal;

    public SteeringBehavior(GameObject ag) {

        // get the real agent using the steering
        agent = ag;

        // Get the animal
        animal = agent.GetComponent<NavMeshAgent>();

        //stuff for the wander behavior
        float theta = Random.value * 2 * Mathf.PI;
        m_dWanderRadius = 5.0f;
        distanceProjection = 5.0f;
        //create a vector to a target position on the wander circle
        m_vWanderTarget = new Vector3(m_dWanderRadius * Mathf.Cos(theta), 0f,
                                    m_dWanderRadius * Mathf.Sin(theta));
        // Translation
        m_vWanderTarget += agent.transform.position + new Vector3(0, 0, distanceProjection);

        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = m_vWanderTarget;
        //cube.GetComponent<Renderer>().material.color = new Color(0, 255, 52);
        Debug.Log("first: " + agent.transform.position + " et " + m_vWanderTarget);
    }

    public void UpdateBehavior() {
        //StartCoroutine(UpdateSteer());
        UpdateSteer();
    }

    //IEnumerator UpdateSteer()
    //{
    //    // TODO watch tuto
    //    // Tire au hasard un nombre entre 0 et 1
    //    float val = Random.value;
    //    if (val > 0.1)
    //    {
    //        Wander();
    //        yield return new WaitForSeconds(.1f);
    //    }
    //    else
    //    {
    //        // choisie une nouvelle direction
    //        float theta = Random.value * 2 * Mathf.PI;
    //        m_vWanderTarget = new Vector3(m_dWanderRadius * Mathf.Cos(theta), 0f,
    //                                    m_dWanderRadius * Mathf.Sin(theta));
    //        m_vWanderTarget += agent.transform.position + new Vector3(0, 0, distanceProjection);

    //        // Marqué une pause.
    //        yield return new WaitForSeconds(3f);
    //    }

    //    //while (Vector3.Distance(transform.position, target) > 0.05f)
    //    //{
    //    //    transform.position = Vector3.Lerp(transform.position, target, smoothing * Time.deltaTime);

    //    //    yield return null;
    //    //}
    //}

    public void UpdateSteer() {
        // TODO watch tuto
        // Tire au hasard un nombre entre 0 et 1
        //float val = Random.value;
        //if (val > 0.005)
        //{
            Wander();
        //}
        //else
        //{
        //    // choisie une nouvelle direction
        //    float theta = Random.value * 2 * Mathf.PI;
        //    m_vWanderTarget = new Vector3(m_dWanderRadius * Mathf.Cos(theta), 0f,
        //                                m_dWanderRadius * Mathf.Sin(theta));
        //    m_vWanderTarget += agent.transform.position + new Vector3(0, 0, -12f);

        //    // Marqué une pause.
        //}

        //while (Vector3.Distance(transform.position, target) > 0.05f)
        //{
        //    transform.position = Vector3.Lerp(transform.position, target, smoothing * Time.deltaTime);

        //    yield return null;
        //}
    }



    public void Wander() {
        Vector3 test = new Vector3(Random.Range(-20.0f, 20.0f), 0f, 0f);
        Vector3 wanderTarget = m_vWanderTarget + test;

        Vector3 centerWanderCircle = agent.transform.position + new Vector3(0, 0, 10);

        Vector3 direction = centerWanderCircle - wanderTarget;

        //reproject this new vector back on to a unit circle
        direction.Normalize();

        // Multiplie par le rayon du cercle wander
        direction *= m_dWanderRadius;
        // translation
        ////Debug.Log(target);
        m_vWanderTarget = agent.transform.position + new Vector3(0, 0, distanceProjection) + direction;

        Debug.Log(agent.transform.position + " et " + m_vWanderTarget);
        animal.SetDestination(m_vWanderTarget);

    }

    public void Wander2() {
        //Vector3 desiredVel = (target - unit.transform.position).normalized * 25 * Time.deltaTime;

        //// then it depends if you are rotating it or not, if rotating:
        //transform.rotation = Quaternio.LookRotation(desiredVel);
        //transform.Translate(transform.forward * Time.deltaTime);
    }

}
