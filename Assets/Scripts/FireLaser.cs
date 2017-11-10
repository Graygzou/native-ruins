using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FireLaser : MonoBehaviour {

    public float updateFrequency = 0.1f;
    public int laserDistance;
    public string bounceTag;
    public string splitTag;
    public string spawnedBeamTag;
    public int maxBounce;
    public int maxSplit;
    private float timer = 0;
    private LineRenderer line;

    // Use this for initialization
    void Start () {
        line = gameObject.GetComponent<LineRenderer>();
        line.enabled = true;
	}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (gameObject.tag != spawnedBeamTag)
    //    {
    //        if (timer >= updateFrequency)
    //        {
    //            timer = 0;
    //            //Debug.Log("Redrawing laser");
    //            foreach (GameObject laserSplit in GameObject.FindGameObjectsWithTag(spawnedBeamTag))
    //                Destroy(laserSplit);

    //            StartCoroutine(RedrawLaser());
    //        }
    //        timer += Time.deltaTime;
    //    }
    //}

    // Update is called once per frame
    void FixedUpdate()
    {
        //timer = 0;
        //line.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, Time.time);
        StartCoroutine(RedrawLaser());
    }

    IEnumerator RedrawLaser()
    {
        //int laserSplit = 1; //How many times it got split
        //int laserReflected = 1; //How many times it got reflected
        int vertexCounter = 0; //How many line segments are there
        bool loopActive = true; //Is the reflecting loop active?

        Vector3 laserDirection = transform.forward; //direction of the next laser
        Vector3 lastLaserPosition = transform.position; //origin of the next laser

        RaycastHit hit;

        while (loopActive)
        {
            Ray ray = new Ray(lastLaserPosition, laserDirection);
            // Get the first object hit
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "Mirror")
                {
                    Debug.Log("Reflect");
                    //print("Found an object - distance: " + hit.distance);
                    //line.SetPosition(0, ray.origin);
                    //line.SetPosition(1, hit.point);

                    //laserReflected++;
                    vertexCounter += 3;
                    line.positionCount = vertexCounter;
                    //line.SetPosition(vertexCounter - 3, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));
                    line.SetPosition(vertexCounter - 2, lastLaserPosition);
                    line.SetPosition(vertexCounter - 1, hit.point);
                    //line.SetWidth(.01f, .01f);

                    Vector3 prevDirection = lastLaserPosition;
                    lastLaserPosition = Vector3.MoveTowards(hit.point, lastLaserPosition, 0.05f);
                    Debug.Log("Test :" + Vector3.Angle(laserDirection, hit.normal));

                    //laserDirection = Vector3.Reflect(laserDirection, -hit.normal);
                    laserDirection = new Vector3(1, 0, 0);
                    //if (Vector3.Angle(laserDirection, hit.normal) > 0)
                    //{
                    //    laserDirection = new Vector3(1, 0, 0);
                    //}
                    //else
                    //{
                    //    laserDirection = new Vector3(-1, 0, 0);
                    //    //laserDirection = Vector3.Reflect(laserDirection, hit.normal);
                    //}
                }
                else
                {
                    Debug.Log("Not Reflect");
                    //laserReflected++;
                    vertexCounter += 2;
                    line.positionCount = vertexCounter;
                    //Vector3 lastPos = lastLaserPosition + (laserDirection.normalized * 10);
                    //line.SetPosition(vertexCounter - 1, lastLaserPosition + (laserDirection.normalized * laserDistance));
                    line.SetPosition(vertexCounter - 2, lastLaserPosition);
                    line.SetPosition(vertexCounter - 1, hit.point);

                    loopActive = false;
                }
            }
            else
            {
                loopActive = false;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    //    IEnumerator RedrawLaser2()
    //{
    //    //Debug.Log("Running");
    //    int laserSplit = 1; //How many times it got split
    //    int laserReflected = 1; //How many times it got reflected
    //    int vertexCounter = 1; //How many line segments are there
    //    bool loopActive = true; //Is the reflecting loop active?

    //    Vector3 laserDirection = transform.forward; //direction of the next laser
    //    Vector3 lastLaserPosition = transform.localPosition; //origin of the next laser

    //    mLineRenderer.SetVertexCount(1);
    //    mLineRenderer.SetPosition(0, transform.position);
    //    RaycastHit hit;

    //    while (loopActive)
    //    {
    //        Debug.Log("Physics.Raycast(" + lastLaserPosition + ", " + laserDirection + ", out hit , " + laserDistance + ")");
    //        if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance) && ((hit.transform.gameObject.tag == bounceTag) || (hit.transform.gameObject.tag == splitTag)))
    //        {
    //            //Debug.Log("Bounce");
    //            laserReflected++;
    //            vertexCounter += 3;
    //            mLineRenderer.SetVertexCount(vertexCounter);
    //            mLineRenderer.SetPosition(vertexCounter - 3, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));
    //            mLineRenderer.SetPosition(vertexCounter - 2, hit.point);
    //            mLineRenderer.SetPosition(vertexCounter - 1, hit.point);
    //            mLineRenderer.SetWidth(.01f, .01f);
    //            lastLaserPosition = hit.point;
    //            Vector3 prevDirection = laserDirection;
    //            laserDirection = Vector3.Reflect(laserDirection, hit.normal);

    //            if (hit.transform.gameObject.tag == splitTag)
    //            {
    //                //Debug.Log("Split");
    //                if (laserSplit >= maxSplit)
    //                {
    //                    Debug.Log("Max split reached.");
    //                }
    //                else
    //                {
    //                    //Debug.Log("Splitting...");
    //                    laserSplit++;
    //                    Object go = Instantiate(gameObject, hit.point, Quaternion.LookRotation(prevDirection));
    //                    go.name = spawnedBeamTag;
    //                    ((GameObject)go).tag = spawnedBeamTag;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            //Debug.Log("No Bounce");
    //            laserReflected++;
    //            vertexCounter++;
    //            mLineRenderer.SetVertexCount(vertexCounter);
    //            Vector3 lastPos = lastLaserPosition + (laserDirection.normalized * laserDistance);
    //            //Debug.Log("InitialPos " + lastLaserPosition + " Last Pos" + lastPos);
    //            mLineRenderer.SetPosition(vertexCounter - 1, lastLaserPosition + (laserDirection.normalized * laserDistance));

    //            loopActive = false;
    //        }
    //        if (laserReflected > maxBounce)
    //            loopActive = false;
    //    }

    //    yield return new WaitForEndOfFrame();
    //}

}
