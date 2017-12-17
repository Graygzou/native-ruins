using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleGameCutScene : Switch {

    public AnimationClip cutScene;
    public AudioClip cutSceneMusic;

    protected override void ActivateSwitch() {
        // Setting up
        cameraCutScene = GameObject.Find("ForthCutSceneCamera").GetComponent<Camera>();
        cameraCutScene.enabled = true;
        GameObject.FindWithTag("Player").GetComponent<MovementControllerHuman>().enabled = false;
        GameObject.FindWithTag("Player").GetComponent<ActionsNew>().enabled = false;
        GameObject.FindWithTag("MainCamera").GetComponent<Camera>().enabled = false;
        GameObject.Find("ThirdCutSceneCamera").GetComponent<Camera>().enabled = false;

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
        GameObject.FindWithTag("Player").GetComponent<ActionsNew>().FinIntro();
    }

}
