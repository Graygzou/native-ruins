using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandUpCutScene : Switch {

    protected override void ActivateSwitch() {
        // Setting up
        GameObject.Find("SecondCutSceneCamera").GetComponent<Camera>().enabled = true;
        GameObject.FindWithTag("Player").GetComponent<MovementControllerHuman>().enabled = false;
        GameObject.FindWithTag("Player").GetComponent<ActionsNew>().enabled = false;
        GameObject.FindWithTag("MainCamera").GetComponent<Camera>().enabled = false;
        GameObject.Find("FirstCutSceneCamera").GetComponent<Camera>().enabled = false;

        // Execute the desired action
        GameObject.FindWithTag("Player").GetComponent<ActionsNew>().GettingUp();
        StartCoroutine("StartStandUp");
    }

    // Update is called once per frame
    IEnumerator StartStandUp() {
        // Wait the end of the animation
        yield return new WaitForSeconds(7f);
        SwitchManager.EndAction();
    }

}
