using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCutScene : CutScene
{
    /*
    public string nameCutScene;
    public AudioClip cutSceneMusic;
    public float PauseTime;
    public bool loop;
    public bool triggerZone;

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
        if (nameCutScene != "") {
            cameraCutScene.GetComponent<Animator>().SetBool("Animation", true);
            cameraCutScene.GetComponent<Animator>().Play(nameCutScene);
            if (cutSceneMusic != null)
            {
                cameraCutScene.GetComponent<AudioSource>().clip = cutSceneMusic;
                cameraCutScene.GetComponent<AudioSource>().Play();
            }
            yield return new WaitForSeconds(PauseTime);
            StopCutScene();
            if (gameObject.GetComponent<SwitchObject>() != null) {
                // Activate all his children
                gameObject.GetComponent<SwitchObject>().ActivateChildren();
            }
        }
    }

    protected override void ActivateSwitch() {
        //throw new System.NotImplementedException();
    }*/
}
