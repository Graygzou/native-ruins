using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FormsController : MonoBehaviour {

    private int currentForm;

    // Use this for initialization
    void Start() {
        GameObject playerRoot = GameObject.Find("Player");
        int i = 0;
        while(i < playerRoot.transform.childCount) {
            if (playerRoot.transform.GetChild(i).gameObject.activeSelf) {
                currentForm = i;
            }
            i++;
        }
        
    }

    public int getCurrentForm() {
        return currentForm;
    }
}
