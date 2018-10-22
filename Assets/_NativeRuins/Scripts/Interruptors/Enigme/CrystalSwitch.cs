using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSwitch : CutScene
{

    public Texture texture;

    // Use this for initialization
    void Start() {
        if (cameraCutScene != null)
            cameraCutScene.GetComponent<Camera>().enabled = false;
    }

    override public IEnumerator PlayCutSceneStart() {
        yield return new WaitForSeconds(1f);
        ActivateSwitch();
        yield return new WaitForSeconds(1f);
        StopCutScene();
        if (gameObject.GetComponent<SwitchObject>() != null)
        {
            // Activate all his children
            gameObject.GetComponent<SwitchObject>().ActivateChildren();
        }
    }

    // The switch does what he's meant for here.
    override protected void ActivateSwitch() {
        gameObject.GetComponent<Renderer>().material.mainTexture = texture;
    }
}
