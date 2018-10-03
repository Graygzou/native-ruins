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
                InventoryManager.Instance.SetStatePickupButton(false);
            }
		}
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.tag.Equals ("Player") && !InventoryManager.Instance.bag_open) {
			if (!InventoryManager.an_object_is_pickable) {
				InventoryManager.an_object_is_pickable = true;
				o_isPickable = true;
				renderer.material.shader = Shader.Find ("Outlined/Silhouetted Diffuse");
                InventoryManager.Instance.SetStatePickupButton(true);
            }
		}
	}

	private void GetInputs(){
		if (Input.GetKeyDown (KeyCode.E) && o_isPickable && isActive) {
			InventoryManager.Instance.AddObjectOfType(o_type, o_object);
			InventoryManager.an_object_is_pickable = false;
            InventoryManager.Instance.SetStatePickupButton(false);
            this.gameObject.GetComponent<MeshRenderer>().enabled=false;
			this.gameObject.GetComponent<SphereCollider>().enabled=false;
			isActive = false;
		}
	}

    IEnumerator WaitRespawn() {
        if (isActive)
        {
            GetInputs();
            yield return 0;
        }
        else
        {
            yield return new WaitForSeconds(900);
            if (!isActive)
            {
                isActive = true;
                this.gameObject.GetComponent<MeshRenderer>().enabled = true;
                this.gameObject.GetComponent<SphereCollider>().enabled = true;
                InventoryManager.an_object_is_pickable = false;
                o_isPickable = false;
                renderer.material.shader = Shader.Find("Mobile/Diffuse");
                InventoryManager.Instance.SetStatePickupButton(false);
            }
        }
    }
}


