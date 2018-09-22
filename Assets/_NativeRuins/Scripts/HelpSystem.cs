using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpSystem : MonoBehaviour
{

    private GameObject help;
    // Use this for initialization
    void Start()
    {
        help = GameObject.Find("HelpSystem/Help");
        help.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            help.SetActive(!help.activeSelf);
        }

    }
}
