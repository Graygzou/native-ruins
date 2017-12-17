using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundCutScene : Switch {

    protected override void ActivateSwitch() {
        // Setting up
        GameObject.Find("ThirdCutSceneCamera").GetComponent<Camera>().enabled = true;
        GameObject.FindWithTag("Player").GetComponent<MovementControllerHuman>().enabled = false;
        GameObject.FindWithTag("MainCamera").GetComponent<Camera>().enabled = false;
        GameObject.Find("SecondCutSceneCamera").GetComponent<Camera>().enabled = false;

        // Execute the desired action
        GameObject.FindWithTag("Player").GetComponent<ActionsNew>().LookAround();
        StartCoroutine("StartStandUp");
    }

    // Update is called once per frame
    IEnumerator StartStandUp() {
        // Wait the end of the animation
        yield return new WaitForSeconds(3f);
        SwitchManager.EndAction();
    }

}
