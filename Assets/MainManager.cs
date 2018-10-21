using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour {

    public enum ManagerName : int
    {
        AudioManager = 0,
        MenuManager = 1,
        ScriptManager = 2,
        DialogueTrigger = 3,
    }

    [SerializeField]
    private Manager[] managers;

    private void Awake()
    {
        // This manager will be persistent
        DontDestroyOnLoad(gameObject);
    }

    public Manager FindManager(ManagerName manageName)
    {
        return managers[(int)manageName];
    }

    public Manager FindManager(string name)
    {
        Manager result = null;
        bool find = false;
        int i = 0;
        while(!find && i < managers.Length)
        {
            Debug.Log(managers[i].name);
            find = managers[i].name.Equals(name);
            if(find)
            {
                result = managers[i];
            }
        }

        return result;
    }
}
