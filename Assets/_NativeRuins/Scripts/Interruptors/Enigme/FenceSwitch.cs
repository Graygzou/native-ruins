using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceSwitch : Switch {

    public float smoothing = 1f;
    private Vector3 target;

    // Use this for initialization
    void Start () {
        if (cameraCutScene != null)
            cameraCutScene.enabled = false;
        target = transform.position - transform.up * 17;
    }

    // Used to launch a mechanism
    override protected void ActivateSwitch() {
        StartCoroutine(MyCoroutine(target));
    }

    IEnumerator MyCoroutine(Vector3 position)
    {
        while (Vector3.Distance(transform.position, position) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, position, smoothing * Time.deltaTime);

            yield return null;
        }
        yield return new WaitForSeconds(3f);
    }

    override public IEnumerator PlayCutSceneStart()
    {
        yield return new WaitForSeconds(1f);
        ActivateSwitch();
        yield return new WaitForSeconds(1f);
        StopCutScene();
    }

    //IEnumerator PlayCutScene(float time)
    //{
    //    // TODO
    //    cameraCutScene.GetComponent<Animator>().enabled = false;
    //    yield return null;
    //}
}
