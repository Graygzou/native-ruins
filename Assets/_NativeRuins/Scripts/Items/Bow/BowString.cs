using UnityEngine;
using System.Collections;

public class BowString : MonoBehaviour {

    [Range(0.0f, 1.0f)]
    public float factor;

    Vector3 firstPosition;
    Vector3 lastPosition;

    public bool stretching;
    public bool releasing;

    public float stretchSpeed = 0.5f;
    public float releaseSpeed = 0.5f;

    // Use this for initialization
    void Start () {
        firstPosition = transform.localPosition;
        lastPosition = transform.localPosition + new Vector3(0.0f, 0.8f, -0.1f); //Vector3.up * 0.8f;
    }
	
	// Update is called once per frame
	void Update () {
        if (stretching)
        {
            factor += stretchSpeed * Time.deltaTime;

            if (factor > 1.0f)
            {
                factor = 1.0f;
            }
        }
        if (releasing)
        {
            factor -= releaseSpeed* Time.deltaTime;

            if (factor < 0.0f)
            {
                factor = 0.0f;
            }
        }

        transform.localPosition = Vector3.Lerp(firstPosition, lastPosition, factor);
    }

    //You probably want to call this somewhere
    public void Stretch()
    {
        stretching = true;
        releasing = false;
        //Debug.Log(GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3").transform.position);
        //Debug.Log(GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmLeftCollarbone/RigArmLeft1/RigArmLeft2/RigArmLeft3/Bow3D").transform.position);
        //Debug.Log((GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3").transform.position - GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmLeftCollarbone/RigArmLeft1/RigArmLeft2/RigArmLeft3").transform.position).normalized * 0.8f);
        //Debug.Log(firstPosition);
        /*
        Vector3 v = (GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3").transform.position - GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmLeftCollarbone/RigArmLeft1/RigArmLeft2/RigArmLeft3/Bow3D").transform.position).normalized * 0.8f;
        Debug.Log(v);
        v = new Vector3(v.y, -v.z, -v.x);
        lastPosition = firstPosition + v;*/
    }

    public void Release()
    {
        releasing = true;
        stretching = false;
    }
}
