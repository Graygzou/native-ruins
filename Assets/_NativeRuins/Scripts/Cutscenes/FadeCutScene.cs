using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCutScene : Switch {

    private float timer;

	// Use this for initialization
	void Start () {
        this.timer = 4.0f;
        Color color = GetComponent<Renderer>().material.color;
        color.a = 1f;
        GetComponent<Renderer>().material.color = color;
    }

    protected override void ActivateSwitch() {
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
        Destroy(gameObject);
        SwitchManager.EndAction();
    }

}
