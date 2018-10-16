using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour {
	
    [SerializeField]
	private Renderer renderer;
    [SerializeField]
    private ObjectsType o_type;
    [SerializeField]
    private RectTransform o_object;

    [SerializeField]
    private bool o_isPickable = true;
    public bool IsPickable { get { return o_isPickable; } }
    private GameObject bag;
    private PlayerAknowledge brain;

    private void Awake()
    {
        bag = GameObject.FindWithTag("BagUI");
        brain = GameObject.Find("Player").GetComponent<PlayerAknowledge>();
    }

    /*
	void OnTriggerExit(Collider other)
    {
		if (other.gameObject.tag.Equals ("Player")) {
			if (InventoryManager.an_object_is_pickable && IsPickable) {
				InventoryManager.an_object_is_pickable = false;
				IsPickable = false;
				renderer.material.shader = Shader.Find ("Mobile/Diffuse");
                InventoryManager.Instance.SetStatePickupButton(false);
			}
		}
	}

	void OnTriggerStay(Collider other)
    {
		if (other.gameObject.tag.Equals ("Player") && !InventoryManager.Instance.bag_open) {
			if (!InventoryManager.an_object_is_pickable) {
				InventoryManager.an_object_is_pickable = true;
				IsPickable = true;
				renderer.material.shader = Shader.Find ("Outlined/Silhouetted Diffuse");
                InventoryManager.Instance.SetStatePickupButton(true);
			}
		}
	}*/

	public void Interact()
    {
        if (IsPickable) {
            if (o_type.Equals(ObjectsType.Bow) && !brain.HasDiscoveredBow)
            {
                brain.HasDiscoveredBow = true;
                DialogueTrigger.TriggerDialogueArc(null);
                FindObjectPostProcess();
            }
            if (o_type.Equals(ObjectsType.Rope) && !brain.HasDiscoveredRope)
            {
                brain.HasDiscoveredRope = true;
                DialogueTrigger.TriggerDialogueCorde(null);
                FindObjectPostProcess();
            }
            if (o_type.Equals(ObjectsType.Sail) && !brain.HasDiscoveredSail)
            {
                brain.HasDiscoveredSail = true;
                DialogueTrigger.TriggerDialogueVoile(null);
                FindObjectPostProcess();
            }
            InventoryManager.Instance.AddObjectOfType(o_type, o_object);
            o_isPickable = false;
            InventoryManager.an_object_is_pickable = false;
            InventoryManager.Instance.SetStatePickupButton(false);
            Destroy(gameObject);
		}
	}

    private void FindObjectPostProcess()
    {
        GameObject playerRoot = GameObject.Find("Player");
        FormsController.Instance.Transformation(FormsController.TransformationType.Human);
        /*
        AudioSource son = GetComponentInParent<AudioSource>();
        son.Play();
        ParticleSystem particles;
        if ((particles = GetComponent<ParticleSystem>()) != null)
        {
            particles.Stop();
        }*/
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ActionsNew>().Stay(100f);
    }

}
