using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FireLaser : MonoBehaviour {

    public float updateFrequency = 1f;
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
        timer = 0;
        line = gameObject.GetComponent<LineRenderer>();
        StartCoroutine(RedrawLaser());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //timer = 0;
        //line.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, Time.time);
        //if (gameObject.tag != "Laser")
        //{
            if (timer >= updateFrequency)
            {
                timer = 0;
                //Debug.Log("Redrawing laser");
                //foreach (GameObject laserSplit in GameObject.FindGameObjectsWithTag("Laser"))
                //    Destroy(laserSplit);

                StartCoroutine(RedrawLaser());
            }
            timer += Time.deltaTime;
        //}
        //else
        //{
        //    line = gameObject.GetComponent<LineRenderer>();
        //    StartCoroutine(RedrawLaser());
        //}
    }

    IEnumerator RedrawLaser()
    {
        line.enabled = true;
        int vertexCounter = 1; //How many line segments are there
        bool loopActive = true; //Is the reflecting loop active?

        Vector3 laserDirection = transform.forward; //direction of the next laser
        Vector3 lastLaserPosition = transform.position; //origin of the next laser

        line.positionCount = vertexCounter;
        line.SetPosition(0, lastLaserPosition);
        RaycastHit hit;

        while (loopActive)
        {
            Ray ray = new Ray(lastLaserPosition, laserDirection);
            // Get the first object hit
            if (Physics.Raycast(ray, out hit))
            {
                GameObject mirror = null;
                //Debug.Log(hit.transform.childCount);
                //Debug.Log(this.gameObject.tag);
                if (hit.transform.childCount >= 1)
                {
                    mirror = hit.transform.GetChild(0).gameObject;
                    if (mirror.transform.tag == "Mirror")
                    {
                        //Debug.DrawRay(lastLaserPosition, (laserDirection * 100), Color.green);
                        //Debug.Log("Reflect");

                        vertexCounter += 3;
                        line.positionCount = vertexCounter;
                        line.SetPosition(vertexCounter - 3, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));
                        line.SetPosition(vertexCounter - 2, lastLaserPosition);
                        line.SetPosition(vertexCounter - 1, hit.point);
                        //line.SetWidth(.01f, .01f);

                        Vector3 prevDirection = lastLaserPosition;
                        lastLaserPosition = hit.point;

                        //Debug.DrawLine(hit.normal, hit.normal * 3, Color.green);
                        //Debug.Log(hit.normal);
                        //laserDirection = Vector3.Reflect(laserDirection, -hit.normal)
                        //laserDirection = new Vector3(-1, 0, 0);
                        //Debug.Log(Vector3.SignedAngle(laserDirection, hit.normal, Vector3.up));

                        Vector3 res = Vector3.Cross(laserDirection, Vector3.up);
                        if (Vector3.Dot(res, hit.normal) > 0)
                        {
                            laserDirection = res;
                            //laserDirection = Quaternion.AngleAxis(-90, Vector3.up) * laserDirection;
                        }
                        else
                        {
                            laserDirection = -res;
                            //laserDirection = Quaternion.AngleAxis(90, Vector3.up) * laserDirection;
                        }
                    }
                    else
                    {
                        //Debug.DrawRay(lastLaserPosition, (laserDirection * 100), Color.red);
                        //Debug.Log("Not Reflect");
                        //laserReflected++;
                        vertexCounter += 1;
                        line.positionCount = vertexCounter;
                        //Vector3 lastPos = lastLaserPosition + (laserDirection.normalized * 10);
                        //line.SetPosition(vertexCounter - 2, lastLaserPosition);
                        line.SetPosition(vertexCounter - 1, lastLaserPosition + (laserDirection.normalized * hit.distance));
                        //line.SetPosition(vertexCounter - 2, lastLaserPosition);
                        //line.SetPosition(vertexCounter - 1, hit.point);

                        loopActive = false;
                    }
                }
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
