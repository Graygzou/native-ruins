using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour {

    private void Awake()
    {
        // This manager will be persistent
        DontDestroyOnLoad(gameObject);
    }
}
