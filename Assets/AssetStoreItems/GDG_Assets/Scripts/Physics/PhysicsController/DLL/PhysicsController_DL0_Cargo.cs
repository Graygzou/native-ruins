/// <summary>
/// Physics controller_ D l0_ cargo.
/// Allows us to spawn cargo when we break our DL0 object
/// </summary>
using UnityEngine;
using System.Collections;

public class PhysicsController_DL0_Cargo : PhysicsController_DL0 {

	public Transform _cargoObject;
	protected Transform _LocalCargoObject;
	public bool useLocalCargo = false;	//Set to true if you want to spawn a specific item as cargo
	protected int _CargoStackNum;

	float GetPassDownMass()
	{
		return _passDownMass = (_myRigidbody.mass - _cargoObject.GetComponent<Rigidbody>().mass) / _physicsController.breakableChunks_DL1.Length;
	}

	override protected void SpawnChild()
	{
		base.SpawnChild();
		SpawnCargo();
	}

	void SpawnCargo()
	{
		if(useLocalCargo)
		{
			_cargoObject = _LocalCargoObject;
		}

		else
		{
		//pop this off the controller stack
		_cargoObject = _physicsController.GetComponent<PhysicsController_Cargo>().PopCargo (_CargoStackNum);
		}

		_cargoObject.gameObject.SetActive (true);
		_cargoObject.transform.position = _myTransform.position;
		_cargoObject.transform.rotation = _myTransform.rotation;

		if(_cargoObject.GetComponent<Rigidbody>() != null)
		{
		_cargoObject.GetComponent<Rigidbody>().velocity = _myRigidbody.velocity;
		_cargoObject.GetComponent<Rigidbody>().angularVelocity = _myRigidbody.angularVelocity;
		}
	}

	public Transform LocalCargoObject
	{
		get {return _LocalCargoObject;}
		set {_LocalCargoObject = value;}
	}

	public int CargoStackNum
	{
		get {return _CargoStackNum;}
		set {_CargoStackNum = value;}
	}
}
