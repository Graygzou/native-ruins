using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour {
	
	public Renderer renderer;
	public ObjectsType o_type;
    [SerializeField]
    private RectTransform o_object;

    private bool o_isPickable;

    private GameObject bag;

    private void Awake()
    {
        bag = GameObject.FindWithTag("BagUI");
    }

    // Use this for initialization
    void Start () {
        o_isPickable = false;
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
        AudioSource son;

        PlayerAknowledge brain = GameObject.Find("Player").GetComponent<PlayerAknowledge>();
        if (Input.GetKeyDown (KeyCode.E) && o_isPickable) {
            if (o_type.Equals(ObjectsType.Bow) && !brain.HasDiscoveredBow)
            {
                brain.HasDiscoveredBow = true;
                son = this.GetComponentInParent<AudioSource>();
                son.Play();
                DialogueTrigger.TriggerDialogueArc(null);     
                GameObject.Find("Terrain/Bow/Chest_bow/Particles_Fireflies").SetActive(false);
                GameObject playerRoot = GameObject.Find("Player");
                FormsController.Instance.Transformation(FormsController.TransformationType.Human);
                GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ActionsNew>().Stay(100f);
            }
            if (o_type.Equals(ObjectsType.Rope) && !brain.HasDiscoveredRope)
            {
                brain.HasDiscoveredRope = true;
                son = this.GetComponentInParent<AudioSource>();
                son.Play();
                DialogueTrigger.TriggerDialogueCorde(null);
                GameObject.Find("EnigmeCorde/Corde/Particles_Fireflies").SetActive(false);
                GameObject playerRoot = GameObject.Find("Player");
                FormsController.Instance.Transformation(FormsController.TransformationType.Human);
                GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ActionsNew>().Stay(100f);
            }
            if (o_type.Equals(ObjectsType.Sail) && !brain.HasDiscoveredSail)
            {
                brain.HasDiscoveredSail = true;
                son = this.GetComponentInParent<AudioSource>();
                son.Play();
                DialogueTrigger.TriggerDialogueVoile(null);
                GameObject.Find("EnigmeVoile/Voile/Particles_Fireflies").SetActive(false);
                GameObject playerRoot = GameObject.Find("Player");
                FormsController.Instance.Transformation(FormsController.TransformationType.Human);
                GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ActionsNew>().Stay(100f);
            }
            InventoryManager.Instance.AddObjectOfType(o_type, o_object);
			InventoryManager.an_object_is_pickable = false;
            InventoryManager.Instance.SetStatePickupButton(false);
            Destroy(this.gameObject);
		}
	}

}
