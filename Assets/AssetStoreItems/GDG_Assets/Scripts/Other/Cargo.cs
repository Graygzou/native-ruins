using UnityEngine;
using System.Collections;

public class Cargo : GDG_Main
{
	
	public bool scaleBoxCol = false;
	public bool scaleSphereCollider = false;	//one or the other
	public int lifetime = 10;
	public float scaleTime = 10.0f;
	public float shrinkColliderSize = .125f;
	private Vector3 _finalBoxScale;
	private float _FinalSphereRadius;
	
	// Use this for initialization
	protected override void Start ()
	{
		
		base.Start ();
		Destroy (_myTransform.gameObject, lifetime);
		
		if (scaleBoxCol) {
			_finalBoxScale = _myTransform.GetComponent<BoxCollider> ().size;
			
		_myTransform.GetComponent<BoxCollider> ().size = new Vector3 (_finalBoxScale.x * shrinkColliderSize,
				_finalBoxScale.y * shrinkColliderSize, _finalBoxScale.z * shrinkColliderSize);
		}
		
		else if (scaleSphereCollider) {
			_FinalSphereRadius = _myTransform.GetComponent<SphereCollider> ().radius;
			_myTransform.GetComponent<SphereCollider> ().radius *= shrinkColliderSize;
		}
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (scaleBoxCol) {
			ScaleBoxCollider ();
		} else if (scaleSphereCollider) {
			ScaleSphereCollider ();
		}
	}
	
	public void ScaleBoxCollider ()
	{
		if (_myTransform.GetComponent<Collider>() != null) {
			if (_myTransform.GetComponent<BoxCollider> ().size.x < _finalBoxScale.x) {
				_myTransform.GetComponent<BoxCollider> ().size = Vector3.Lerp (_myTransform.GetComponent<BoxCollider> ()
				.size, _finalBoxScale, Time.deltaTime * scaleTime);
			}
		}
	}
	
	public void ScaleSphereCollider ()
	{
		if (_myTransform.GetComponent<Collider>() != null) {
			if (_myTransform.GetComponent<SphereCollider> ().radius < _FinalSphereRadius) {
				_myTransform.GetComponent<SphereCollider> ().radius = Mathf.Lerp (_myTransform.GetComponent<SphereCollider> ()
				.radius, _FinalSphereRadius, Time.deltaTime * scaleTime);
			}
		}
	}
}
