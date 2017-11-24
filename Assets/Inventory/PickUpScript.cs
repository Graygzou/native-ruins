using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour {
	
	public Renderer renderer;
	public ObjectsType o_type;
    [SerializeField]private RectTransform o_object;

	private bool o_isPickable = false;
    private DialogueTrigger dialogue;

    // Use this for initialization
    void Start () {
        dialogue = GameObject.Find("Affichages/Dialogues/DialogueTrigger").GetComponent<DialogueTrigger>();
    }
	
	// Update is called once per frame
	void Update () {
		GetInputs ();
		
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
        AudioSource son;

        if (Input.GetKeyDown (KeyCode.E) && o_isPickable) {
            if (o_type.Equals(ObjectsType.Bow) && GameObject.Find("Terrain/Bow/Chest_bow/Particles_Fireflies").activeSelf)
            {
                son = this.GetComponent<AudioSource>();
                son.Play();
                dialogue.TriggerDialogueArc();     
                GameObject.Find("Terrain/Bow/Chest_bow/Particles_Fireflies").SetActive(false);
            }
            if (o_type.Equals(ObjectsType.Rope) && GameObject.Find("EnigmeCorde/Corde/Particles_Fireflies").activeSelf)
            {
                son = this.GetComponent<AudioSource>();
                son.Play();
                dialogue.TriggerDialogueCorde();
                GameObject.Find("EnigmeCorde/Corde/Particles_Fireflies").SetActive(false);
            }
            if (o_type.Equals(ObjectsType.Sail) && GameObject.Find("EnigmeVoile/Voile/Particles_Fireflies").activeSelf)
            {
                son = this.GetComponent<AudioSource>();
                son.Play();
                dialogue.TriggerDialogueVoile();
                GameObject.Find("EnigmeVoile/Voile/Particles_Fireflies").SetActive(false);
            }
            InventoryManager.AddObjectOfType(o_type);
			InventoryManager.an_object_is_pickable = false;
			RectTransform clone = Instantiate(o_object) as RectTransform;
			clone.SetParent (GameObject.Find("InventoryManager/Canvas/Bag").transform, false);
			GameObject.Find ("InventoryManager/Canvas/ButtonRamasser").SetActive(false);
			Destroy(this.gameObject);
		}
	}

}
