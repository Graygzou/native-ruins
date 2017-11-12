using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSwitch : Switch {

    public Texture texture;

    // Use this for initialization
    void Start () {
    }

    override public void Activate() {
        // If the switch got a camera, switch to activate it
        if (cameraCutScene != null) {
            StartCutScene();
            StartCoroutine("PlayCutScene");
            StopCutScene();
        }
    }

    IEnumerator PlayCutScene()
    {
        yield return new WaitForSeconds(3f);
        gameObject.GetComponent<Renderer>().material.mainTexture = texture;
        yield return new WaitForSeconds(3f);
        if (gameObject.GetComponent<SwitchObject>() != null) {
            Debug.Log("childrens");
            //gameObject.GetComponent<SwitchObject>().ActivateChildren();
        }
            
    }
}
