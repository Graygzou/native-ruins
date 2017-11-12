/// <summary>
/// 
/// /// 11/1/2013
/// Steve Peters
/// GameDevelopersGuild.com
/// Miami Fl
/// 
/// </summary>
using UnityEngine;
using System.Collections;



public class PhysicsController_DL1 : PhysicsController_Child {
		
	protected override void Start ()
	{
		level = 1;
		base.Start ();

		breakStrength = DL2BreakStrength;		//have to set the break strength per breakable type
		scaleCol = shrinkDL1Collider;			//prevent unnessary shrinnking calculations
	}
	
	protected override void Update ()
	{
		base.Update ();
		removeMe = FPSController_Singleton.Instance.removalState == FPSController_Singleton.removabilityState.removeDL1;

		if(!preventBreaking)
		{
		canBreak = FPSController_Singleton.Instance.canBreakDL1 && DL2Enabled;
		}
	}

	override public void breakObject(bool b)
	{
		base.breakObject(DL2Enabled && (breakThoughLevel >= level));
	}
	
	override public void breakObject(bool b, Vector3 force)
	{
		base.breakObject(DL2Enabled && (breakThoughLevel >= level), force);
	}
	
	//breaks the object and adds a world force and torque to it
	override public void breakObject(bool b, Vector3 force, Vector3 rotation)
	{
		base.breakObject(DL2Enabled && (breakThoughLevel >= level), force, rotation);
	}
	
	override protected void SpawnChild()
	{
		if(_physicsController.playDL1BreakSound)
		{
			PlayBreakingSound();
		}
		
		if(_physicsController.playDL1ParticleSystem)
		{
			PlayParticleSystem();
		}

		//Spawn all of the required DL2 chunks from the stack
		_DLMaster = _physicsController.PopStack(StackTraceNumber + _physicsController.breakableChunks_DL1.Length);
		base.SpawnChild();
	}
}	

