using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemPumaSwitch : CutScene
{

    private bool PlayedOnce;

    // Use this for initialization
    void Start()
    {
        PlayedOnce = false;
        if (cameraCutScene != null)
            cameraCutScene.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == GameObject.FindWithTag("Player").GetComponent<Collider>() && (PlayedOnce == false))
        {
            PlayedOnce = true;
            Activate();
        }
    }

    override public IEnumerator PlayCutSceneStart() {
        cameraCutScene.GetComponent<Animator>().Play("CutscenePuma");
        cameraCutScene.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(10f);
        //ActivateSwitch();
        //yield return new WaitForSeconds(1f);
        StopCutScene();
        if (gameObject.GetComponent<SwitchObject>() != null)
        {
            // Activate all his children
            gameObject.GetComponent<SwitchObject>().ActivateChildren();
        }
    }

    // The switch does what he's meant for here.
    override protected void ActivateSwitch()
    {
        GameObject judy = GameObject.FindWithTag("Player");

    }

    override protected void DiactivateSwitch()
    {
        GameObject judy = GameObject.FindWithTag("Player");
    }
}
