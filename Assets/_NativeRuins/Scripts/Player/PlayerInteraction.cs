using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

    [SerializeField]
    private IList<PickUpScript> items;

    [SerializeField]
    private PickUpScript closestItems = null;

    private void Awake()
    {
        items = new List<PickUpScript>();
    }

    void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.tag.Equals ("3DObjectForInventory")) {
            GameObject currentObject = other.gameObject;
            PickUpScript pickUpScript = currentObject.GetComponent<PickUpScript>();

            // Make the item glow
            currentObject.GetComponent<Renderer>().material.shader = Shader.Find("Outlined/Silhouetted Diffuse");

            // Others settings
            InventoryManager.an_object_is_pickable = true;
            InventoryManager.Instance.SetStatePickupButton(true);

            // Add the items to the list
            items.Add(pickUpScript);
		}
	}

	void OnTriggerStay(Collider other)
    {
        // Reorgize the structure to put closer objects at the top of it.
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (items[i] == null)
            {
                items.RemoveAt(i);
            }
            else
            {
                if (closestItems == null || Vector3.Distance(items[i].transform.position, transform.position) < Vector3.Distance(closestItems.transform.position, transform.position))
                {
                    closestItems = items[i];
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("3DObjectForInventory"))
        {
            GameObject currentObject = other.gameObject;
            PickUpScript pickUpScript = currentObject.GetComponent<PickUpScript>();
            if (items.Contains(pickUpScript))
            {
                // Stop the glowing effect
                currentObject.GetComponent<Renderer>().material.shader = Shader.Find("Mobile/Diffuse");

                // Others settings
                InventoryManager.an_object_is_pickable = false;
                InventoryManager.Instance.SetStatePickupButton(false);

                // Remove it from the list 
                items.Remove(pickUpScript);
            }
        }
    }

    public bool HasItemsClose()
    {
        return items.Count > 0;
    }

    public PickUpScript GetClosestItem()
    {
        return closestItems;
    }

}
