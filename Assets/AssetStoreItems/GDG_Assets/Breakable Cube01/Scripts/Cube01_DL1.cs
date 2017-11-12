using UnityEngine;
using System.Collections;

public class Cube01_DL1 : PhysicsController_DL1 {
override public void BreakAndDestroy ()
{
	//This is where I normally update my points control system with the value of the broken block
	base.BreakAndDestroy ();
}
}
