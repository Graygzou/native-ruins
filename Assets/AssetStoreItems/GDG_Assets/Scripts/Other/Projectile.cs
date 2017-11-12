/// Steve Peters
/// 10/9/2012
/// Restacks the projectile after a set time
/// </summary>
using UnityEngine;
using System.Collections;

public class Projectile : GDG_Main
{

	public float lifetime = 10;
	private float temptime;
	protected Launcher launcher;

	void OnEnable ()
	{
		temptime = Time.time + lifetime;
		launcher = GameObject.FindGameObjectWithTag ("Launcher").GetComponent<Launcher>() ;
	}
	
	void Update ()
	{
		// Sends it back to the stack when lifetime ends.
		if (Time.time > temptime) {
			Restack();
		}
	}

	 protected virtual void Restack()
	{
		launcher.PushProjectile (_myRigidbody);
	}

	
}
