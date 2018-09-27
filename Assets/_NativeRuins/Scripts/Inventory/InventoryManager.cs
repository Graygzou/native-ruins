using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    #region Structs
    [System.Serializable]
    public struct CraftRecipeUI
    {
        [SerializeField]
        public GameObject redCross;

        [SerializeField]
        public GameObject button;
    }
    #endregion

    #region Consts
    private const int NB_CRAFT_RECETTE = 5;
    #endregion

    #region Fields
    [Header("Component settings")]
    [SerializeField]
    private CraftRecipeUI[] craftRecipesUI = new CraftRecipeUI[NB_CRAFT_RECETTE];
    [SerializeField]
    private GameObject craft;
    [SerializeField]
    private Text arrowNumber;

    [Header("RectTransform settings")]
    [SerializeField] private RectTransform m_canvas;
    [SerializeField] private RectTransform m_bag;
    [SerializeField] private RectTransform b_anchor;
    [SerializeField] private AudioSource m_bagSound;
    [SerializeField] private AudioSource m_craftSound;

    [Header("RectTransform of items")]
    [SerializeField] private RectTransform o_Bow;
    [SerializeField] private RectTransform o_Torch;
    [SerializeField] private RectTransform o_Bonfire;
    [SerializeField] private RectTransform o_Raft;
    [SerializeField] private RectTransform o_Plank;
    [SerializeField] private RectTransform o_Arrow;

    public static bool isBowEquiped = false;
	public static bool isTorchEquiped = false;

	public static bool bag_open = false;
	private Vector2 deltaScreen;

	private static SortedDictionary<ObjectsType, int> inventaire = new SortedDictionary<ObjectsType, int>();
	public static bool an_object_is_pickable = false;
    #endregion

    void Start () {
		deltaScreen = m_canvas.sizeDelta;
    }
	
	void Update () {
		OpenOrCloseInventory ();
		PutWeaponInBag ();
    }

    public RectTransform GetBagRectTransform()
    {
        return m_bag;
    }

	public SortedDictionary<ObjectsType, int> GetInventory(){
		return inventaire;
	}

	private void OpenOrCloseInventory(){
		if (Input.GetKeyDown (KeyCode.Tab)) {
			m_bagSound.Play ();
            
            if (!bag_open) {
                m_bag.localPosition = Vector3.zero; 
				m_bag.localScale = new Vector3(2.5f, 2.5f, 1f);
			} else {
                m_bag.anchoredPosition = new Vector3(-200f, 100f, 0f);
                m_bag.localScale = new Vector3(1f, 1f, 1f);
			}
            craft.SetActive(!bag_open);
            bag_open = !bag_open;
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
				clone.SetParent (m_bag.transform, false);
				isTorchEquiped = false;
			}
			if (isBowEquiped && !player.GetComponent<MovementControllerHuman>().getIsAiming()) {
				AddObjectOfType (ObjectsType.Bow);
                player.GetComponent<ActionsNew>().DisarmWeapon();
                StartCoroutine("DisarmBow");
				clone = Instantiate(o_Bow) as RectTransform;
				clone.SetParent (m_bag.transform, false);
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


    public void RemoveObjectOfType(ObjectsType obj)
    {
        if(!inventaire.ContainsKey(obj))
        {
            Debug.LogWarning("Removing non existing object !");
        }
        else
        {
            if (inventaire[obj] <= 1)
            {
                inventaire.Remove(obj);
            } 
            else
            {
                inventaire[obj] -= 1;
            }
            DisplayNumberArrow();
        }
        ActiveRedCross();
    }

    public void DrawArrow()
    {
        DestroyArrow();
        RemoveObjectOfType(ObjectsType.Arrow);
    }

    private void DestroyArrow()
    {
        Transform bag = m_bag.transform;
        int i = 0;
        bool trouve = false;
        while (i < bag.childCount && !trouve)
        {
            if(trouve = (bag.GetChild(i).GetComponent<ObjectScript>().o_type == ObjectsType.Arrow))
            {
                Destroy(bag.GetChild(i).gameObject);
            }
            i++;
        }
    }

    public static bool HasArrowLeft()
    {
        return inventaire[ObjectsType.Arrow] > 0;
    }

    private void DisplayNumberArrow()
    {
        if (inventaire.ContainsKey(ObjectsType.Arrow))
        {
            arrowNumber.text = "x" + inventaire[ObjectsType.Arrow];
        }
    }

    public void AddObjectOfType(ObjectsType obj)
    {
        AddObjectOfType(obj, 1);
    }

    public void AddObjectOfType(ObjectsType obj, int amount){
        if (inventaire.ContainsKey(obj))
        {
            inventaire[obj] += amount;
        }
        else
        {
            inventaire.Add(obj, amount);
        }
        DisplayNumberArrow();
        ActiveRedCross();
    }

    public void EmptyBag()
    {
        inventaire.Clear();
        DisplayNumberArrow();
    }

    private void ActiveRedCross()
    {
        bool ownOnePieceFlint = inventaire.ContainsKey(ObjectsType.Flint) && inventaire[ObjectsType.Flint] >= 1;

        bool ownOnePieceWood = inventaire.ContainsKey(ObjectsType.Wood) && inventaire[ObjectsType.Wood] >= 1;
        bool ownTwoPiecesWood = inventaire.ContainsKey(ObjectsType.Wood) && inventaire[ObjectsType.Wood] >= 2;
        bool ownThreePiecesWood = inventaire.ContainsKey(ObjectsType.Wood) && inventaire[ObjectsType.Wood] >= 3;

        bool ownOnePieceRope = inventaire.ContainsKey(ObjectsType.Rope) && inventaire[ObjectsType.Rope] >= 1;
        
        bool ownOnePieceSail = inventaire.ContainsKey(ObjectsType.Sail) && inventaire[ObjectsType.Sail] >= 1;

        bool ownFivePiecesPlank = inventaire.ContainsKey(ObjectsType.Plank) && inventaire[ObjectsType.Plank] >= 5;

        bool activeCraftArrowOrTorch = ownOnePieceWood && ownOnePieceFlint;
        craftRecipesUI[0].redCross.SetActive(!activeCraftArrowOrTorch);
        craftRecipesUI[0].button.SetActive(activeCraftArrowOrTorch);

        bool activeCraftPlank = ownTwoPiecesWood;
        craftRecipesUI[1].redCross.SetActive(!activeCraftPlank);
        craftRecipesUI[1].button.SetActive(activeCraftPlank);

        bool activeCraftBonFire = ownThreePiecesWood && ownOnePieceFlint;
        craftRecipesUI[2].redCross.SetActive(!activeCraftBonFire);
        craftRecipesUI[2].button.SetActive(activeCraftBonFire);

        craftRecipesUI[3].redCross.SetActive(!activeCraftArrowOrTorch);
        craftRecipesUI[3].button.SetActive(activeCraftArrowOrTorch);

        bool activeCraftRaft = ownFivePiecesPlank && ownOnePieceSail && ownOnePieceRope;
        craftRecipesUI[4].redCross.SetActive(!activeCraftRaft);
        craftRecipesUI[4].button.SetActive(activeCraftRaft);
    }

	public void CraftArrow(){
		m_craftSound.Play ();
		int wood = 1;
		int flint = 1;
			int i = 0;
		bool trouve = false;
		Transform bag = m_bag.transform;
		while (i < bag.childCount && !trouve) {
			if((bag.GetChild(i).GetComponent<ObjectScript>().o_type == ObjectsType.Wood) && wood > 0) {
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

		AddObjectOfType(ObjectsType.Arrow);
		AddObjectOfType(ObjectsType.Arrow);
		AddObjectOfType(ObjectsType.Arrow);
		RectTransform clone1 = Instantiate(o_Arrow) as RectTransform;
		clone1.SetParent (bag.transform, false);
		RectTransform clone2 = Instantiate(o_Arrow) as RectTransform;
		clone2.SetParent (bag.transform, false);
		RectTransform clone3 = Instantiate(o_Arrow) as RectTransform;
		clone3.SetParent (bag.transform, false);
	}

	public void CraftPlank(){
		m_craftSound.Play ();
		int wood = 2;
		int i = 0;
		bool trouve = false;
		Transform bag = m_bag.transform;
		while (i < bag.childCount && !trouve) {
			if((bag.GetChild(i).GetComponent<ObjectScript>().o_type == ObjectsType.Wood) && wood>0) {
				wood--;
				Destroy(bag.GetChild(i).gameObject);
				RemoveObjectOfType (ObjectsType.Wood);
			}
			trouve = (wood == 0);
			i++;
		}


		AddObjectOfType(ObjectsType.Plank);
		RectTransform clone1 = Instantiate(o_Plank) as RectTransform;
		clone1.SetParent (m_bag.transform, false);
	}

    internal int GetNumberItems(ObjectsType objectType)
    {
        return inventaire.ContainsKey(objectType) ? inventaire[objectType] : 0;
    }

    public void CraftTorch(){
		m_craftSound.Play ();
		int wood = 1;
		int flint = 1;
		int i = 0;
		bool trouve = false;
		Transform bag = m_bag.transform;
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

		AddObjectOfType(ObjectsType.Torch);
		RectTransform clone1 = Instantiate(o_Torch) as RectTransform;
		clone1.SetParent (bag.transform, false);
	}

	public void CraftBonfire(){
		m_craftSound.Play ();
		int wood = 3;
		int flint = 1;
		int i = 0;
		bool trouve = false;
		Transform bag = m_bag.transform;
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

		AddObjectOfType(ObjectsType.Fire);
		RectTransform clone1 = Instantiate(o_Bonfire) as RectTransform;
		clone1.SetParent (m_bag.transform, false);
	}

	public void CraftRaft(){
		m_craftSound.Play ();
		int rope = 1;
		int sail = 1;
		int plank = 5;
		int i = 0;
		bool trouve = false;
		Transform bag = m_bag.transform;
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

		AddObjectOfType(ObjectsType.Raft);
		RectTransform clone1 = Instantiate(o_Raft) as RectTransform;
		clone1.SetParent (bag.transform, false);

        DialogueTrigger.TriggerDialogueFin(null);
	}
}
