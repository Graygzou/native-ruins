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

public class PhysicsController_DL3 : PhysicsController_Child
{
	
	protected override void Start ()
	{
		base.Start ();
		//other chunk properties for DL3 are set by the DL2 object
		scaleCol = shrinkDL3Collider;			//prevent unnessary shrinking calculations
	}
	
	protected override void Update ()
	{
		base.Update ();
		removeMe = (FPSController_Singleton.Instance.removalState == FPSController_Singleton.removabilityState.removeDL1 ||
			FPSController_Singleton.Instance.removalState == FPSController_Singleton.removabilityState.removeDL2 ||
			FPSController_Singleton.Instance.removalState == FPSController_Singleton.removabilityState.removeDL3);
		//no need to check the breakability of level 3
	}
}
