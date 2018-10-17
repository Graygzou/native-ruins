using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AutoSelectInput : MonoBehaviour {

    [SerializeField]
    private GameObject selectedObject;
    [SerializeField]
    private EventSystem eventSystem;

    private bool buttonSelected = false;

    void Update()
    {
        Debug.Log("IsFocused" + eventSystem.isFocused);
        Debug.Log("CurrentGameObject" + eventSystem.currentSelectedGameObject);
        if (Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
    }

    private void OnDisable()
    {
        buttonSelected = false;
    }
}
