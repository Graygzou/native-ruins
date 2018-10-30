using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusTrigger : Trigger
{
    public override void Fire()
    {
        // Execute the desired action
        GameObject.FindWithTag("Player").GetComponent<PlayerProperties>().Focus();
        StartCoroutine("Focus");
    }

    // Update is called once per frame
    IEnumerator Focus()
    {
        // Wait the end of the animation
        yield return new WaitForSeconds(3f);
        SwitchManager.EndAction();
        NoticeSubscribers();
    }
}
