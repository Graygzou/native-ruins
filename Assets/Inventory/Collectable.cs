using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Collectable : MonoBehaviour {



	public Renderer renderer;
	public ObjectsType o_type;
	[SerializeField]private RectTransform o_object;



	private bool o_isPickable = false;
	private bool isActive=true;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		StartCoroutine (WaitRespawn ());
		//GetInputs ();
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag.Equals ("Player")) {
			if (InventoryManager.an_object_is_pickable && o_isPickable) {
				InventoryManager.an_object_is_pickable = false;
				o_isPickable = false;
				renderer.material.shader = Shader.Find ("Mobile/Diffuse");
				GameObject.Find ("InventoryManager/Canvas/ButtonRamasser").SetActive(false);
			}
		}
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.tag.Equals ("Player") && !InventoryManager.bag_open) {
			if (!InventoryManager.an_object_is_pickable) {
				InventoryManager.an_object_is_pickable = true;
				o_isPickable = true;
				renderer.material.shader = Shader.Find ("Outlined/Silhouetted Diffuse");
				GameObject.Find ("InventoryManager/Canvas/ButtonRamasser").SetActive(true);
			}
		}
	}

	private void GetInputs(){
		if (Input.GetKeyDown (KeyCode.E) && o_isPickable && isActive) {
			InventoryManager.AddObjectOfType(o_type);
			InventoryManager.an_object_is_pickable = false;
			RectTransform clone = Instantiate(o_object) as RectTransform;
			clone.SetParent (GameObject.Find("InventoryManager/Canvas/Bag").transform, false);
			GameObject.Find ("InventoryManager/Canvas/ButtonRamasser").SetActive(false);
			this.gameObject.GetComponent<MeshRenderer>().enabled=false;
			this.gameObject.GetComponent<SphereCollider>().enabled=false;
			isActive = false;
		}
	}

	IEnumerator WaitRespawn(){
		if (isActive) {
			GetInputs ();
			yield return 0;
		} else {
			yield return new WaitForSeconds (5);
			isActive = true;
			this.gameObject.GetComponent<MeshRenderer>().enabled=true;
			this.gameObject.GetComponent<SphereCollider>().enabled=true;
		}
	}
}


