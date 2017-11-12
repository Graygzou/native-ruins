using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour {
	
	public Renderer renderer;
	public InventoryManager.Object_Type o_type;
	[SerializeField]private RectTransform o_object;


	private bool o_isPickable = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GetInputs ();
		
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag.Equals ("Player")) {
			o_isPickable = false;
			renderer.material.shader = Shader.Find ("Mobile/Diffuse");
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag.Equals ("Player")) {
			o_isPickable = true;
			renderer.material.shader = Shader.Find ("Outlined/Silhouetted Diffuse");
		}
	}

	private void GetInputs(){
		if (Input.GetKeyDown (KeyCode.E) && o_isPickable) {
			//m_bagSound.Play ();
			InventoryManager.AddObjectOfType(o_type);
			RectTransform clone = Instantiate(o_object) as RectTransform;
			clone.SetParent (GameObject.Find("InventoryManager/Canvas/Bag").transform, false);
			Destroy(this.gameObject);
		}
	}

}
