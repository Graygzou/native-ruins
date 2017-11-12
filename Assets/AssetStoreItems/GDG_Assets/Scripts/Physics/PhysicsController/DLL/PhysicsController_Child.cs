/// <summary>
/// 
/// /// 11/1/2013
/// Steve Peters
/// GameDevelopersGuild.com
/// Miami Fl
/// 
/// Primary class for all the breakable chunks used in the scene
/// 
/// </summary>
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsController_Child : GDG_Physics
{
	public bool positionFrozen = false;
	public bool preventBreaking = false;		//used with fixed position chunks to keep them from breaking into lower levels
	public bool _FlipMaterials = false;			//flip material slots 1 and 2, a hack fix for wierd Blender imports
	public bool playerCanBreak = false;
	protected float _passDownMass;
	protected bool _breaker = false;				//special use variable that prevents multiple sub-objects from being intantiated when breaking object(multiple collisions issue)
	protected int breakStrength;
	protected bool scaleCol;
	protected PhysicsController_Master _DLMaster;
	protected AudioClip _AudioClipToPlay;		
		
	protected bool _CanExplode = true;

    protected colliderType colType = colliderType.nullCollider;     //keeps a reference to our collidertype
    private BoxCollider boxCol;                     //caches our box collider so the we don't need to use GetComponent in the scale collider method
    private SphereCollider sphereCol;               

	override protected void Start ()
	{
		base.Start ();
		base.SetInitialChunkProperties ();

        //set our collider type
        if (_myTransform.GetComponent<Collider>() != null)
        {
            if (_myTransform.GetComponent<Collider>().GetType() == (typeof(BoxCollider)))
            {
                colType = colliderType.box;
                boxCol = (BoxCollider)_myTransform.GetComponent(typeof(BoxCollider));
            }
            else if (_myTransform.GetComponent<Collider>().GetType() == typeof(SphereCollider))
            {
                colType = colliderType.sphere;
                sphereCol = (SphereCollider)_myTransform.GetComponent(typeof(SphereCollider));
            }
            else if (_myTransform.GetComponent<Collider>().GetType() == typeof(MeshCollider))
            {
                colType = colliderType.mesh;
            }
            else
            {
                Debug.Log("Collider Type not supported. Please submit bug report.");
                colType = colliderType.notSupported;
            }
        }

		//we need to add a slight delay to our lower levels break objects method so that they aren't denied the ability to break from 
		//the FPS controller singleton while FPS is low. Only called if we're breaking from an external script through the 
		//explode or break methods
		if (passDownBreakage) {
			Invoke ("BreakObjectHelper", 0.1f);
		}
	}
	
	virtual protected void Update ()
	{
		if (scaleCol) {
			ScaleCollider ();
		}
		
		if (_deleteable) {
			CheckForRemoval ();
		}
	}
	
	#region breaking methods

	
	/*
	 * These methods are used if you want to break an object using an outside script ie. explosion controller 
	 * or fracture by raycast
	 * */
	
	#region CallableFractureMethods
	
		
	/// <summary>
	/// Breaks the object. Useful for circumstances where the object needs to break but the collider will not interact
	/// with anything, such as magic or explosions. DLEnabled is passed from lower levels
	/// </summary>
	virtual public void breakObject (bool b)
	{
		if (b && canBreak) {
			passDownBreakage = true;
			_myRigidbody.isKinematic = false;
			_breaker = true;
			SpawnChild ();
		}
	}
	
	//breaks the object and adds a world force to it
	virtual public void breakObject (bool b, Vector3 force)
	{
		if (b && canBreak) {
			_myRigidbody.isKinematic = false;
			_myRigidbody.velocity += force;
			breakObject (b);
		}
	}
	
	//breaks the object and adds a world force and torque to it
	virtual public void breakObject (bool b, Vector3 force, Vector3 rotation)
	{
		if (b && canBreak) {
			_myRigidbody.isKinematic = false;
			_myRigidbody.angularVelocity += rotation;
			breakObject (b, force);
		}
	}

	void BreakObjectHelper ()
	{
		breakObject (true);
	}
	#endregion

	#region CallableExplosionMethods

	public void explodeObject ()
	{
		if (_CanExplode) {

			_physicsController.PopExplosionSphere (_myTransform.position);

			_myTransform.GetComponent<PhysicsController_Child> ().breakObject (true);

			if (_physicsController.explosionSound != null) {
				AudioSource.PlayClipAtPoint (_physicsController.explosionSound, _myTransform.position, _physicsController.audioVolume);
			}
			_CanExplode = false;
		}
	}

	#endregion

	//having the breaker prevents multiple instances of DL1 from being created
	virtual protected void OnCollisionEnter (Collision col)
	{
		if (!_breaker && canBreak && !preventBreaking) {

			if (_PhysicsController.CanGameObjectDestroyBreakableObject (col.gameObject)) {
				float collisionForce = GetImpactForce (col);
						
				//If the force is strong enough, break the block and average fps is above minimum framerate
				if ((collisionForce > breakStrength) || (_PassDownContactForce > breakStrength)) {
					_PassDownContactForce = collisionForce;
					_breaker = true;
					SpawnChild ();
				}
			}
		}
	}

    //special case for colliding with character controllers which require a trigger and cannot be used to calculate physics
    public void animationBreak(Collider col)
    {
        if (!_breaker && playerCanBreak && !preventBreaking) {
            _breaker = true;
            SpawnChild();
        }
    }


    //special case for colliding with character controllers which require a trigger and cannot be used to calculate physics
    virtual public void OnTriggerEnter (Collider col)
	{
		if (!_breaker && canBreak && playerCanBreak && !preventBreaking) {
			
			if (col.gameObject.tag.Equals ("Player")) {
				_breaker = true;
				SpawnChild ();
			}
		}
	}
	#endregion
	
	/// <summary>
	/// Spawns the child of the master object, passes the nessesary values to the rigidbody and kills itself
	/// scale is passed from DL0 to DL1 Master, and from DL2 Master to DL2 objects
	/// everywhere else scale is passed due to parent child relationship
	/// </summary>
	virtual protected void SpawnChild ()
	{
		_DLMaster.gameObject.SetActive (true);
        _DLMaster.GetTransform().localScale = _myTransform.localScale;

		_DLMaster.GetTransform().rotation = _myTransform.rotation;
        _DLMaster.GetTransform().position = _myTransform.position;

		_DLMaster.playerCanBreak = playerCanBreak;
		_DLMaster.GetRigidbody().velocity = _myRigidbody.velocity;
        _DLMaster.GetRigidbody().angularVelocity = _myRigidbody.angularVelocity;
		_DLMaster._PhysicsController = _physicsController;
        _DLMaster.GetRigidbody().mass = _myRigidbody.mass;
		_DLMaster.passDownBreakage = passDownBreakage;
		_DLMaster.PassDownContactForce = _PassDownContactForce;

		_DLMaster.EnableChildren ();
		BreakAndDestroy ();
	}
	
	public void ScaleCollider ()
	{
		
			if (colType == colliderType.box) {

                if (boxCol.size.x < _finalScale.x)
                {
                    boxCol.size = Vector3.Lerp(boxCol.size, _finalScale, Time.deltaTime * scaleTime);
				} else {
					scaleCol = false;
				}
            }
            else if (colType == colliderType.sphere)
            {
				if (sphereCol.radius < _finalScale.x) {
                    sphereCol.radius = Mathf.Lerp(sphereCol.radius, _finalScale.x, Time.deltaTime * scaleTime);
				} else {
					scaleCol = false;
				}
            }
            else if (colType == colliderType.mesh)
            {
				//MeshColliders don't scale";
				scaleCol = false;
			} else {
				Debug.Log ("Scale collider was called on a collider type that is not supported. Please submit bug report.");	
			}
		
	}
	
	public void PlayParticleSystem ()
	{
		if (Time.time > _physicsController.ParticleSystemDelayTime) {
			_physicsController.ParticleSystemDelayTime = Time.time + 0.5f;
			Transform go = Instantiate (_physicsController.breakParticleSystem, _myTransform.position,
			_myTransform.rotation) as Transform;
			Destroy (go.gameObject, _physicsController.particleSystemLifetime);
		}
	}

	public void PlayBreakingSound ()
	{
		if (FPSController_Singleton.Instance.CanPlayBreakingSound ()) {
			//Gets a random clip from the physics controller
			int audioClipNumber = (int)Mathf.Round (Random.Range (0, _physicsController.breakSounds.Length));
			_AudioClipToPlay = _physicsController.breakSounds [audioClipNumber];

            AudioSource.PlayClipAtPoint(_AudioClipToPlay, _myTransform.position, 100);//_physicsController.audioVolume);
		}
	}

	void SetMass (int cargoMass)
	{
		GetComponent<Rigidbody>().mass += cargoMass;
	}
	
	protected override void SetMaterials ()
	{
		base.SetMaterials ();

		if (_physicsController.overrideMaterials) {
			if (GetComponent<Renderer>() != null) {
				Material[] tempMats = new Material[1];
				;
				
				if (GetComponent<Renderer>().materials.Length == 1) {
					tempMats [0] = _physicsController.outsideMaterial;
				} else if (GetComponent<Renderer>().materials.Length == 2) {
					tempMats = new Material[2];
					if (_FlipMaterials) {
						tempMats [1] = _physicsController.outsideMaterial;
						tempMats [0] = _physicsController.insideMaterial;
					} else {
						tempMats [0] = _physicsController.outsideMaterial;
						tempMats [1] = _physicsController.insideMaterial;
					}
				} else if (GetComponent<Renderer>().materials.Length == 3) {
					tempMats = new Material[3];
					tempMats [0] = _physicsController.outsideMaterial;
					tempMats [1] = _physicsController.insideMaterial;
					tempMats [2] = _physicsController.outsideMaterial;
				}
				
				GetComponent<Renderer>().materials = tempMats;
			}
		}
	}

    protected enum colliderType
    {
        box,
        sphere,
        mesh,
        notSupported,
        nullCollider
    };
}
