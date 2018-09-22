using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCutScene : Switch {

    protected override void ActivateSwitch() {

        // Execute the desired action
        GameObject.FindWithTag("Player").GetComponent<ActionsNew>().Focus();
        StartCoroutine("Focus");
    }

    // Update is called once per frame
    IEnumerator Focus() {
        // Wait the end of the animation
        yield return new WaitForSeconds(3f);
        SwitchManager.EndAction();
    }

}
