using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAknowledge : MonoBehaviour {

    public bool HasDiscoveredBow { get; set; }
    public bool HasDiscoveredRope { get; set; }
    public bool HasDiscoveredSail { get; set; }

    // Use this for initialization
    void Start () {
        HasDiscoveredBow = false;
        HasDiscoveredRope = false;
        HasDiscoveredSail = false;
    }
}
