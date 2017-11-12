using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSwitch : Switch {

    public Texture texture;

    // Use this for initialization
    void Start () {
    }
    override public void Activate() {
        gameObject.GetComponent<Renderer>().material.mainTexture = texture;
    }
}
