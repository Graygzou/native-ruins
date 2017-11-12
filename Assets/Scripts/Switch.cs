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

    void Start() {
        if(cameraCutScene != null)
            cameraCutScene.enabled = false;
    }

    // Used to launch a mechanism
    public abstract void Activate();

    // Used to launch a cutscene
    public virtual void StartCutScene() {
        // Pre-process
        GameObject.Find("SportyGirl").GetComponent<PlayerController>().enabled = false;
        GameObject.Find("SportyGirl").GetComponent<MovementController>().enabled = false;
        GameObject.Find("SportyGirl").transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().enabled = false;
        // Enable the right camera
        cameraCutScene.enabled = true;
        playerCamera.enabled = false;
    }

    //public abstract IEnumerator PlayCutScene(float time);

    public virtual void StopCutScene() {
        cameraCutScene.enabled = false;
        playerCamera.enabled = true;
        GameObject.Find("SportyGirl").transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().enabled = true;
        GameObject.Find("SportyGirl").GetComponent<PlayerController>().enabled = true;
        GameObject.Find("SportyGirl").GetComponent<MovementController>().enabled = true;
    }

}
