using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTrigger : Trigger
{
    private float timer = 4.0f;

    public override void Fire() {
        Color color = GetComponent<Renderer>().sharedMaterial.color;
        color.a = 1f;
        GetComponent<Renderer>().sharedMaterial.color = color;
        StartCoroutine("StartFade");
    }

    // Update is called once per frame
    IEnumerator StartFade() {
        while(timer > 0) {
            timer = timer - Time.deltaTime;

            if (timer <= 2 && timer > 0 || timer >= 3) {
                Color color = GetComponent<Renderer>().material.color;
                color.a -= 0.5f * Time.deltaTime;
                GetComponent<Renderer>().material.color = color;
            } else if (timer > 0) {
                Color color = GetComponent<Renderer>().material.color;
                color.a += 0.5f * Time.deltaTime;
                GetComponent<Renderer>().material.color = color;
            }
            yield return new WaitForEndOfFrame();
        }
        SwitchManager.EndAction();
        base.NoticeSubscribers();
    }

    public override void Interrupt()
    {
        base.Interrupt();
        // Make the plane color invisible.
        GetComponent<Renderer>().material.color = new Color(255, 255, 255, 0);
    }
}
