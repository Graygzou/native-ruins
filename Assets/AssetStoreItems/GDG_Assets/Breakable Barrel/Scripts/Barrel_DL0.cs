using UnityEngine;
using System.Collections;

public class Barrel_DL0 : PhysicsController_DL0_Cargo {

override public void BreakAndDestroy ()
{		
	//This is where I normally update my points control system with the value of the broken block
	base.BreakAndDestroy ();
}
}
