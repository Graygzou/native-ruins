 using UnityEngine;
 using UnityEngine.UI;
 using UnityEngine.EventSystems;
 using System.Collections;

public class ObjectScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    private bool mouseDown = false;
    private Vector3 startMousePos;
    private Vector3 startPos;
    private bool restrictX;
    private bool restrictY;
    private float fakeX;
    private float fakeY;
    private float myWidth;
    private float myHeight;

    private RectTransform ParentRT;
    public RectTransform MyRect;
    private Vector3 player_pos;

    private GameObject lifeBar;
    private GameObject buttonUtiliser;
    private GameObject infoObjets;

    public ObjectsType o_type;

	[SerializeField]
    private Rigidbody o_mushroom;

	public bool is_usable = false;

	[SerializeField]
    private AudioSource m_pickSound;

	private bool isUsed = false;

    private InventoryManager inventoryManager;

    private void Awake()
    {
        inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();
    }

    void Start()
     {
        m_pickSound.Play();
        ParentRT =  (RectTransform)GameObject.Find ("InventoryHUD").transform;
        myWidth = (MyRect.rect.width + 5) / 2;
        myHeight = (MyRect.rect.height + 5) / 2;

        lifeBar = GameObject.FindWithTag("LifeBar");
        buttonUtiliser = GameObject.Find("Affichages/HUD/InventoryHUD/ButtonUtiliser");
        infoObjets = GameObject.FindWithTag("InfoObjets");
    }
 
	void Update () 
	{
		if (InventoryManager.instance.bag_open) {
			DragNDrop ();
		}
	}


	public void OnCollisionEnter2D(Collision2D collision){
		if (collision.gameObject.name == "FallDetector") {
			player_pos = GameObject.FindWithTag ("Player").transform.position;
            GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>().RemoveObjectOfType (o_type);
			Rigidbody clone;
			clone = Instantiate(o_mushroom,new Vector3(player_pos.x+20f*(Random.value-0.5f), player_pos.y+10f, player_pos.z+20f*(Random.value-0.5f)) ,Random.rotation) as Rigidbody;
			Destroy (this.gameObject);
		}
	}

     public void OnPointerDown(PointerEventData ped) 
     {
         mouseDown = true;
         startPos = transform.position;
         startMousePos = Input.mousePosition;
     }
     
     public void OnPointerUp(PointerEventData ped) 
     {
        mouseDown = false;
        //InventoryManager.Instance.ChangePickUpButtonState(false);
        HideInfo ();
     }
     
	 private void DragNDrop(){
		if (mouseDown) {
			Vector3 currentPos = Input.mousePosition;
			Vector3 diff = currentPos - startMousePos;
			Vector3 pos = startPos + diff;
			transform.position = pos;

			if(transform.localPosition.x < 0 - ((ParentRT.rect.width / 2)  - myWidth) || transform.localPosition.x > ((ParentRT.rect.width / 2) - myWidth))
				restrictX = true;
			else
				restrictX = false;

			if(transform.localPosition.y < 0 - ((ParentRT.rect.height / 2)  - myHeight) || transform.localPosition.y > ((ParentRT.rect.height / 2) - myHeight))
				restrictY = true;
			else
				restrictY = false;

			if(restrictX)
			{
				if(transform.localPosition.x < 0)
					fakeX = 0 - (ParentRT.rect.width / 2) + myWidth;
				else
					fakeX = (ParentRT.rect.width / 2) - myWidth;

				Vector3 xpos = new Vector3 (fakeX, transform.localPosition.y, 0.0f);
				transform.localPosition = xpos;
			}

			if(restrictY)
			{
				if(transform.localPosition.y < 0)
					fakeY = 0 - (ParentRT.rect.height / 2) + myHeight;
				else
					fakeY = (ParentRT.rect.height / 2) - myHeight;

				Vector3 ypos = new Vector3 (transform.localPosition.x, fakeY, 0.0f);
				transform.localPosition = ypos;
			}
			GetInputs ();
			if (is_usable && !isUsed) {
                buttonUtiliser.SetActive(true);
				ShowInfo (o_type);
			}
			if(!is_usable)
				ShowInfo (o_type);
		}
	 }

	private void GetInputs(){
		if (Input.GetKeyDown (KeyCode.E) && is_usable) {
			UseObject (o_type);
		}
	}

	//Ajouter la verification de si on ets humain pour arc et torche !!!
	private void UseObject(ObjectsType o_type){
		switch(o_type) {
		case ObjectsType.Bow:
			if (!InventoryManager.instance.isTorchEquiped) {
				InventoryManager.instance.isBowEquiped = true;
                buttonUtiliser.SetActive(false);
				HideInfo ();
                inventoryManager.RemoveObjectOfType (o_type);
				isUsed = true;
                GameObject player = GameObject.FindWithTag("Player");
                player.GetComponent<ActionsNew>().EquipWeapon();
                GameObject.Find ("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmLeftCollarbone/RigArmLeft1/RigArmLeft2/RigArmLeft3/Bow3D").SetActive (true);
			    Destroy(this.gameObject);
            }
			break;
		case ObjectsType.Fire:
			break;
		case ObjectsType.Meat:
            lifeBar.GetComponent<LifeBar> ().Eat (30);
            buttonUtiliser.SetActive (false);
			HideInfo ();
                inventoryManager.RemoveObjectOfType (o_type);
			isUsed = true;
			Destroy(this.gameObject);
			break;
		case ObjectsType.Mushroom:
            lifeBar.GetComponent<LifeBar>().Eat(10);
            buttonUtiliser.SetActive(false);
			HideInfo ();
                inventoryManager.RemoveObjectOfType (o_type);
			isUsed = true;
			Destroy(this.gameObject);
			break;
		case ObjectsType.Torch:
			if (!InventoryManager.instance.isBowEquiped) {
				InventoryManager.instance.isTorchEquiped = true;
                buttonUtiliser.SetActive(false);
				HideInfo ();
                inventoryManager.RemoveObjectOfType (o_type);
				isUsed = true;
                GameObject player = GameObject.FindWithTag("Player");
                player.GetComponent<ActionsNew>().EquipWeapon();
                GameObject.Find ("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3/Torch3D").SetActive (true);
				Destroy(this.gameObject);
			}
			break;
		}
	}

	private void ShowInfo(ObjectsType o_type){
		switch (o_type) {
		case ObjectsType.Arrow:
            infoObjets.transform.Find("Arrow").gameObject.SetActive(true);
            break;
		case ObjectsType.Mushroom:
            infoObjets.transform.Find("Mushroom").gameObject.SetActive(true);
            break;
		case ObjectsType.Bow:
            infoObjets.transform.Find("Bow").gameObject.SetActive(true);
            break;
		case ObjectsType.Meat:
            infoObjets.transform.Find("Meat").gameObject.SetActive(true);
            break;
		case ObjectsType.Plank:
            infoObjets.transform.Find("Plank").gameObject.SetActive(true);
            break;
		case ObjectsType.Sail:
            infoObjets.transform.Find("Sail").gameObject.SetActive(true);
            break;
		case ObjectsType.Fire:
            infoObjets.transform.Find("Bonfire").gameObject.SetActive(true);
            break;
	    case ObjectsType.Raft:
            infoObjets.transform.Find("Raft").gameObject.SetActive(true);
            break;
		case ObjectsType.Wood:
            infoObjets.transform.Find("Wood").gameObject.SetActive(true);
            break;
		case ObjectsType.Rope:
            infoObjets.transform.Find("Rope").gameObject.SetActive(true);
            break;
		case ObjectsType.Flint:
            infoObjets.transform.Find("Flint").gameObject.SetActive(true);
			break;
		case ObjectsType.Torch:
            infoObjets.transform.Find("Torch").gameObject.SetActive(true);
			break;
		}
	}

	private void HideInfo(){
        for (int i = 0; i < infoObjets.transform.childCount; i++)
        {
            infoObjets.transform.GetChild(i).gameObject.SetActive(false);
        }
	}
     
 }