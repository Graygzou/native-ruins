using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandUpTrigger : Trigger
{

    public override void Fire()
    {
        // Execute the desired action
        GameObject.FindWithTag("Player").GetComponent<PlayerProperties>().StandUp();

        StartCoroutine("StartStandUp");
    }

    // Update is called once per frame
    IEnumerator StartStandUp()
    {
        // Wait the end of the animation
        //yield return new WaitForSeconds(7f);
        yield return null;
        SwitchManager.EndAction();
        NoticeSubscribers();
    }

}
