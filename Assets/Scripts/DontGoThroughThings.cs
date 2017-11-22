using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//private LayerMask layerMask; //make sure we aren't in this layer 
//private float skinWidth = 0.1f; //probably doesn't need to be changed 
//private float minimumExtent; 
//private float partialExtent; 
//private float sqrMinimumExtent; 
//private Vector3 previousPosition; 
//private RigidBody myRigidbody; 

//public class DontGoThroughThings : MonoBehaviour {

//	// Use this for initialization
//	void Awake () {
//        myRigidbody = rigidbody;
//        previousPosition = myRigidbody.position;
//        minimumExtent = Mathf.Min(Mathf.Min(collider.bounds.extents.x, collider.bounds.extents.y), collider.bounds.extents.z);
//        partialExtent = minimumExtent * (1.0 - skinWidth);
//        sqrMinimumExtent = minimumExtent * minimumExtent;
//    }
	
//	// Update is called once per frame
//	void FixedUpdate () {
//        //have we moved more than our minimum extent? 
//        Vector3 movementThisStep = myRigidbody.position - previousPosition;
//        float movementSqrMagnitude = movementThisStep.sqrMagnitude;
//        if (movementSqrMagnitude > sqrMinimumExtent)
//        {
//            float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
//            RayCastHit hitInfo;
//            //check for obstructions we might have missed 
//            if (Physics.Raycast(previousPosition, movementThisStep, hitInfo, movementMagnitude, layerMask.value))
//                myRigidbody.position = hitInfo.point - (movementThisStep / movementMagnitude) * partialExtent;
//        }
//        previousPosition = myRigidbody.position;

//    }
//}
