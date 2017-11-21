using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSwitch : Switch {



    void Awake() {
        enabled = false;
    }

    void OnCollisionEnter(Collision other) {
        if (enabled && other.collider.tag != "Player" && other.collider.tag != "Animal") {
            transform.position = other.contacts[0].point;
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    protected override void ActivateSwitch() {
        GetComponent<Rigidbody>().isKinematic = true;
    }
}
