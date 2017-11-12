/// <summary>
/// 
/// 11/1/2013
/// Steve Peters
/// GameDevelopersGuild.com
/// Miami Fl
/// 
/// </summary>
using UnityEngine;
using System.Collections;

//caches our most used local variables
public class GDG_Main : MonoBehaviour {
	
	protected Transform _myTransform;
	protected Rigidbody _myRigidbody;
	
	// Use this for initialization
	protected virtual void Start () 
	{
		
		_myTransform = transform;
		
		if(GetComponent<Rigidbody>() != null)
		{
			_myRigidbody = GetComponent<Rigidbody>();
		}
		else
		{
			//Debug.Log("Rigid Body Is Null " + this.name);
		}
	}
	
	public void ResetRigidbody()
	{
		_myRigidbody = GetComponent<Rigidbody>();
	}
	
	public void SetRigidBody(Rigidbody newBody)
	{
		_myRigidbody = newBody;	
	}
	public Rigidbody GetRigidbody()
	{
		return _myRigidbody;
	}
	
	public Transform GetTransform()
	{
		return _myTransform;
	}
}
