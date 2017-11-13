using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemSwitch : Switch {

    // Use this for initialization
    void Start () {
        if (cameraCutScene != null)
            cameraCutScene.enabled = false;
    }

    public void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Activate();
        }
    }

    override public IEnumerator PlayCutScene()
    {
        yield return new WaitForSeconds(1f);
        ActivateSwitch();
        cameraCutScene.GetComponent<Animator>().Play("CutsceneOurs");
        yield return new WaitForSeconds(12f);
        StopCutScene();
        if (gameObject.GetComponent<SwitchObject>() != null) {
            // Activate all his children
            gameObject.GetComponent<SwitchObject>().ActivateChildren();
        }
    }

    protected override void ActivateSwitch() {
        //throw new System.NotImplementedException();
    }

}
