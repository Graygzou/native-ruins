using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager {

    #region Enums
    public enum ActionsLabels : int
    {
        Movement = 0,
        Sprint = 1,
        Crouch = 2,
        Jump = 3,
        Aiming = 4,
        Attack = 5,
        OpenInventory = 6,
        PutAwayWeapon = 7,
        Interact = 8,
        Transformation = 9,
        MoveCamera = 100,
        
    }

    public enum EventTypeButton : int
    {
        Up = 0,
        Down = 1,
        Hold = 2,
    }

    public enum EventTypeChanged : int
    {
        Changed = 0,
        UnChanged = 1,
    }
    #endregion

    // Use keycode
    //private IDictionary<KeyCode, System.Action[]> keyCodeCallbacks = new Dictionary<KeyCode, System.Action[]>();
    // Use Unity inputs
    private static IDictionary<ActionsLabels, KeyValuePair<string[], System.Action[]>> buttonInputsCallbacks = new Dictionary<ActionsLabels, KeyValuePair<string[], System.Action[]>>();
    private static IDictionary<string, System.Action> axisMovementsCallbacks = new Dictionary<string, System.Action>();
    private static IDictionary<ActionsLabels, KeyValuePair<string[], System.Action[]>> axisMovementsChangedCallbacks = new Dictionary<ActionsLabels, KeyValuePair<string[], System.Action[]>>();


    // Others
    /*
    public System.Action[] mouseScrollCallbacks = new System.Action[] { };
    public System.Action[] mouseDragCallbacks = new System.Action[] { };
    public System.Action[] mouseExitWindowsCallbacks = new System.Action[] { };
    public System.Action[] mouseEnterWindowsCallbacks = new System.Action[] { };*/

    /*
        MouseMove = 3,
        MouseDrag = 4,
        MouseExitWindows = 5,
        MouseEnterWindows = 6,*/

    /*
    #region KeyboardEventsDelegates
    public delegate void KeyUpEvent();
    public static event KeyUpEvent OnKeyUpEvent;
    public delegate void KeyDownEvent();
    public static event KeyDownEvent OnKeyDownEvent;
    #endregion

    #region MouseEventsDelegates
    public delegate void MouseUpEvent(int buttonPressed);
    public static event MouseUpEvent OnMouseUpEvent;
    public delegate void MouseDownEvent(int buttonPressed);
    public static event MouseDownEvent OnMouseDownEvent;

    public delegate void MouseMoveEvent(Vector2 mousePosition);
    public static event MouseMoveEvent OnMouseMoveEvent;

    public delegate void MouseDragEvent(Vector2 mousePosition);
    public static event MouseDragEvent OnMouseDragEvent;

    public delegate void MouseEnterWindowsEvent();
    public static event MouseEnterWindowsEvent OnMouseEnterWindowsEvent;
    public delegate void MouseExitWindowsEvent();
    public static event MouseExitWindowsEvent OnMouseExitWindowsEvent;
    #endregion

    #region ScrollEventsDelegates
    public delegate void ScrollEvent();
    public static event ScrollEvent OnScrollEvent;
    #endregion
    */

    #region TESTS
    private static void Start()
    {
        /*SubscribeButtonEvent("Horizontal", EventTypeButton.Down, Horizontal);
        SubscribeButtonEvent("Vertical", EventTypeButton.Down, Vertical);
        //SubscribeKeyEvent("Zoom1", EventTypeButton.Down, Zoom1);
        SubscribeButtonEvent("Fire1", EventTypeButton.Down, Fire1);
        SubscribeButtonEvent("Aiming", EventTypeButton.Down, Aiming);
        //SubscribeButtonEvent("Fire2", EventTypeButton.Down, Fire2);
        SubscribeMouseMovementsEvent("Fire1", Fire1);
        SubscribeButtonEvent("Jump", EventTypeButton.Down, Jump);
        SubscribeMouseMovementsEvent("ZoomCamera", MouseScrollWheel);
        SubscribeButtonEvent("Submit", EventTypeButton.Down, Submit);
        SubscribeButtonEvent("Cancel", EventTypeButton.Down, Cancel);
        SubscribeButtonEvent("Boost", EventTypeButton.Down, Boost);
        SubscribeButtonEvent("OpenInventory", EventTypeButton.Down, OpenInventory);
        SubscribeButtonEvent("Crouch", EventTypeButton.Down, Crouch);*/
        SubscribeMouseMovementsEvent("Boost", Boost);
        //SubscribeMouseMovementsEvent("HorizontalCamera", HorizontalCamera);
        //SubscribeMouseMovementsEvent("VerticalCamera", VerticalCamera);
    }

    private static void Horizontal() 
    {
        Debug.Log("Horizontal pressed !");
    }

    private static void Vertical()
    {
        Debug.Log("Vertical pressed !");
    }

    private static void Zoom1()
    {
        Debug.Log("Zoom1 pressed !");
    }

    private static void Fire1()
    {
        Debug.Log("Fire1 pressed !");
    }

    private static void Aiming()
    {
        Debug.Log("Aiming pressed !");
    }

    private static void Fire3()
    {
        Debug.Log("Fire3 pressed !");
    }

    private static void Jump()
    {
        Debug.Log("Jump (space bar) pressed !");
    }

    private static void MouseX()
    {
        Debug.Log("MouseX pressed !");
    }

    private static void MouseY()
    {
        Debug.Log("MouseY pressed !");
    }

    private static void MouseScrollWheel()
    {
        Debug.Log("MouseScrollWheel pressed !");
    }

    private static void Submit()
    {
        Debug.Log("Submit pressed !");
    }

    private static void Cancel()
    {
        Debug.Log("Cancel pressed !");
    }

    private static void Boost()
    {
        Debug.Log("Boost pressed !");
    }

    private static void OpenInventory()
    {
        Debug.Log("Inventory pressed !");
    }

    private static void Crouch()
    {
        Debug.Log("Crouch pressed !");
    }

    private static void HorizontalCamera()
    {
        Debug.Log("Horizontal camera !");
    }

    private static void VerticalCamera()
    {
        Debug.Log("Vertical camera !");
    }

    #endregion

    public static void CheckAllInputs()
    {
        GetVirtualButtonInputs();
        GetMouseMoveInput();
        GetMouseMovementsChangedInput();
    }

    #region UnityVirtualEvents
    public static void GetVirtualButtonInputs()
    {
        foreach (ActionsLabels action in buttonInputsCallbacks.Keys)
        {
            KeyValuePair<string[], System.Action[]> currentPair = buttonInputsCallbacks[action];
            bool triggered = false;
            int i = 0;
            while(!triggered && i < currentPair.Key.Length)
            {
                string input = currentPair.Key[i];
                if (triggered = (Input.GetButtonUp(input) && buttonInputsCallbacks[action].Value[(int)EventTypeButton.Up] != null))
                {
                    buttonInputsCallbacks[action].Value[(int)EventTypeButton.Up]();
                }
                if (triggered = (Input.GetButton(input) && buttonInputsCallbacks[action].Value[(int)EventTypeButton.Hold] != null))
                {
                    buttonInputsCallbacks[action].Value[(int)EventTypeButton.Hold]();
                }
                if (triggered = (Input.GetButtonDown(input) && buttonInputsCallbacks[action].Value[(int)EventTypeButton.Down] != null))
                {
                    buttonInputsCallbacks[action].Value[(int)EventTypeButton.Down]();
                }
                i++;
            }
        }
    }

    public static void GetMouseMoveInput()
    {
        foreach (string input in axisMovementsCallbacks.Keys)
        {
            axisMovementsCallbacks[input]();
        }
    }

    public static void GetMouseMovementsChangedInput()
    {
        foreach (ActionsLabels action in axisMovementsChangedCallbacks.Keys)
        {
            KeyValuePair<string[], System.Action[]> currentPair = axisMovementsChangedCallbacks[action];
            bool triggered = false;
            int i = 0;
            while (!triggered && i < currentPair.Key.Length)
            {
                string input = currentPair.Key[i];
                if (triggered = (Input.GetAxis(input) != 0 && axisMovementsChangedCallbacks[action].Value[(int)EventTypeChanged.Changed] != null))
                {
                    axisMovementsChangedCallbacks[action].Value[(int)EventTypeChanged.Changed]();
                }
                else if (axisMovementsChangedCallbacks[action].Value[(int)EventTypeChanged.UnChanged] != null)
                {
                    axisMovementsChangedCallbacks[action].Value[(int)EventTypeChanged.UnChanged]();
                }
                i++;
            }
        }
    }
    #endregion

    public static void UnsubscribeAll(string key)
    {
        buttonInputsCallbacks.Clear();
        axisMovementsCallbacks.Clear();
        axisMovementsChangedCallbacks.Clear();

        buttonInputsCallbacks = null;
        axisMovementsCallbacks = null;
        axisMovementsChangedCallbacks = null;
    }

    #region SubscribeEvents
    public static void SubscribeButtonEvent(ActionsLabels action, string input, EventTypeButton type, System.Action callback)
    {
        if (buttonInputsCallbacks.ContainsKey(action))
        {
            buttonInputsCallbacks[action].Value[(int)type] = callback;
        }
        else
        {
            switch(type)
            {
                case EventTypeButton.Up:
                    SubscribeButtonEvents(action, input, new System.Action[] { callback, null, null });
                    break;
                case EventTypeButton.Down:
                    SubscribeButtonEvents(action, input, new System.Action[] { null, callback, null });
                    break;
                case EventTypeButton.Hold:
                    SubscribeButtonEvents(action, input, new System.Action[] { null, null, callback });
                    break;
            }
        }
    }

    public static void SubscribeButtonEvents(ActionsLabels action, string input, params System.Action[] callbacks)
    {
        SubscribeButtonEvents(action, new string[] { input }, callbacks);
    }

    public static void SubscribeButtonEvents(ActionsLabels action, string[] input, params System.Action[] callbacks)
    {
        KeyValuePair<string[], System.Action[]> currentValue = new KeyValuePair<string[], System.Action[]>(input, callbacks);
        if (buttonInputsCallbacks.ContainsKey(action))
        {
            // Override input
            buttonInputsCallbacks[action] = currentValue;
        }
        else
        {
            int enumsLength = Enum.GetNames(typeof(EventTypeButton)).Length;
            if (callbacks.Length == enumsLength)
            {
                buttonInputsCallbacks.Add(action, currentValue);
            }
            else
            {
                Debug.LogWarning("The size of the callbacks array (" + callbacks.Length + ")" +
                    " is not equals to the lenght of the Enums (" + enumsLength + "). Callbacks ignored.");
            }
        }
    }

    public static void SubscribeMouseMovementsEvent(string input, System.Action callbacks)
    {
        if (axisMovementsCallbacks.ContainsKey(input))
        {
            // Override input
            axisMovementsCallbacks[input] = callbacks;
        }
        else
        {
            // Add the input
            axisMovementsCallbacks.Add(input, callbacks);
        }
    }

    public static void SubscribeMouseMovementsChangedEvent(ActionsLabels action, string input, EventTypeChanged type, System.Action callback)
    {
        if (axisMovementsChangedCallbacks.ContainsKey(action))
        {
            // Override input
            axisMovementsChangedCallbacks[action].Value[(int)type] = callback;
        }
        else
        {
            // Add the input
            switch (type)
            {
                case EventTypeChanged.Changed:
                    SubscribeMouseMovementsChangedEvents(action, input, new System.Action[] { callback, null });
                    break;
                case EventTypeChanged.UnChanged:
                    SubscribeMouseMovementsChangedEvents(action, input, new System.Action[] { null, callback });
                    break;
            }
        }
    }

    public static void SubscribeMouseMovementsChangedEvents(ActionsLabels action, string input, params System.Action[] callbacks)
    {
        SubscribeMouseMovementsChangedEvents(action, new string[] { input }, callbacks);
    }

    public static void SubscribeMouseMovementsChangedEvents(ActionsLabels action, string[] input, params System.Action[] callbacks)
    {
        KeyValuePair<string[], System.Action[]> currentValue = new KeyValuePair<string[], System.Action[]>(input, callbacks);
        if (axisMovementsChangedCallbacks.ContainsKey(action))
        {
            // Override input
            axisMovementsChangedCallbacks[action] = currentValue;
        }
        else
        {
            // Add the input
            int enumsLength = Enum.GetNames(typeof(EventTypeChanged)).Length;
            if (callbacks.Length == enumsLength)
            {
                axisMovementsChangedCallbacks.Add(action, currentValue);
            }
            else
            {
                Debug.LogWarning("The size of the callbacks array (" + callbacks.Length + ")" +
                    " is not equals to the lenght of the Enums (" + enumsLength + "). Callbacks ignored.");
            }
        }
    }
    #endregion

}

