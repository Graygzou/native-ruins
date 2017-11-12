using UnityEngine;
using System.Collections;

public class projectile_Explosive : Projectile
{

		public Transform explosion;

		protected void OnCollisionEnter (Collision col)
		{
		
				//blow some shit up
				//Debug.Log("kablamo");
			
				if (col.gameObject.tag == "Breakable") {
						col.gameObject.GetComponent<PhysicsController_Child> ().explodeObject ();
						Restack ();
				}
		}

		override protected void Restack ()
		{
				launcher.PushExplosiveProjectile (_myRigidbody);
		}
	
}
