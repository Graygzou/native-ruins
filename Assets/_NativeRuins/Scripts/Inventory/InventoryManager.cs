using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

	[SerializeField] private RectTransform m_canvas;
	[SerializeField] private RectTransform m_bag;
	[SerializeField]private RectTransform b_anchor;
	[SerializeField]private AudioSource m_bagSound;
	[SerializeField]private AudioSource m_craftSound;

	[SerializeField]private RectTransform o_Bow;
	[SerializeField]private RectTransform o_Torch;
	[SerializeField]private RectTransform o_Bonfire;
	[SerializeField]private RectTransform o_Raft;
	[SerializeField]private RectTransform o_Plank;
	[SerializeField]private RectTransform o_Arrow;

    private static int nbArrow;
    private DialogueTrigger dialogue;

    public static bool isBowEquiped = false;
	public static bool isTorchEquiped = false;

	public static bool bag_open = false;
	private Vector2 deltaScreen;

	private static ArrayList inventaire = new ArrayList ();
	public static bool an_object_is_pickable = false;
	// Use this for initialization
	void Start () {
		deltaScreen = m_canvas.sizeDelta;
        nbArrow = 0;
		ActiveRedCross();
        dialogue = GameObject.Find("Affichages/Dialogues/DialogueTrigger").GetComponent<DialogueTrigger>();
    }
	
	// Update is called once per frame
	void Update () {
		//print (inventaire.Count);
		OpenOrCloseInventory ();
		PutWeaponInBag ();
        //NumberOfArrow();
		StartCoroutine(ActiveRedCross());
    }

	public ArrayList GetInventory(){
		return inventaire;
	}

	private void OpenOrCloseInventory(){
		if (Input.GetKeyDown (KeyCode.Tab)) {
			m_bagSound.Play ();
			if (!bag_open) {
				bag_open = !bag_open;
				m_bag.localScale = new Vector3(7f,3.5f,3.5f);
				m_bag.transform.localPosition =  new Vector3(0f, 0f, 0f);
				GameObject.Find ("InventoryManager/Canvas/Craft").SetActive (true);
			} else {
				bag_open = !bag_open;
				m_bag.localScale = new Vector3(2f,1f,1f);
				m_bag.localPosition = b_anchor.localPosition;
				GameObject.Find ("InventoryManager/Canvas/Craft").SetActive (false);
				//m_bag.transform.position += Vector3(-100f,0f,0f);
			}
        }
	}

	public void PutWeaponInBag (){
        if (Input.GetKeyDown (KeyCode.R) && !bag_open) {
            RectTransform clone;
            GameObject player = GameObject.FindWithTag("Player");
			if (isTorchEquiped) {
				AddObjectOfType (ObjectsType.Torch);
                player.GetComponent<ActionsNew>().DisarmWeapon();
                StartCoroutine("DisarmTorch");
				clone = Instantiate(o_Torch) as RectTransform;
				clone.SetParent (GameObject.Find("InventoryManager/Canvas/Bag").transform, false);
				isTorchEquiped = false;
			}
			if (isBowEquiped && !player.GetComponent<MovementControllerHuman>().getIsAiming()) {
				AddObjectOfType (ObjectsType.Bow);
                player.GetComponent<ActionsNew>().DisarmWeapon();
                StartCoroutine("DisarmBow");
				clone = Instantiate(o_Bow) as RectTransform;
				clone.SetParent (GameObject.Find("InventoryManager/Canvas/Bag").transform, false);
				isBowEquiped = false;
			}

		}
	}

    private IEnumerator DisarmTorch() {
        yield return new WaitForSeconds(0.6f);
        GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3/Torch3D").SetActive(false);
    }

    private IEnumerator DisarmBow() {
        yield return new WaitForSeconds(0.6f);
        GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmLeftCollarbone/RigArmLeft1/RigArmLeft2/RigArmLeft3/Bow3D").SetActive(false);
    }


    public static void RemoveObjectOfType(ObjectsType o){
		foreach (ObjectsType obj in inventaire) {
			if (obj == o) {
				inventaire.Remove (o);
                if (o.Equals(ObjectsType.Arrow)) {
                    nbArrow--;
                    displayNumberArrow();
                }
                return;
			}
		}
	}

    public static void DrawArrow() {
        DestroyArrow();
        foreach (ObjectsType obj in inventaire) {
            if (obj.Equals(ObjectsType.Arrow)) {
                inventaire.Remove(obj);
                nbArrow--;
                displayNumberArrow();
                return;
            }
        }
    }

    private static void DestroyArrow() {
        Transform bag = GameObject.Find("Affichages/InventoryManager/Canvas/Bag").transform;
        int i = 0;
        bool trouve = false;
        while (i < bag.childCount && !trouve) {
            if(trouve = (bag.GetChild(i).GetComponent<ObjectScript>().o_type == ObjectsType.Arrow)) {
                Destroy(bag.GetChild(i).gameObject);
            }
            i++;
        }
    }

    public static bool hasArrowLeft() {
        foreach (ObjectsType obj in inventaire) {
            if(obj.Equals(ObjectsType.Arrow)) {
                return true;
            }
        }
        return false;
    }

    private static void displayNumberArrow() {
        if (nbArrow < 10) {
            GameObject.Find("Affichages/Arrow/Nb_arrow").GetComponent<Text>().text = "x " + nbArrow;
        } else {
            GameObject.Find("Affichages/Arrow/Nb_arrow").GetComponent<Text>().text = "x" + nbArrow;
        }
    }

    public static void AddObjectOfType(ObjectsType o){
		inventaire.Add (o);
        // Update the arrow indicator
        if (o.Equals(ObjectsType.Arrow)) {
            nbArrow++;
            displayNumberArrow();
        }
    }

	private IEnumerator ActiveRedCross(){
		int wood = 0;
		int flint = 0;
		int plank = 0;
		int sail = 0;
		int rope = 0;

		foreach (ObjectsType obj in inventaire) {
			switch (obj) {
			case ObjectsType.Wood:
				wood++;
				break;
			case ObjectsType.Rope:
				rope++;
				break;
			case ObjectsType.Flint:
				flint++;
				break;
			case ObjectsType.Plank:
				plank++;
				break;
			case ObjectsType.Sail:
				sail++;
				break;
			}
		}
		if (wood >= 2) {
			GameObject.Find ("InventoryManager/Canvas/Craft/Plank/RedCross").SetActive (false);
			GameObject.Find ("InventoryManager/Canvas/Craft/Plank/Button").SetActive (true);
		} else {
			GameObject.Find ("InventoryManager/Canvas/Craft/Plank/RedCross").SetActive (true);
			GameObject.Find ("InventoryManager/Canvas/Craft/Plank/Button").SetActive (false);
		}
		if (wood >= 1 && flint >=1) {
			GameObject.Find ("InventoryManager/Canvas/Craft/Arrow/RedCross").SetActive (false);
			GameObject.Find ("InventoryManager/Canvas/Craft/Arrow/Button").SetActive (true);
			GameObject.Find ("InventoryManager/Canvas/Craft/Torch/RedCross").SetActive (false);
			GameObject.Find ("InventoryManager/Canvas/Craft/Torch/Button").SetActive (true);
		} else {
			GameObject.Find ("InventoryManager/Canvas/Craft/Arrow/RedCross").SetActive (true);
			GameObject.Find ("InventoryManager/Canvas/Craft/Arrow/Button").SetActive (false);
			GameObject.Find ("InventoryManager/Canvas/Craft/Torch/RedCross").SetActive (true);
			GameObject.Find ("InventoryManager/Canvas/Craft/Torch/Button").SetActive (false);
		}
		if (wood >= 3 && flint >=1) {
			GameObject.Find ("InventoryManager/Canvas/Craft/Bonfire/RedCross").SetActive (false);
			GameObject.Find ("InventoryManager/Canvas/Craft/Bonfire/Button").SetActive (true);
		} else {
			GameObject.Find ("InventoryManager/Canvas/Craft/Bonfire/RedCross").SetActive (true);
			GameObject.Find ("InventoryManager/Canvas/Craft/Bonfire/Button").SetActive (false);
		}
		if (plank >= 5 && sail >=1 && rope >=1) {
			GameObject.Find ("InventoryManager/Canvas/Craft/Raft/RedCross").SetActive (false);
			GameObject.Find ("InventoryManager/Canvas/Craft/Raft/Button").SetActive (true);
		} else {
			GameObject.Find ("InventoryManager/Canvas/Craft/Raft/RedCross").SetActive (true);
			GameObject.Find ("InventoryManager/Canvas/Craft/Raft/Button").SetActive (false);
		}
		yield return new WaitForSeconds (0.2f);
	}

	public void CraftArrow(){
		m_craftSound.Play ();
		int wood = 1;
		int flint = 1;
			int i = 0;
		bool trouve = false;
		Transform bag = GameObject.Find("Affichages/InventoryManager/Canvas/Bag").transform;
		while (i < bag.childCount && !trouve) {
			if((bag.GetChild(i).GetComponent<ObjectScript>().o_type == ObjectsType.Wood) && wood>0) {
				wood--;
				Destroy(bag.GetChild(i).gameObject);
				RemoveObjectOfType (ObjectsType.Wood);
			}else if((bag.GetChild(i).GetComponent<ObjectScript>().o_type == ObjectsType.Flint) && flint>0) {
				flint--;
				Destroy(bag.GetChild(i).gameObject);
				RemoveObjectOfType (ObjectsType.Flint);
			}
			trouve = (flint == 0) && (wood == 0);
			i++;
		}

		InventoryManager.AddObjectOfType(ObjectsType.Arrow);
		InventoryManager.AddObjectOfType(ObjectsType.Arrow);
		InventoryManager.AddObjectOfType(ObjectsType.Arrow);
		RectTransform clone1 = Instantiate(o_Arrow) as RectTransform;
		clone1.SetParent (GameObject.Find("InventoryManager/Canvas/Bag").transform, false);
		RectTransform clone2 = Instantiate(o_Arrow) as RectTransform;
		clone2.SetParent (GameObject.Find("InventoryManager/Canvas/Bag").transform, false);
		RectTransform clone3 = Instantiate(o_Arrow) as RectTransform;
		clone3.SetParent (GameObject.Find("InventoryManager/Canvas/Bag").transform, false);
	}

	public void CraftPlank(){
		m_craftSound.Play ();
		int wood = 2;
		int i = 0;
		bool trouve = false;
		Transform bag = GameObject.Find("Affichages/InventoryManager/Canvas/Bag").transform;
		while (i < bag.childCount && !trouve) {
			if((bag.GetChild(i).GetComponent<ObjectScript>().o_type == ObjectsType.Wood) && wood>0) {
				wood--;
				Destroy(bag.GetChild(i).gameObject);
				RemoveObjectOfType (ObjectsType.Wood);
			}
			trouve = (wood == 0);
			i++;
		}


		InventoryManager.AddObjectOfType(ObjectsType.Plank);
		RectTransform clone1 = Instantiate(o_Plank) as RectTransform;
		clone1.SetParent (GameObject.Find("InventoryManager/Canvas/Bag").transform, false);
	}

	public void CraftTorch(){
		m_craftSound.Play ();
		int wood = 1;
		int flint = 1;
		int i = 0;
		bool trouve = false;
		Transform bag = GameObject.Find("Affichages/InventoryManager/Canvas/Bag").transform;
		while (i < bag.childCount && !trouve) {
			if((bag.GetChild(i).GetComponent<ObjectScript>().o_type == ObjectsType.Wood) && wood>0) {
				wood--;
				Destroy(bag.GetChild(i).gameObject);
				RemoveObjectOfType (ObjectsType.Wood);
			}else if((bag.GetChild(i).GetComponent<ObjectScript>().o_type == ObjectsType.Flint) && flint>0) {
				flint--;
				Destroy(bag.GetChild(i).gameObject);
				RemoveObjectOfType (ObjectsType.Flint);
			}
			trouve = (flint == 0) && (wood == 0);
			i++;
		}

		InventoryManager.AddObjectOfType(ObjectsType.Torch);
		RectTransform clone1 = Instantiate(o_Torch) as RectTransform;
		clone1.SetParent (GameObject.Find("InventoryManager/Canvas/Bag").transform, false);
	}

	public void CraftBonfire(){
		m_craftSound.Play ();
		int wood = 3;
		int flint = 1;
		int i = 0;
		bool trouve = false;
		Transform bag = GameObject.Find("Affichages/InventoryManager/Canvas/Bag").transform;
		while (i < bag.childCount && !trouve) {
			if((bag.GetChild(i).GetComponent<ObjectScript>().o_type == ObjectsType.Wood) && wood>0) {
				wood--;
				Destroy(bag.GetChild(i).gameObject);
				RemoveObjectOfType (ObjectsType.Wood);
			}else if((bag.GetChild(i).GetComponent<ObjectScript>().o_type == ObjectsType.Flint) && flint>0) {
				flint--;
				Destroy(bag.GetChild(i).gameObject);
				RemoveObjectOfType (ObjectsType.Flint);
			}
			trouve = (flint == 0) && (wood == 0);
			i++;
		}

		InventoryManager.AddObjectOfType(ObjectsType.Fire);
		RectTransform clone1 = Instantiate(o_Bonfire) as RectTransform;
		clone1.SetParent (GameObject.Find("InventoryManager/Canvas/Bag").transform, false);
	}

	public void CraftRaft(){
		m_craftSound.Play ();
		int rope = 1;
		int sail = 1;
		int plank = 5;
		int i = 0;
		bool trouve = false;
		Transform bag = GameObject.Find("Affichages/InventoryManager/Canvas/Bag").transform;
		while (i < bag.childCount && !trouve) {
			if((bag.GetChild(i).GetComponent<ObjectScript>().o_type == ObjectsType.Plank) && plank>0) {
				plank--;
				Destroy(bag.GetChild(i).gameObject);
				RemoveObjectOfType (ObjectsType.Plank);
			}else if((bag.GetChild(i).GetComponent<ObjectScript>().o_type == ObjectsType.Sail) && sail>0) {
				sail--;
				Destroy(bag.GetChild(i).gameObject);
				RemoveObjectOfType (ObjectsType.Sail);
			}else if((bag.GetChild(i).GetComponent<ObjectScript>().o_type == ObjectsType.Rope) && rope>0) {
				rope--;
				Destroy(bag.GetChild(i).gameObject);
				RemoveObjectOfType (ObjectsType.Rope);
			}
			trouve = (sail == 0) && (plank == 0) && (rope == 0);
			i++;
		}

		InventoryManager.AddObjectOfType(ObjectsType.Raft);
		RectTransform clone1 = Instantiate(o_Raft) as RectTransform;
		clone1.SetParent (GameObject.Find("InventoryManager/Canvas/Bag").transform, false);

        dialogue.TriggerDialogueFin(null);
	}

    //private void NumberOfArrow() {
    //    int nbArrow = 0;
    //    foreach (Object_Type obj in inventaire) {
    //        if(obj.Equals(Object_Type.Arrow)) {
    //            nbArrow++;
    //        }
    //    }
    //    nbArrowText.text = "x " + nbArrow;
    //}
}
