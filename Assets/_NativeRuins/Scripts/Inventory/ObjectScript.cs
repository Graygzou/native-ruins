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

	private GameObject LifeBar;
	private GameObject HungerBar;

	public ObjectsType o_type;
	[SerializeField]private Rigidbody o_mushroom;
	public bool is_usable=false;
	[SerializeField]private AudioSource m_pickSound;

	private bool isUsed = false;

     void Start()
     {
		m_pickSound.Play();
		 HungerBar = GameObject.Find ("Gauges/Hunger");
		 LifeBar = GameObject.Find ("Gauges/Life");
		 ParentRT =  (RectTransform)GameObject.Find ("Canvas").transform;
         myWidth = (MyRect.rect.width + 5) / 2;
         myHeight = (MyRect.rect.height + 5) / 2;
     }
 
	void Update () 
	{
		if (InventoryManager.bag_open) {
			DragNDrop ();
		}
	}


	public void OnCollisionEnter2D(Collision2D collision){
		if (collision.gameObject.name=="FallDetector") {
			player_pos = GameObject.FindWithTag ("Player").transform.position;
			InventoryManager.RemoveObjectOfType (o_type);
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
		 GameObject.Find ("InventoryManager/Canvas/ButtonUtiliser").SetActive(false);
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
				GameObject.Find ("InventoryManager/Canvas/ButtonUtiliser").SetActive(true);
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
			if (!InventoryManager.isTorchEquiped) {
				InventoryManager.isBowEquiped = true;
				GameObject.Find ("InventoryManager/Canvas/ButtonUtiliser").SetActive(false);
				HideInfo ();
				InventoryManager.RemoveObjectOfType (o_type);
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
			LifeBar.GetComponent<LifeBar> ().Eat (30);
			GameObject.Find ("InventoryManager/Canvas/ButtonUtiliser").SetActive (false);
			HideInfo ();
			InventoryManager.RemoveObjectOfType (o_type);
			isUsed = true;
			Destroy(this.gameObject);
			break;
		case ObjectsType.Mushroom:
			LifeBar.GetComponent<LifeBar>().Eat(10);
			GameObject.Find ("InventoryManager/Canvas/ButtonUtiliser").SetActive(false);
			HideInfo ();
			InventoryManager.RemoveObjectOfType (o_type);
			isUsed = true;
			Destroy(this.gameObject);
			break;
		case ObjectsType.Torch:
			if (!InventoryManager.isBowEquiped) {
				InventoryManager.isTorchEquiped = true;
				GameObject.Find ("InventoryManager/Canvas/ButtonUtiliser").SetActive(false);
				HideInfo ();
				InventoryManager.RemoveObjectOfType (o_type);
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
			GameObject.Find ("InventoryManager/Canvas/InfoObjets/Arrow").SetActive(true);
			break;
		case ObjectsType.Mushroom:
			GameObject.Find ("InventoryManager/Canvas/InfoObjets/Mushroom").SetActive(true);
			break;
		case ObjectsType.Bow:
			GameObject.Find ("InventoryManager/Canvas/InfoObjets/Bow").SetActive(true);
			break;
		case ObjectsType.Meat:
			GameObject.Find ("InventoryManager/Canvas/InfoObjets/Meat").SetActive(true);
			break;
		case ObjectsType.Plank:
			GameObject.Find ("InventoryManager/Canvas/InfoObjets/Plank").SetActive(true);
			break;
		case ObjectsType.Sail:
			GameObject.Find ("InventoryManager/Canvas/InfoObjets/Sail").SetActive(true);
			break;
		case ObjectsType.Fire:
			GameObject.Find ("InventoryManager/Canvas/InfoObjets/Bonfire").SetActive(true);
			break;
		case ObjectsType.Raft:
			GameObject.Find ("InventoryManager/Canvas/InfoObjets/Raft").SetActive(true);
			break;
		case ObjectsType.Wood:
			GameObject.Find ("InventoryManager/Canvas/InfoObjets/Wood").SetActive(true);
			break;
		case ObjectsType.Rope:
			GameObject.Find ("InventoryManager/Canvas/InfoObjets/Rope").SetActive(true);
			break;
		case ObjectsType.Flint:
			GameObject.Find ("InventoryManager/Canvas/InfoObjets/Flint").SetActive(true);
			break;
		case ObjectsType.Torch:
			GameObject.Find ("InventoryManager/Canvas/InfoObjets/Torch").SetActive(true);
			break;
		}
	}

	private void HideInfo(){
		GameObject.Find ("InventoryManager/Canvas/InfoObjets/Mushroom").SetActive(false);
		GameObject.Find ("InventoryManager/Canvas/InfoObjets/Arrow").SetActive(false);
		GameObject.Find ("InventoryManager/Canvas/InfoObjets/Meat").SetActive(false);
		GameObject.Find ("InventoryManager/Canvas/InfoObjets/Bow").SetActive(false);
		GameObject.Find ("InventoryManager/Canvas/InfoObjets/Plank").SetActive(false);
		GameObject.Find ("InventoryManager/Canvas/InfoObjets/Sail").SetActive(false);
		GameObject.Find ("InventoryManager/Canvas/InfoObjets/Raft").SetActive(false);
		GameObject.Find ("InventoryManager/Canvas/InfoObjets/Torch").SetActive(false);
		GameObject.Find ("InventoryManager/Canvas/InfoObjets/Bonfire").SetActive(false);
		GameObject.Find ("InventoryManager/Canvas/InfoObjets/Wood").SetActive(false);
		GameObject.Find ("InventoryManager/Canvas/InfoObjets/Rope").SetActive(false);
		GameObject.Find ("InventoryManager/Canvas/InfoObjets/Flint").SetActive(false);
	}
     
 }