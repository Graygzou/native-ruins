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

public class PhysicsController_DL2_Master : PhysicsController_Master {

	
	/// <summary>
	/// Transfers the properties of the original block to the chunks
	/// </summary>
	override public void EnableChildren ()
	{
		instantiateSmallCollider = _physicsController.shrinkDL2Collider;
		base.EnableChildren();
		
	}
}
