using UnityEngine;
using System.Collections;

public class OpenChest : MonoBehaviour {

    [Range(0.0f, 1.0f)]
    public float factor;

    Quaternion closedAngle;
    Quaternion openedAngle;

    public bool closing;
    public bool opening;

    public float speed = 0.5f;

    int newAngle = 127;

    // Use this for initialization
    void Start () {
        openedAngle = transform.rotation;
        closedAngle = Quaternion.Euler(transform.eulerAngles + Vector3.right * newAngle);
    }
	
	// Update is called once per frame
	void Update () {

        if (closing)
        {
            factor += speed * Time.deltaTime;

            if (factor > 1.0f)
            {
                factor = 1.0f;
            }
        }
        if (opening)
        {
            factor -= speed * Time.deltaTime;

            if (factor < 0.0f)
            {
                factor = 0.0f;
            }
        }

        transform.rotation = Quaternion.Lerp(openedAngle, closedAngle, factor);
	}

    //You probably want to call this somewhere
    void Close()
    {
        closing = true;
        opening = false;
    }

    void Open()
    {
        opening = true;
        closing = false;
    }
}
