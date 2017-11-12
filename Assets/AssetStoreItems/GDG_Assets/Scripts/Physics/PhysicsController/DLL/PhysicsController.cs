/// <summary>
/// 
/// /// 11/1/2013
/// Steve Peters
/// GameDevelopersGuild.com
/// Miami Fl
/// 
/// Physics controller.
/// Physics controller class for breakable objects
/// The physics controller is a generic class which stores all of the objects variables, but also
/// contains stacks of the breakable child objects
/// 
/// A reference to the physics controller is passed to the DL0 object(the main breakable object), and all relevant variables 
/// are passed from parent to child. The sub child objects((DL1 )child of DL1 Master)  are responsible for 
/// instantiating the lower level chunks and passing the nesessary variables to it./// </summary>
using UnityEngine;
using System.Collections;

public class PhysicsController : GDG_Physics
{
	[HideInInspector]
	public float
				objectMass = 250;				//Only need to set one, the other block masses are relative to this one

	[HideInInspector]
	public bool
				overrideMass,
				overrideMaterials,
				playDL0ParticleSystem,
				playDL1ParticleSystem,
				playDL2ParticleSystem,
				playDL0BreakSound,
				playDL1BreakSound,
				playDL2BreakSound = false;
	[HideInInspector]
	public PhysicMaterial
				physicsMat;
	[HideInInspector]
	public Transform
				breakParticleSystem;
	[HideInInspector]
	public float
				particleSystemLifetime = 1.0f;
	[HideInInspector]
	private float
				_ParticleSystemDelayTime = 0;
	
	//Had to remove typecasting to Physics_Controller_DLX_Master since Unity refuses to allow you to assign a prefab as anything other than a GameObject
	[HideInInspector]
	public PhysicsController_DL1_Master[] breakableChunks_DL1;
	[HideInInspector]
	public PhysicsController_Master[] breakableChunks_DL2;
	private Stack[] stacks;

	//Holds all of the breaking sounds for this type of object. A check must be done against the FPSController_Singleton to see 
	//if it's allowed to play its sound due to the timebuffer.
	[HideInInspector]
	public AudioClip[]
				breakSounds;
	[HideInInspector]
	public float
				audioVolume = 1.0f;

	//cargo
	[HideInInspector]
	public Transform[]
				cargoObjects;
	protected Stack[] cargoStack;
	protected Transform _tempCargo;

	//explosions properties
	//Holds the spheres that we will use to blow apart objects in order to simulate explosions
	private Stack _ExplosionSpheres;
	protected Vector3 _finalExplosionScale = new Vector3 (5, 5, 5);		//Sets the final size of the explosion sphere, larger makes for more destruction

	[HideInInspector]
	public float
		explosionStrength = 0.05f;	//controls the rate that the explosionsphere is scaled up from zero, and thus the strength of the explosion
	
	[HideInInspector]
	public AudioClip
				explosionSound;
	[HideInInspector]
	public string[] tagArray;

	override public void Awake ()
	{		
		base.Awake ();
		
		// Prevent lower levels from instantiating when unnescessary.
		if (DL1Enabled == false) {
			DL2Enabled = false;
			DL3Enabled = false;
		} else if (DL2Enabled == false)
			DL3Enabled = false;

		
		//get the needed size of the stacks array and populate it with new stacks
		int length = breakableChunks_DL1.Length + breakableChunks_DL2.Length;// + breakableChunks_DL3.Length;
		stacks = new Stack[length];
		for (int t = 0; t < length; t++) {
			stacks [t] = new Stack ();
		}

		SetProperties ();

		//stack sopme explosion spheres. 4 should be enough, but we'll include an instantiate in the popexplosionsphere method
		_ExplosionSpheres = new Stack ();
		for (int i = 0; i < 4; i++) {
			CreateExplosionSphere ();
		}
	}

	virtual  protected void  SetProperties ()
	{
		for (int j = 0; j < _myTransform.childCount; j++) {
            PhysicsController_DL0 child = (PhysicsController_DL0)_myTransform.GetChild(j).GetComponent(typeof(PhysicsController_DL0));
            child._physicsController = this;
			
			if (overrideMass) {
				child.GetComponent<Rigidbody>().mass = objectMass;
			}

			if (DL1Enabled) {
				for (int i = 0; i < breakableChunks_DL1.Length; i++) {
					//instantiaite DL1Masters
					PhysicsController_DL1_Master go = Instantiate (breakableChunks_DL1 [i], _myTransform.position, _myTransform.rotation) as PhysicsController_DL1_Master;
					PushStack (go, i);
				}
				
			}
			
			//Instantiate DL2 Masters
			if (DL2Enabled) {
				for (int i = 0; i < breakableChunks_DL2.Length; i++) {
					//instantiaite DL1Masters
					PhysicsController_DL2_Master go = Instantiate (breakableChunks_DL2 [i], _myTransform.position, _myTransform.rotation) as PhysicsController_DL2_Master;
					PushStack (go, i + breakableChunks_DL1.Length);
				}
			}
		}
	}
		
	public bool CanGameObjectDestroyBreakableObject (GameObject go)
	{
		for (int i = 0; i < tagArray.Length; i++) {
			if (go.tag.Equals (tagArray [i])) {
				return false;
			}
		}
		return true;
	}

	
		#region 
	//Stack control methods
	public void PushStack (PhysicsController_Master x, int stackNumber)
	{
		x.GetComponent<PhysicsController_Master> ()._PhysicsController = this;
		x.SetInitialChunkProperties ();
		x.gameObject.SetActive (false);
		x.StackTraceNumber = stackNumber;
		stacks [stackNumber].Push (x);
	}
	
	public PhysicsController_Master PopStack (int stackNumber)
	{
		return (PhysicsController_Master)stacks [stackNumber].Pop ();
	}
	

	#endregion
	

	
	public float ParticleSystemDelayTime {
		get { return _ParticleSystemDelayTime; }
		set { _ParticleSystemDelayTime = value; }
	}

	#region breakableMethods

	void CreateExplosionSphere ()
	{
		GameObject go = (GameObject)Instantiate (Resources.Load ("ExplosionSphere", typeof(GameObject)));
		go.GetComponent<ExplosionSphereController> ().PhysicsController = this;
		//go.tag = "Explosive";
		go.gameObject.SetActive (false);
		_ExplosionSpheres.Push (go);
	}

	public void PopExplosionSphere (Vector3 position)
	{
		if (_ExplosionSpheres.Count == 0) {
			//stack is empty, make a new sphere
			CreateExplosionSphere ();
		}

		GameObject explosionSphere = (GameObject)_ExplosionSpheres.Pop ();
		explosionSphere.gameObject.SetActive (true);
		explosionSphere.transform.localScale = new Vector3 (0, 0, 0);
		explosionSphere.transform.position = position;
	}

	public void RestackExplosionSphere (GameObject sphere)
	{
		sphere.SetActive (false);
		_ExplosionSpheres.Push (sphere);
	}

	#endregion

	public Vector3 FinalExplosionScale {
		get { return _finalExplosionScale;}
		set { _finalExplosionScale = value;}
	}
}

