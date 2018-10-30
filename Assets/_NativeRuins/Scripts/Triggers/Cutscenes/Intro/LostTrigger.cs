using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostTrigger : Trigger
{
    public override void Fire()
    {
        // Execute the desired action
        GameObject.FindWithTag("Player").GetComponent<PlayerProperties>().Mad();
        StartCoroutine("Lost");
    }

    // Update is called once per frame
    IEnumerator Lost()
    {
        // Wait the end of the animation
        yield return new WaitForSeconds(2f);
        SwitchManager.EndAction();
        NoticeSubscribers();
    }
}
