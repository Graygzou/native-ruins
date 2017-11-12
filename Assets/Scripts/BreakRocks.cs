using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakRocks : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject cubes = transform.parent.GetChild(1).gameObject;
        cubes.SetActive(false);
        GameObject fog = transform.parent.GetChild(2).gameObject;
        fog.GetComponent<ParticleSystem>().Stop();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && 
            GameObject.FindWithTag("Player").GetComponent<Forms>().currentForm == (int)Forms.forms.bear &&
            Input.GetKey(KeyCode.LeftShift)) {
            transform.parent.parent.GetChild(0).gameObject.SetActive(false); //desactive le gros collider
            GameObject cubes = transform.parent.GetChild(1).gameObject;
            StartCoroutine(SmokeAnimation());
            cubes.SetActive(true);
            // For each child, call the break method to destroy the cube
            foreach (Transform child in cubes.transform) {
                child.GetComponent<PhysicsController_DL0>().animationBreak(other);
            }
        }
    }

    private IEnumerator SmokeAnimation()
    {
        GameObject fog = transform.parent.GetChild(2).gameObject;
        fog.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(seconds: 0.2f);
        fog.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        this.gameObject.SetActive(false);
    }
}
