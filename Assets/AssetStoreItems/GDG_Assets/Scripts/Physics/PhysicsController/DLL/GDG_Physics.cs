/// <summary>
/// 
/// /// 11/1/2013
/// Steve Peters
/// GameDevelopersGuild.com
/// Miami Fl
/// 
/// GDG Physics is the main class for the GDG physics system. It maintains attributes that all physics classes need.
/// 
/// The structure of the system involves a Object Master which holds unbroken Destruction Level 0 (DL0) objects in the scene.
/// After these objects are broken, either by contact or by calling a break method, a DL1 Master object is popped. 
/// 
/// The DL1 Master contains all of the lower DL1 chunks which it pops off the stack and passes attributes. The DL1 Master
/// is required to keep the DL1 objects center aligned with that of the DL0 parent. The DL2 objects are spawned in a similar fashion.
/// For the DL3 objects, it is simpler to keep them as children of the DL2 object, and turn them on when needed.
/// 
/// Before any object is fractured, a check is done against the FPS controller system singleton to see if the framerate is
/// sufficient for breaking to a lower level.
/// 
/// It also contains a setinitialproperties method which is used to populate all subchunks with it's needed properties
/// before they are stacked, which is (slightly) more efficient that doing it when they are popped for use.
/// </summary>
using UnityEngine;
using System.Collections;

public class GDG_Physics : GDG_Main
{
	
	#region variables

		//Hidden variables
		[HideInInspector]
		public bool
				DL1Enabled,
				DL2Enabled,
				DL3Enabled;
		[HideInInspector]
		public int
				DL1BreakStrength = 150, //how hard you have to hit the object to break it
				DL2BreakStrength = 250,
				DL3BreakStrength = 500;
		[HideInInspector]
		public bool
				shrinkDL1Collider, //Scales the collider to prevent objects from blowing apart on instantiation
				shrinkDL2Collider,
				shrinkDL3Collider = false;
		[HideInInspector]
		public float
				shrinkColliderSize = 0.8f;			//How small to shrink the collider

		[HideInInspector]
		public float
				scaleTime = 0.125f;				//How fast to grow the collider to original size.

		[HideInInspector]
		public float
				chunkLifetime = 20;				//The amount of time the chunks will stay alive after being broken

		[HideInInspector]
		public Material
				outsideMaterial,
				insideMaterial;
		[HideInInspector]
		public bool
				passDownBreakage = false;			//Used to propogate breakage to lower levels when an object is exploded or broken externally. Passed child - master - child...

		[HideInInspector]
		public int	breakThoughLevel = 1;				//use this variable when breaking from an external script using the break methods, this way you can control how many levels deep the object will break
	
		[HideInInspector]
		public PhysicsController _physicsController;	//A reference to the Object_Controller (parent of DL0 objects)

		//Protected Variables
		protected int level = 0;						//Lets the PhysicsController_Child know if it's a DL0, DL1, DL2, or DL3
		protected Vector3 _finalScale = new Vector3 (1.0f, 1.0f, 1.0f);	//final scale size for cube colliders


		protected float _chunkMass = 1;					//Mass is set in the controller
		
		protected bool _deleteable = false;				//temp bool for chunk removal timer
		protected float removalTime;
		protected bool canBreak = false;				//Controlled by FPSController singleton. Prevents breaking when framerate is low
		protected bool removeMe = false;
		private int _stackTraceNumber;					//This is used to identify which stack an object resided in in the PhysicsControllerMaster stack array
		protected float _PassDownContactForce;			//Used to bring the force applied to one level down to the next when it breaks
	#endregion

		/// <summary>
		/// Same as the GDG main Start() class, but called first to prevent initialization issues in derived classes
		/// </summary>
		virtual public void Awake ()
		{
				_myTransform = transform;
		
				if (GetComponent<Rigidbody>() != null) {
						_myRigidbody = GetComponent<Rigidbody>();
				}
		}

		//sets the initial chunk properties at setup, before the chunk go onto the stack, 
		//so that we have fewer calculations at runtime when they are popped off
		public void SetInitialChunkProperties ()
		{
				DL1Enabled = _physicsController.DL1Enabled;
				DL1BreakStrength = _physicsController.DL1BreakStrength;
				DL2Enabled = _physicsController.DL2Enabled;
				DL2BreakStrength = _physicsController.DL2BreakStrength;
				DL3Enabled = _physicsController.DL3Enabled;
				DL3BreakStrength = _physicsController.DL3BreakStrength;
		
				chunkLifetime = _physicsController.chunkLifetime;
		
				if (_myTransform.GetComponent<Collider>() != null)
						_myTransform.GetComponent<Collider>().material = _physicsController.physicsMat;
		
				shrinkDL1Collider = _physicsController.shrinkDL1Collider;
				shrinkDL2Collider = _physicsController.shrinkDL2Collider;
				shrinkDL3Collider = _physicsController.shrinkDL3Collider;
		
				scaleTime = _physicsController.scaleTime;

				if (GetComponent<Collider>() != null) {
						GetComponent<Collider>().material = _physicsController.physicsMat;
				}
		
				breakThoughLevel = _physicsController.breakThoughLevel;

				SetMaterials ();
		}

		protected virtual void SetMaterials ()
		{

		}

		public virtual void SetChunkProperties ()
		{
				SetRemovalTime ();
		}
	
		virtual public void SetRemovalTime ()
		{
				removalTime = Time.time + chunkLifetime;
				_deleteable = true;
		}
    
		virtual public void BreakAndDestroy ()
		{
				transform.DetachChildren ();
				Destroy (gameObject);
		}
	
		virtual protected void CheckForRemoval ()
		{
				if (removeMe || Time.time > removalTime)
						BreakAndDestroy ();
		}
	
		/// <summary>
		/// Gets the chunk mass.
		/// split up the total mass of the block evenly amongst its children
		/// </summary>
		/// <returns>
		/// The chunk mass.
		/// </returns>
		virtual	public float GetChunkMass ()
		{
				int i = _myTransform.childCount;
		
		
				//prevent division by zero
				if (i == 0) {
						i = 1;
				}
				return _myRigidbody.mass / i;
		}

		public float GetImpactForce (Collision col)
		{
				if (col.gameObject.GetComponent<Rigidbody>() != null) {
						return Mathf.Pow (col.relativeVelocity.magnitude, 2) * col.gameObject.GetComponent<Rigidbody>().mass;
				} else {
						//Debug.Log("no rigidbody attached to collidier");
						return col.relativeVelocity.magnitude;
				}
		}

	#region gettersnsetters

		/// <summary>
		/// Gets or sets the stack trace number. Lets objects know which PhysicsController stack they belong to
		/// </summary>
		/// <value>The stack trace number.</value>
		public int StackTraceNumber {
				get{ return _stackTraceNumber;}
				set{ _stackTraceNumber = value;}
		}

		public PhysicsController _PhysicsController {
				get { return _physicsController;}
				set { _physicsController = value;}
		}

		public float PassDownContactForce {
				get { return _PassDownContactForce;}
				set { _PassDownContactForce = value;}
		}
	#endregion
}
