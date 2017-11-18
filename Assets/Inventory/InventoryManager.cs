using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

	[SerializeField] private RectTransform m_canvas;
	[SerializeField] private RectTransform m_bag;
	[SerializeField]private RectTransform b_anchor;
	[SerializeField]private AudioSource m_bagSound;

	[SerializeField]private RectTransform o_Bow;
	[SerializeField]private RectTransform o_Torch;
    public Text nbArrowText;

    public static bool isBowEquiped = false;
	public static bool isTorchEquiped = false;

	public static bool bag_open = false;
	private Vector2 deltaScreen;

	public enum Object_Type {Mushroom, Meat, Flint, Wood, Bow, Arrow, Torch, Fire, Plank, Sail, Rope, Raft};
	private static ArrayList inventaire = new ArrayList ();
	public static bool an_object_is_pickable = false;
	// Use this for initialization
	void Start () {
		deltaScreen = m_canvas.sizeDelta;
	}
	
	// Update is called once per frame
	void Update () {
		//print (inventaire.Count);
		OpenOrCloseInventory ();
		PutWeaponInBag ();
        NumberOfArrow();
    }

	private void OpenOrCloseInventory(){
		if (Input.GetKeyDown (KeyCode.Tab)) {
			m_bagSound.Play ();
			if (!bag_open) {
				bag_open = !bag_open;
				m_bag.localScale = new Vector3(7f,3.5f,3.5f);
				m_bag.transform.localPosition =  new Vector3(0f, 0f, 0f);
			} else {
				bag_open = !bag_open;
				m_bag.localScale = new Vector3(2f,1f,1f);
				m_bag.localPosition = b_anchor.localPosition;
					
				//m_bag.transform.position += Vector3(-100f,0f,0f);
			}
        }
	}

	public void PutWeaponInBag (){
		if (Input.GetKeyDown (KeyCode.R) && !bag_open) {
			RectTransform clone;
			if (isTorchEquiped) {
				AddObjectOfType (Object_Type.Torch);
				GameObject.Find ("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3/Torch3D").SetActive(false);
				clone = Instantiate(o_Torch) as RectTransform;
				clone.SetParent (GameObject.Find("InventoryManager/Canvas/Bag").transform, false);
				isTorchEquiped = false;
			}
			if (isBowEquiped) {
				AddObjectOfType (Object_Type.Bow);
				GameObject.Find ("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmLeftCollarbone/RigArmLeft1/RigArmLeft2/RigArmLeft3/Bow3D").SetActive(false);
				clone = Instantiate(o_Bow) as RectTransform;
				clone.SetParent (GameObject.Find("InventoryManager/Canvas/Bag").transform, false);
				isBowEquiped = false;
			}

		}
	}


	public static void RemoveObjectOfType(Object_Type o){
		foreach (Object_Type obj in inventaire) {
			if (obj == o) {
				inventaire.Remove (o);
				return;
			}
		}
	}

	public static void AddObjectOfType(Object_Type o){
		inventaire.Add (o);
	}

    private void NumberOfArrow()
    {
        int nbArrow = 0;
        foreach (Object_Type obj in inventaire)
        {
            if(obj.Equals(Object_Type.Arrow))
            {
                nbArrow++;
            }
        }
        nbArrowText.text = "x " + nbArrow;
    }
}
