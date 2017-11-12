using UnityEngine;
using System.Collections;

public class ExplosionSphereController : GDG_Main
{
	private PhysicsController _PhysicsController;
	private const float RESTACKBUFFER = 1.0f;	//1 second is magic enough
	private float restackTime;

	void OnEnable()
	{
		restackTime = Time.time + RESTACKBUFFER;
	}

	void Update()
	{
		if(Time.time > restackTime)
		{
			RestackSphere();
		}
	}

		void FixedUpdate ()
		{
				if (_myTransform.localScale.x < _PhysicsController.FinalExplosionScale.x) {
						_myTransform.localScale = Vector3.Lerp (_myTransform.localScale,
			                                    _PhysicsController.FinalExplosionScale,
			                                    Time.deltaTime / _PhysicsController.explosionStrength);
				}
		}

		public PhysicsController PhysicsController {
				get { return _PhysicsController;}
				set { _PhysicsController = value;}
		}

		protected void RestackSphere ()
		{
		_PhysicsController.RestackExplosionSphere(_myTransform.gameObject);
		}
}
