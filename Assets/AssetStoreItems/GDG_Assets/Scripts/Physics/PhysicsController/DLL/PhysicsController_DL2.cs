/// <summary>
/// 
/// /// 11/1/2013
/// Steve Peters
/// GameDevelopersGuild.com
/// Miami Fl
/// 
/// This class also spawns the DL3 objects
/// </summary>
using UnityEngine;
using System.Collections;

public class PhysicsController_DL2 : PhysicsController_Child
{	
	protected override void Start ()
	{
		level = 2;
		base.Start ();

		breakStrength = DL3BreakStrength;		//have to set the break strength per breakable type
		scaleCol = shrinkDL2Collider;			//prevent unnessary shrinnking calculations
	}
	
	protected override void Update ()
	{
		base.Update ();
		removeMe = (FPSController_Singleton.Instance.removalState == FPSController_Singleton.removabilityState.removeDL2 ||
			FPSController_Singleton.Instance.removalState == FPSController_Singleton.removabilityState.removeDL3);

		if (!preventBreaking) {
			canBreak = FPSController_Singleton.Instance.canBreakDL2 && DL3Enabled;
		}
	}
	
	override public void breakObject (bool b)
	{
		base.breakObject (DL3Enabled && (breakThoughLevel >= level));
	}
	
	override public void breakObject (bool b, Vector3 force)
	{
		base.breakObject (DL3Enabled && (breakThoughLevel >= level), force);
	}
	
	//breaks the object and adds a world force and torque to it
	override public void breakObject (bool b, Vector3 force, Vector3 rotation)
	{
		base.breakObject (DL3Enabled && (breakThoughLevel >= level), force, rotation);
	}
	
/*
 * DL3 objects are instantiated as children of the DL2 object
 * */
	override protected void SpawnChild ()
	{	
		if (_physicsController.playDL2BreakSound) {
			PlayBreakingSound ();
		}
		
		if (_physicsController.playDL2ParticleSystem) {
			PlayParticleSystem ();
		}
	
		foreach (Transform child in _myTransform) {

            //cache the child object
            GDG_Physics gChild = (GDG_Physics)child.GetComponent(typeof(GDG_Physics));

            gChild.gameObject.SetActive(true);
            gChild.GetRigidbody().velocity = _myRigidbody.velocity;
            gChild.GetRigidbody().angularVelocity = _myRigidbody.angularVelocity;
            gChild.GetRigidbody().mass = GetChunkMass();

            gChild._PhysicsController = _PhysicsController;
            gChild.SetChunkProperties();
			
			//DL3 children automatically recieve the scale of the parent
		}
		
		BreakAndDestroy ();
	}
}
