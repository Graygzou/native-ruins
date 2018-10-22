using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostCutScene : CutScene
{

    protected override void ActivateSwitch() {

        // Execute the desired action
        GameObject.FindWithTag("Player").GetComponent<ActionsNew>().Lost();
        StartCoroutine("Lost");
    }

    // Update is called once per frame
    IEnumerator Lost() {
        // Wait the end of the animation
        yield return new WaitForSeconds(2f);
        SwitchManager.EndAction();
    }

}
