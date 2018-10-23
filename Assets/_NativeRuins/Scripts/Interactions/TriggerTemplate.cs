using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerTemplate {

    public Camera cameraCutScene;

    protected Camera playerCamera;

    protected bool isActived = false;

    void Awake()
    {
        // Get the camera of the player
        playerCamera = Camera.main;
    }

    void Start()
    {
        if (cameraCutScene != null)
        {
            cameraCutScene.enabled = false;
        }
    }

    public void Activate()
    {
        // If the switch got a camera, switch to activate it
        if (cameraCutScene != null && !isActived)
        {
            // Call a cut-scene to start the switch
            SetupCutSceneStart();
            PlayCutSceneStart();
        }
        else if (cameraCutScene != null && isActived)
        {
            // Call a cut-scene to end the switch
            PlayCutSceneEnd();
        }
        else
        {
            // Simply activate the desired switch
            ActivateSwitch();
        }
    }

    // Used to launch the real mechanism
    protected abstract void ActivateSwitch();

    // Used to cancel the mechanism
    protected virtual void DiactivateSwitch() { }

    // Used to setup a cutscene
    public virtual void SetupCutSceneStart()
    {
        isActived = true;
        GameObject.FindWithTag("Player").GetComponent<MovementController>().enabled = false;
        ActionsNew actions = GameObject.FindWithTag("Player").GetComponent<ActionsNew>();
        if (actions != null)
        {
            actions.Stay(100f);
        }

        // Enable the right camera
        playerCamera.enabled = false;
        cameraCutScene.enabled = true;
    }

    public virtual IEnumerator PlayCutSceneStart()
    {
        yield return null;
    }

    public virtual IEnumerator PlayCutSceneEnd()
    {
        yield return null;
    }

    public virtual void StopCutScene()
    {
        cameraCutScene.enabled = false;
        playerCamera.enabled = true;
        GameObject.FindWithTag("Player").GetComponent<MovementController>().enabled = true;
    }

}
