/// <summary>
/// Launcher.
/// 
/// 3/2/2013
/// Steve Peters
/// Game Developers Guild - Miami, FL
/// 
/// Allows us to launch projectiles at a wall. It preinstantiates and stores the projectiles in a 
/// stack to improve performance
/// </summary>
using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour
{
	
	public Rigidbody projectile;
	public Rigidbody explosiveProjectile;
	public float launchspeed = 50;
	public bool useExplodingProjectiles = false;
	
	private float _LaunchDelayTime = 0.0f;
	
	public int stackSize = 60;			
	public Transform launchHole1;
	public Transform launchHole2;
	
	private Stack _Projectiles;
	private Stack _ExplosiveProjectiles;
	private Transform _myTransform;
	
	// Use this for initialization
	void Start ()
	{
		_myTransform = transform;
		_Projectiles = new Stack();
		if(useExplodingProjectiles)
		{
		_ExplosiveProjectiles = new Stack();
		}

		for(int i = 0; i <  stackSize; i++)
		{
			Rigidbody tr = Instantiate (projectile, _myTransform.position, _myTransform.rotation) as Rigidbody;
			PushProjectile(tr);

			if(useExplodingProjectiles)
			{
			Rigidbody rr = Instantiate (explosiveProjectile, _myTransform.position, _myTransform.rotation) as Rigidbody;
			PushExplosiveProjectile(rr);
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(_Projectiles.Count > 0)
		{
			if(Time.time > _LaunchDelayTime)
		{	
			if (Input.GetButtonDown ("Fire1")) 
			{
				Rigidbody tr = PopProjectile();
				tr.gameObject.SetActive(true);
				tr.transform.position = launchHole1.position;
				tr.transform.rotation = launchHole1.rotation;
				tr.velocity = transform.TransformDirection (Vector3.forward * launchspeed);
				
			    tr = PopProjectile();
				tr.gameObject.SetActive(true);
				tr.transform.position = launchHole2.position;
				tr.transform.rotation = launchHole2.rotation;
				tr.velocity = transform.TransformDirection (Vector3.forward * launchspeed);
					
				_LaunchDelayTime = Time.time + 0.5f;
			}
		}	
		}

		if(useExplodingProjectiles)
		{
		if(_ExplosiveProjectiles.Count > 0)
		{
			if(Time.time > _LaunchDelayTime)
			{	
				if (Input.GetButtonDown ("Fire2")) 
				{
					Rigidbody tr = PopExplosiveProjectile();
					tr.gameObject.SetActive(true);
					tr.transform.position = launchHole1.position;
					tr.transform.rotation = launchHole1.rotation;
					tr.velocity = transform.TransformDirection (Vector3.forward * launchspeed);
					
					tr = PopExplosiveProjectile();
					tr.gameObject.SetActive(true);
					tr.transform.position = launchHole2.position;
					tr.transform.rotation = launchHole2.rotation;
					tr.velocity = transform.TransformDirection (Vector3.forward * launchspeed);
					
					_LaunchDelayTime = Time.time + 0.5f;
				}
			}	
		}
		}
	}
	
	public void PushProjectile(Rigidbody x)
	{
		x.gameObject.SetActive(false);
	 	_Projectiles.Push(x);
	}
	
	public Rigidbody PopProjectile()
	{
		return (Rigidbody)_Projectiles.Pop();
	}

	public void PushExplosiveProjectile(Rigidbody x)
	{
		x.gameObject.SetActive(false);
		_ExplosiveProjectiles.Push(x);
	}
	
	public Rigidbody PopExplosiveProjectile()
	{
		return (Rigidbody)_ExplosiveProjectiles.Pop();
	}
}
