﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCutScene : Switch {

    private void OnTriggerEnter(Collider other) {
        if (other == GameObject.FindWithTag("Player").GetComponent<Collider>() && triggerZone) {
            if (loop) {
                Activate();
            } else if (!loop && !isActived) {
                Activate();
            }
        }
    }

    override public IEnumerator PlayCutSceneStart() {
        ActivateSwitch();
        if (cutSceneStart != null) {
            cameraCutScene.GetComponent<Animation>().clip = cutSceneStart;
            cameraCutScene.GetComponent<Animation>().Play();
            if (cutSceneMusic != null)
            {
                cameraCutScene.GetComponent<AudioSource>().clip = cutSceneMusic;
                cameraCutScene.GetComponent<AudioSource>().Play();
            }
            yield return new WaitForSeconds(cameraCutScene.GetComponent<Animation>().clip.length);
            StopCutScene();
            if (gameObject.GetComponent<SwitchObject>() != null) {
                // Activate all his children
                gameObject.GetComponent<SwitchObject>().ActivateChildren();
            }
        }
    }

    protected override void ActivateSwitch() {
        //throw new System.NotImplementedException();
    }
}