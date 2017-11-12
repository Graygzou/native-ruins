/// <summary>
/// 
/// /// 11/1/2013
/// Steve Peters
/// GameDevelopersGuild.com
/// Miami Fl
/// 
/// Primary class for all the DL0 objects used in the scene. These are in the scene when it is loaded
/// </summary>
using UnityEngine;
using System.Collections;

public class PhysicsController_DL0 : PhysicsController_Child {

	override protected void Start () {
		level = 0;
		//on start all DL0 objects are expected to be children of a physics controller
		if(_physicsController == null)
		_physicsController = (PhysicsController)_myTransform.parent.gameObject.GetComponent(typeof(PhysicsController));

		base.Start();
		breakStrength = DL1BreakStrength;		//have to set the break strength per breakable type
	}
	
	//prevent parent class from removing the DL0
		protected override void Update ()
	{
		//base.Update ();
		if(!preventBreaking)
		{
		canBreak = FPSController_Singleton.Instance.canBreakDL1 && DL1Enabled;
		}
	}
	
	override public void breakObject(bool b)
	{
		base.breakObject(DL1Enabled && (breakThoughLevel >= level));
	}
	
	override public void breakObject(bool b, Vector3 force)
	{
		base.breakObject(DL1Enabled && (breakThoughLevel >= level), force);
	}
	
	//breaks the object and adds a world force and torque to it
	override public void breakObject(bool b, Vector3 force, Vector3 rotation)
	{
		base.breakObject(DL1Enabled && (breakThoughLevel >= level), force, rotation);
	}
	
	//overriden to prevent children fom being detached
	override public void BreakAndDestroy ()
	{
		gameObject.SetActive(false);
	}
	
	override protected void SpawnChild()
	{
		//split the mass evenly between the DL1 chunks
		_passDownMass = GetPassDownMass();
		
		//Instantiate DL1 chunks
		for(int i = 0; i< _physicsController.breakableChunks_DL1.Length; i++)
		{
			_DLMaster = _physicsController.PopStack (i);
			_myRigidbody.mass = _passDownMass;
			_DLMaster.transform.localScale = _myTransform.localScale;
			base.SpawnChild ();

		}

		if(_physicsController.playDL0BreakSound)
		{
			PlayBreakingSound();
		}
		
		if(_physicsController.playDL0ParticleSystem)
		{
			PlayParticleSystem();
		}
		
		base.SpawnChild();
	}

	float GetPassDownMass()
	{
		return _myRigidbody.mass / _physicsController.breakableChunks_DL1.Length ;
	}
}
