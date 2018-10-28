using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundTrigger : Trigger
{
    [SerializeField]
    private Transform newTransform;

    public override void Fire()
    {
        GameObject.FindWithTag("Player").transform.SetPositionAndRotation(newTransform.position, newTransform.rotation);
        GameObject.FindWithTag("Player").GetComponent<PlayerProperties>().LookAround();
        StartCoroutine("StartStandUp");
    }

    // Update is called once per frame
    IEnumerator StartStandUp()
    {
        // Wait the end of the animation
        yield return new WaitForSeconds(3f);
        SwitchManager.EndAction();
    }
}
