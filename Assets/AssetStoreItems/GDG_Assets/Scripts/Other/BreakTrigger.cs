/// <summary>
/// 
/// 11/22/2013
/// Steve Peters
/// GameDevelopersGuild.com
/// Miami Fl
/// 
/// Test class to show how to call the break functions of Objects using the GDG breakable objects framework
/// 
/// </summary>
/// 
using UnityEngine;
using System.Collections;

public class BreakTrigger : MonoBehaviour {

	public PhysicsController_Child breakTarget;
	public BreakType breaktype = BreakType.breakStandard;

	private bool _CanTrigger = true;

	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Player")
		{
			Debug.Log("triggered by player");

			if(breakTarget != null && _CanTrigger)
			{
				if(breakTarget.gameObject.activeSelf)
				{
				if(breaktype == BreakType.breakStandard)
				{
					breakTarget.breakObject(true);
				}

				else if(breaktype == BreakType.breakForce)
				{
					breakTarget.breakObject(true, new Vector3(10,0,0));
				}

				else if(breaktype == BreakType.breakRotation)
				{
					breakTarget.breakObject(true, new Vector3(0,0,0), new Vector3(0,100,0));
				}

				else
				{
					breakTarget.explodeObject();
				}

				_CanTrigger = false;
				}
			}
		}
	}

	public enum BreakType
	{
		breakStandard, 
		breakForce,
		breakRotation,
		explode
	}
}
