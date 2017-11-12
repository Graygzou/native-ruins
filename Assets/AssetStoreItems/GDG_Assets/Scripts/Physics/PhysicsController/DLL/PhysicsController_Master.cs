/// <summary>
/// 
/// /// 11/1/2013
/// Steve Peters
/// GameDevelopersGuild.com
/// Miami Fl
/// 
/// This object exists solely as a placeholder. It is spawned, spawns its DL subchunks as children, assigns them thier needed variables
/// and kills itself
/// </summary>
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsController_Master : GDG_Physics
{
	[HideInInspector]
	public float chunkMass;							//The mass of the object

	[HideInInspector]
	public bool instantiateSmallCollider = false;	//Shrinks the collider for the sub-chunk to prevent crossed colliders

	protected Transform _DestructionLevel3;
	public bool playerCanBreak = false;
		
	override protected void Start ()
	{
		base.Start();
		SetChunkProperties ();
	}
	
	virtual public void EnableChildren ()
	{
		int i = 0;
		
		foreach (Transform child in _myTransform) {

            //cache the child object since we'll be using it alot
            PhysicsController_Child pChild = (PhysicsController_Child)child.GetComponent(typeof(PhysicsController_Child));

			if (instantiateSmallCollider) {
				InstantiateSmallCollider(child);
			}

			if (child.GetComponent<PhysicsController_Child>().positionFrozen) {
				child.GetComponent<Rigidbody>().isKinematic = child.GetComponent<PhysicsController_Child>().positionFrozen;
			}

			else if (child.GetComponent<PhysicsController_Child>().preventBreaking) 
			{
				child.GetComponent<PhysicsController_Child>().preventBreaking = true;
			}
			
			SetChildVariables (pChild, i);
			i++;
		}
		BreakAndDestroy();
	}
	
	virtual public void SetChildVariables (PhysicsController_Child pChild, int i)
	{
		pChild.gameObject.SetActive (true);

       

		pChild.GetComponent<Rigidbody>().mass = GetChunkMass ();

        if (!pChild.positionFrozen)
		{
            pChild.GetRigidbody().velocity = _myRigidbody.velocity;
            pChild.GetRigidbody().angularVelocity = _myRigidbody.angularVelocity;
		}
        pChild.playerCanBreak = playerCanBreak;
        pChild._PhysicsController = _physicsController;
        pChild.SetChunkProperties();
        pChild.StackTraceNumber = StackTraceNumber;
        pChild.passDownBreakage = passDownBreakage;
        pChild.PassDownContactForce = _PassDownContactForce;

        if (instantiateSmallCollider && pChild.GetComponent<Collider>() != null)
        {
			if(GetComponent<Collider>() != null)
		{
			if(instantiateSmallCollider)
			{
                InstantiateSmallCollider(pChild.transform);
			}
		}
		}
	}
	
	void InstantiateSmallCollider(Transform child)
	{
		if(child.GetComponent<Collider>().GetType() == (typeof(BoxCollider)))
		{
		Vector3 boxSize = child.GetComponent<BoxCollider> ().size;
			_finalScale = child.GetComponent<BoxCollider> ().size;
			child.GetComponent<BoxCollider> ().size = new Vector3 (boxSize.x * _physicsController.shrinkColliderSize,
				boxSize.y * _physicsController.shrinkColliderSize, boxSize.z * _physicsController.shrinkColliderSize);
		}
		
		else if(child.GetComponent<Collider>().GetType() == typeof(SphereCollider))
		{
            float temp = child.GetComponent<SphereCollider>().radius;
            _finalScale = Vector3.one * temp;

            child.GetComponent<SphereCollider>().radius = shrinkColliderSize;
		}
		
		else if(child.GetComponent<Collider>().GetType() == typeof(MeshCollider))
		{
			
		}
		
		else
		{
		Debug.Log("Collider Type not supported. Please submit bug report.");	
		}
	}

}
