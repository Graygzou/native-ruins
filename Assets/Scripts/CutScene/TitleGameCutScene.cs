using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleGameCutScene : Switch {

    public AnimationClip cutScene;
    public AudioClip cutSceneMusic;

    protected override void ActivateSwitch() {
        // Setting up
        GameObject.Find("ThirdCutSceneCamera").GetComponent<Camera>().enabled = false;
        cameraCutScene = GameObject.Find("ForthCutSceneCamera").GetComponent<Camera>();
        cameraCutScene.enabled = true;

        // Execute the desired action
        StartCoroutine("StartStandUp");
    }

    // Update is called once per frame
    IEnumerator StartStandUp() {
        cameraCutScene.GetComponent<Animator>().Play("Title");
        if (cutSceneMusic != null)
        {
            cameraCutScene.GetComponent<AudioSource>().clip = cutSceneMusic;
            cameraCutScene.GetComponent<AudioSource>().Play();
        }
        yield return new WaitForSeconds(15f);
        StopCutScene();
        GameObject.Find("Player").GetComponent<FormsController>().enabled = true;
        GameObject.FindWithTag("Player").GetComponent<ActionsNew>().FinIntro();
        
        Sauvegarde.EnableUI();
    }
}
