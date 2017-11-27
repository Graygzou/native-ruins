using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Switch : MonoBehaviour {

    protected Camera playerCamera;
    public Camera cameraCutScene;

    void Awake() {
        // Get the camera of the player
        playerCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    public void Activate() {
        // If the switch got a camera, switch to activate it
        if (cameraCutScene != null) {
            StartCutScene();
            StartCoroutine("PlayCutScene");
        } else {
            ActivateSwitch();
        }
    }

    // Used to launch the real mechanism
    protected abstract void ActivateSwitch();

    // Used to cancel the mechanism
    protected virtual void DiactivateSwitch() { }

    // Used to launch a cutscene
    public virtual void StartCutScene() {
        // Pre-process
        //GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = false;
        GameObject.FindWithTag("Player").GetComponent<MovementController>().enabled = false;
        //GameObject.Find("SportyGirl").transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().enabled = false;
        // Enable the right camera
        playerCamera.enabled = false;
        cameraCutScene.enabled = true;
    }

    public virtual IEnumerator PlayCutScene() {
        yield return null;
    }

    public virtual void StopCutScene() {
        cameraCutScene.enabled = false;
        playerCamera.enabled = true;
        //GameObject.Find("SportyGirl").transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().enabled = true;
        //GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = true;
        GameObject.FindWithTag("Player").GetComponent<MovementController>().enabled = true;
    }

}
