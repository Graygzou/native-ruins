using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fights : MonoBehaviour {

    private float distance;
    private GameObject forms;
	// Use this for initialization
	void Start () {
		forms = GameObject.Find("Forms");
    }
	
	// Update is called once per frame
	void Update () {
		if(forms.GetComponent<Forms>().currentForm == (int)Forms.forms.bear && Input.GetMouseButton(0))
        {
            Debug.Log("coucou");
            //GameObject.FindWithTag("Player").PlayAnimation();
            RaycastHit hit;
            distance = 1f; //distance de l'animal pour pouvoir lui infliger des degats
            Ray Judy = new Ray(GameObject.FindWithTag("Player").transform.position, Vector3.forward);
            if(Physics.Raycast(Judy,out hit,distance))
            {
               
                    GameObject.FindWithTag("LifeBar").GetComponent<Scrollbar>().size -= 0.1f;
                    //Inflige degat a l'animal
               
            }
        }
	}
}
