using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSwitch : Switch {

    private bool hasBeenActivated;
    private Transform colliderTransform;

    void Awake() {
        enabled = false;
        hasBeenActivated = false;
    }

    void OnCollisionEnter(Collision other) {
        if (enabled && other.collider.tag != "Player" && other.collider.tag != "Animal") {
            transform.position = other.contacts[0].point;
            GetComponent<Rigidbody>().isKinematic = true;
        }
        if (other.collider.tag == "Animal") {
            colliderTransform = other.transform;
            ActivateSwitch();
        }
    }

    protected override void ActivateSwitch() {
        if (!hasBeenActivated) {
            hasBeenActivated = true;
            colliderTransform.gameObject.GetComponent<AgentProperties>().takeDamages(15f);
        }
    }
}
