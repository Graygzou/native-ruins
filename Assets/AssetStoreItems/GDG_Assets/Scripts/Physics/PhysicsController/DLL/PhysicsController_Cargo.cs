/// <summary>
/// Physics controller_ cargo.
/// Extends the physics controller class so that we can use cargo
/// </summary>
using UnityEngine;
using System.Collections;

public class PhysicsController_Cargo : PhysicsController
{
	override public void Awake ()
	{		
		cargoStack = new Stack[cargoObjects.Length];
		
		for (int t = 0; t < cargoObjects.Length; t++) {
			cargoStack [t] = new Stack ();
		}

		base.Awake ();
	}

	override  protected void  SetProperties ()
	{
		//base.SetProperties ();
		for (int j = 0; j < _myTransform.childCount; j++) {
			PhysicsController_DL0_Cargo child = (PhysicsController_DL0_Cargo)_myTransform.GetChild (j).GetComponent(typeof(PhysicsController_DL0_Cargo));
			child._PhysicsController = this;
			
			if (overrideMass) {
				child.GetComponent<Rigidbody>().mass = objectMass;
			}
			
			if (DL1Enabled) {
				for (int i = 0; i < breakableChunks_DL1.Length; i++) {
					//instantiaite DL1Masters
					PhysicsController_DL1_Master go = Instantiate (breakableChunks_DL1 [i], _myTransform.position, _myTransform.rotation) as PhysicsController_DL1_Master;
					PushStack (go, i);
				}
				
			}
			
			//Instantiate DL2 Masters
			if (DL2Enabled) {
				for (int i = 0; i < breakableChunks_DL2.Length; i++) {
					//instantiaite DL1Masters
					PhysicsController_DL2_Master go = Instantiate (breakableChunks_DL2 [i], _myTransform.position, _myTransform.rotation) as PhysicsController_DL2_Master;
					PushStack (go, i + breakableChunks_DL1.Length);
				}
			}
			StackCargo (child, j);
		}
	}

	protected void StackCargo (PhysicsController_DL0_Cargo child, int randomSeed)
	{
		//protect against an empty array
		if (! (cargoObjects.Length == 0)) {
			if (child.useLocalCargo) {
				child.LocalCargoObject = Instantiate (child._cargoObject, _myTransform.position, _myTransform.rotation) as Transform;
				//set the mass of the DL0 object to include itself + the weight of the cargo
				if (child.LocalCargoObject.GetComponent<Rigidbody>() != null) {
					child.GetComponent<Rigidbody>().mass += child.LocalCargoObject.GetComponent<Rigidbody>().mass;
				}

				child.LocalCargoObject.gameObject.SetActive (false);
			} else {
				//the max side of the random range is exclusive, while the min size is inclusive
				Random.seed = (int)(randomSeed * (1000 * Time.realtimeSinceStartup));
				int randomCargoNum = Random.Range (0, cargoObjects.Length);
				_tempCargo = Instantiate (cargoObjects [randomCargoNum], _myTransform.position, _myTransform.rotation) as Transform;
			
				PushCargo (_tempCargo, randomCargoNum);
				child.GetComponent<PhysicsController_DL0_Cargo> ().CargoStackNum = randomCargoNum;

				//set the mass of the DL0 object to include itself + the weight of the cargo
				child.GetComponent<Rigidbody>().mass += _tempCargo.GetComponent<Rigidbody>().mass;

			}
		} else {
			Debug.Log ("Need to add cargo objects to array");
		}
	}

	public void PushCargo (Transform x, int stackNumber)
	{
		x.gameObject.SetActive (false);
		cargoStack [stackNumber].Push (x);
	}
	
	public Transform PopCargo (int stackNumber)
	{
				
		if (stackNumber > cargoStack.Length - 1) {
			Debug.Log ("Add cargo to the CargoObjects array and ensure no slots are empty. If you want to instantiate nothing, use the included null cargo prefab");
			return null;
		} else {
			Transform tr = (Transform)cargoStack [stackNumber].Pop ();
			tr.gameObject.SetActive (true);		
			return tr;
		}
	}
}
