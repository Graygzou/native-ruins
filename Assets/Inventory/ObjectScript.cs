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

	public InventoryManager.Object_Type o_type;
	[SerializeField]private Rigidbody o_mushroom;
	public bool is_usable=false;
	[SerializeField]private AudioSource m_pickSound;


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
		}
	 }

	private void GetInputs(){
		if (Input.GetKeyDown (KeyCode.E) && is_usable) {
			UseObject (o_type);
		}
	}

	//Ajouter la verification de si on ets humain pour arc et torche !!!
	private void UseObject(InventoryManager.Object_Type o_type){
		switch(o_type) {
		case InventoryManager.Object_Type.Bow:
			if (!InventoryManager.isTorchEquiped) {
				GameObject.Find ("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3/Bow3D").SetActive (true);
				InventoryManager.isBowEquiped = true;
				Destroy(this.gameObject);
			}
			break;
		case InventoryManager.Object_Type.Fire:
			break;
		case InventoryManager.Object_Type.Meat:
			LifeBar.GetComponent<LifeBar>().Eat(30);
			Destroy(this.gameObject);
			break;
		case InventoryManager.Object_Type.Mushroom:
			LifeBar.GetComponent<LifeBar>().Eat(10);
			Destroy(this.gameObject);
			break;
		case InventoryManager.Object_Type.Torch:
			if (!InventoryManager.isBowEquiped) {
				GameObject.Find ("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3/Torch3D").SetActive (true);
				InventoryManager.isTorchEquiped = true;
				Destroy(this.gameObject);
			}
			break;
		}
		InventoryManager.RemoveObjectOfType (o_type);
	}
     
 }