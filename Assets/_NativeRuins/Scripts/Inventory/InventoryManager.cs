using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    public static InventoryManager instance = null;

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

    #region Serialize Field
    [Header("Component settings")]
    [SerializeField]
    private CraftRecipeUI[] craftRecipesUI = new CraftRecipeUI[NB_CRAFT_RECETTE];
    [SerializeField]
    private GameObject craft;
    [SerializeField]
    private Text arrowNumber;
    [SerializeField]
    private BoxCollider2D fallDetector;
    [SerializeField]
    private GameObject pickupButton;

    [Header("RectTransform settings")]
    [SerializeField] private RectTransform m_canvas;
    [SerializeField] private RectTransform m_bag;
    [SerializeField] private RectTransform m_items;
    [SerializeField] private RectTransform spawningAnchor;
    [SerializeField] private AudioClip m_bagSound;
    [SerializeField] private AudioClip m_craftSound;

    [Header("RectTransform of items")]
    [SerializeField] private RectTransform o_Bow;
    [SerializeField] private RectTransform o_Torch;
    [SerializeField] private RectTransform o_Bonfire;
    [SerializeField] private RectTransform o_Raft;
    [SerializeField] private RectTransform o_Plank;
    [SerializeField] private RectTransform o_Arrow;

    #endregion

    public bool isBowEquiped = false;
    public bool isTorchEquiped = false;

    public bool bag_open = false;
    private Vector2 deltaScreen;

    private AudioSource audioSource;
    private EdgeCollider2D collider;

    private static SortedDictionary<ObjectsType, int> inventaire = new SortedDictionary<ObjectsType, int>();
    public static bool an_object_is_pickable = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        collider = m_bag.GetComponent<EdgeCollider2D>();
    }

    void Start () {
		deltaScreen = m_canvas.sizeDelta;
    }
	
	void Update () {
		OpenOrCloseInventory ();
		PutWeaponInBag ();
    }

    #region Add / Remove methods

    public void EmptyBag()
    {
        inventaire.Clear();
        DisplayNumberArrow();
    }

    public void RemoveObjectOfType(ObjectsType type, GameObject obj)
    {
        if (!inventaire.ContainsKey(type))
        {
            Debug.LogWarning("Removing non existing object !");
        }
        else
        {
            if (inventaire[type] <= 1)
            {
                inventaire.Remove(type);
            }
            else
            {
                inventaire[type] -= 1;
            }
            DisplayNumberArrow();
        }
        // Destroy the object
        Destroy(obj);

        ActiveRedCross();
    }

    public void RemoveObjectOfType(ObjectsType type)
    {
        Debug.Log("HERE");
        Transform bag = m_items.transform;
        bool trouve = false;
        int i = 0;
        while (i < bag.childCount && !trouve)
        {
            if (trouve = (bag.GetChild(i).GetComponent<ObjectScript>().o_type == type))
            {
                RemoveObjectOfType(type, bag.GetChild(i).gameObject);
            }
            i++;
        }
    }

    public void AddObjectOfType(ObjectsType obj, RectTransform obj2D)
    {
        AddObjectOfType(obj, obj2D, 1);
    }

    public void AddObjectOfType(ObjectsType obj, RectTransform obj2D, int amount)
    {
        // Add the object in the structure
        if (inventaire.ContainsKey(obj))
        {
            inventaire[obj] += amount;
        }
        else
        {
            inventaire.Add(obj, amount);
        }
        // Add physically the object
        for (int indice = 0; indice < amount; indice++)
        {
            Debug.Log(GetSpawningPosition());
            RectTransform clone = Instantiate(obj2D) as RectTransform;
            clone.SetParent(m_items);
            clone.anchoredPosition = GetSpawningPosition();
            clone.localRotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
            // Scale the object back (useful during craft)
            clone.transform.localScale = obj2D.localScale;
        }
        // Adding Post-Process
        DisplayNumberArrow();
        ActiveRedCross();
    }
    #endregion

    #region Crafts methods
    /**
     * Craft three arrows from 1 wood and 1 flint.
     * @Pre: Their are, at least, 1 wood and 1 flint in the inventory.
     */
    public void CraftArrow()
    {
        Debug.Log("CraftArrows");
        CraftObject(1, 1);
        AddObjectOfType(ObjectsType.Arrow, o_Arrow);
        AddObjectOfType(ObjectsType.Arrow, o_Arrow);
        AddObjectOfType(ObjectsType.Arrow, o_Arrow);
    }

    /**
     * Craft 1 plank from 2 wood.
     * @Pre: Their are, at least, 2 wood in the inventory.
     */
    public void CraftPlank()
    {
        CraftObject(2);
        AddObjectOfType(ObjectsType.Plank, o_Plank);
    }

    /**
     * Craft 1 torch from 1 wood, 1 flint.
     * @Pre: Their are, at least, 1 wood and 1 flint in the inventory.
     */
    public void CraftTorch()
    {
        CraftObject(1, 1);
        AddObjectOfType(ObjectsType.Torch, o_Torch);
    }

    /**
     * Craft 1 bonfire from 3 wood, 1 flint.
     * @Pre: Their are, at least, 3 wood and 1 flint in the inventory.
     */
    public void CraftBonfire()
    {
        CraftObject(3, 1);
        AddObjectOfType(ObjectsType.Torch, o_Torch);
        AddObjectOfType(ObjectsType.Fire, o_Bonfire);
    }

    public void CraftRaft()
    {
        CraftObject(0, 0, 1, 5, 1);
        AddObjectOfType(ObjectsType.Raft, o_Raft);

        // Should not be there.
        //DialogueTrigger.TriggerDialogueFin(null);
    }

    private void CraftObject(int wood = 0, int flint = 0, int sail = 0, int plank = 0, int rope = 0)
    {
        audioSource.PlayOneShot(m_craftSound);
        Debug.Log("Craf !!t");
        bool trouve = false;
        Transform bag = m_items.transform;

        int i = 0;
        while (i < bag.childCount && !trouve)
        {
            ObjectsType type = bag.GetChild(i).GetComponent<ObjectScript>().o_type;
            switch (type)
            {
                case ObjectsType.Wood:
                    if(wood > 0)
                    {
                        wood--;
                        RemoveObjectOfType(type, bag.GetChild(i).gameObject);
                    }
                    break;
                case ObjectsType.Flint:
                    if (flint > 0)
                    {
                        flint--;
                        RemoveObjectOfType(type, bag.GetChild(i).gameObject);
                    }
                    break;
                case ObjectsType.Plank:
                    if (plank > 0)
                    {
                        plank--;
                        RemoveObjectOfType(type, bag.GetChild(i).gameObject);
                    }
                    break;
                case ObjectsType.Sail:
                    if (sail > 0)
                    {
                        sail--;
                        RemoveObjectOfType(type, bag.GetChild(i).gameObject);
                    }
                    break;
                case ObjectsType.Rope:
                    if (rope > 0)
                    {
                        rope--;
                        RemoveObjectOfType(type, bag.GetChild(i).gameObject);
                    }
                    break;
                default:
                    break;
            }
            trouve = (wood == 0) && (flint == 0) && (plank == 0) && (sail == 0) && (rope == 0);
            i++;
        }
    }
    #endregion

    public void SetStatePickupButton(bool state)
    {
        pickupButton.SetActive(state);
    }

    public SortedDictionary<ObjectsType, int> GetInventory(){
		return inventaire;
	}

	private void OpenOrCloseInventory()
    {
		if (Input.GetKeyDown (KeyCode.Tab))
        {
            audioSource.PlayOneShot(m_bagSound);
            if (!bag_open)
            {
                m_bag.localPosition = Vector3.zero;
                m_bag.anchoredPosition = new Vector3(-960f, 540f, 0f);
                m_bag.localScale = new Vector3(2.5f, 2.5f, 1f);
                fallDetector.offset = new Vector2(0f, 40f);
                collider.edgeRadius = 25f;
            }
            else
            {
                m_bag.localScale = new Vector3(1f, 1f, 1f);
                m_bag.anchoredPosition = new Vector3(-200f, 100f, 0f);
                fallDetector.offset = new Vector2(0f, -250f);
                collider.edgeRadius = 10f;
            }
            craft.SetActive(!bag_open);
            bag_open = !bag_open;
        }
	}

    public Vector2 GetSpawningPosition()
    {
        float halfWidth = spawningAnchor.sizeDelta.x / 2;
        float halfheight = spawningAnchor.sizeDelta.y / 2;
        float xValue = Random.Range(spawningAnchor.anchoredPosition.x - halfWidth, spawningAnchor.anchoredPosition.x + halfWidth);
        float yValue = Random.Range(spawningAnchor.anchoredPosition.y - halfheight, spawningAnchor.anchoredPosition.y + halfheight);
        return new Vector2(xValue, yValue);
    }

    public void PutWeaponInBag ()
    {
        if (Input.GetKeyDown (KeyCode.R) && !bag_open) {
            GameObject player = GameObject.FindWithTag("Player");
			if (isTorchEquiped)
            {
				AddObjectOfType (ObjectsType.Torch, o_Torch);
                player.GetComponent<ActionsNew>().DisarmWeapon();
                StartCoroutine("DisarmTorch");
				isTorchEquiped = false;
			}
			if (isBowEquiped && !player.GetComponent<MovementControllerHuman>().getIsAiming())
            {
				AddObjectOfType (ObjectsType.Bow, o_Bow);
                player.GetComponent<ActionsNew>().DisarmWeapon();
                StartCoroutine("DisarmBow");
				isBowEquiped = false;
			}
		}
	}

    private IEnumerator DisarmTorch()
    {
        yield return new WaitForSeconds(0.6f);
        GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3/Torch3D").SetActive(false);
    }

    private IEnumerator DisarmBow()
    {
        yield return new WaitForSeconds(0.6f);
        GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmLeftCollarbone/RigArmLeft1/RigArmLeft2/RigArmLeft3/Bow3D").SetActive(false);
    }

    public void DrawArrow()
    {
        RemoveObjectOfType(ObjectsType.Arrow);
    }

    public static bool HasArrowLeft()
    {
        return inventaire[ObjectsType.Arrow] > 0;
    }

    internal int GetNumberItems(ObjectsType objectType)
    {
        return inventaire.ContainsKey(objectType) ? inventaire[objectType] : 0;
    }

    #region UI managements

    private void DisplayNumberArrow()
    {
        if (inventaire.ContainsKey(ObjectsType.Arrow))
        {
            arrowNumber.text = "x" + inventaire[ObjectsType.Arrow];
        }
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
    #endregion
}
