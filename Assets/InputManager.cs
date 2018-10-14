using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    #region Enums
    public enum EventTypeButton : int
    {
        Up = 0,
        Down = 1,
        Hold = 2,
    }
    #endregion

    private IDictionary<string, System.Action[]> keysCallbacks = new Dictionary<string, System.Action[]>();
    public IDictionary<int, System.Action[]> mouseButtonsCallbacks = new Dictionary<int, System.Action[]>();
    public System.Action[] mouseMoveCallbacks = new System.Action[] { };
    public System.Action[] mouseScrollCallbacks = new System.Action[] { };
    public System.Action[] mouseDragCallbacks = new System.Action[] { };
    public System.Action[] mouseExitWindowsCallbacks = new System.Action[] { };
    public System.Action[] mouseEnterWindowsCallbacks = new System.Action[] { };

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

    #region KeyboardEvents
    public void SubscribeKeyEvent(string keyCode, EventTypeButton type, System.Action callback)
    {
        if (keysCallbacks.ContainsKey(keyCode))
        {
            keysCallbacks[keyCode][(int)type] = callback;
        }
        else
        {
            SubscribeKeyEvents(keyCode, new System.Action[] { callback });
        }

    }

    public void SubscribeKeyEvents(string key, params System.Action[] callbacks)
    {
        keysCallbacks.Add(key, callbacks);
    }
    #endregion

    #region MouseButtonsEvents
    public void SubscribeMouseEvent(int buttonID, EventTypeButton type, System.Action callback)
    {
        if(mouseButtonsCallbacks.ContainsKey(buttonID))
        {
            mouseButtonsCallbacks[buttonID][(int)type] = callback;
        }
        else
        {
            SubscribeMouseEvents(buttonID, new System.Action[] { callback });
        }
    }

    public void SubscribeMouseEvents(int buttonID, params System.Action[] callbacks)
    {
        mouseButtonsCallbacks.Add(buttonID, callbacks);
    }
    #endregion

    #region OthersMouseEvents
    public void SubscribeMouseMoveEvent(params System.Action[] callbacks)
    {
        mouseMoveCallbacks = callbacks;
    }

    public void SubscribeMouseDragEvent(params System.Action[] callbacks)
    {
        mouseDragCallbacks = callbacks;
    }

    public void SubscribeMouseExitWindowsEvent(params System.Action[] callbacks)
    {
        mouseExitWindowsCallbacks = callbacks;
    }

    public void SubscribeMouseEnterWindowsEvent(params System.Action[] callbacks)
    {
        mouseEnterWindowsCallbacks = callbacks;
    }
    #endregion

    public void UnsubscribeAll(string key)
    {
        keysCallbacks.Clear();
        mouseButtonsCallbacks.Clear();

        keysCallbacks = null;
        mouseButtonsCallbacks = null;
        mouseMoveCallbacks = null;
    }


    void Update() {

        GetKeyboardInputs();
        GetMouseButtonsInputs();

        GetMouseMoveInput();
        GetMouseScrollInput();
        /* TODO later ??
        GetMouseDragInput();

        GetMouseExitWindowsInput();
        GetMouseEnterWindowsInput();*/
    }

    #region KeyboardEvents
    private void GetKeyboardInputs()
    {
        foreach(string key in keysCallbacks.Keys)
        {
            if (Input.GetKeyUp(key) && keysCallbacks[key][(int)EventTypeButton.Up] != null)
            {
                keysCallbacks[key][(int)EventTypeButton.Up]();
            }
            if (Input.GetKey(key) && keysCallbacks[key][(int)EventTypeButton.Hold] != null)
            {
                keysCallbacks[key][(int)EventTypeButton.Hold]();
            }
            if (Input.GetKeyDown(key) && keysCallbacks[key][(int)EventTypeButton.Down] != null)
            {
                keysCallbacks[key][(int)EventTypeButton.Down]();
            }
        }
    }
    #endregion

    # region MouseEvents
    private void GetMouseButtonsInputs()
    {
        foreach (int buttonID in mouseButtonsCallbacks.Keys)
        {
            if (Input.GetMouseButtonUp(buttonID) && mouseButtonsCallbacks[buttonID][(int)EventTypeButton.Up] != null)
            {
                mouseButtonsCallbacks[buttonID][(int)EventTypeButton.Up]();
            }
            if (Input.GetMouseButton(buttonID) && mouseButtonsCallbacks[buttonID][(int)EventTypeButton.Hold] != null)
            {
                mouseButtonsCallbacks[buttonID][(int)EventTypeButton.Hold]();
            }
            if (Input.GetMouseButtonDown(buttonID) && mouseButtonsCallbacks[buttonID][(int)EventTypeButton.Down] != null)
            {
                mouseButtonsCallbacks[buttonID][(int)EventTypeButton.Down]();
            }
        }
    }

    private void GetMouseMoveInput()
    {
        foreach (Action callback in mouseMoveCallbacks)
        {
            // Should pass the vector2 ?!
            callback();
        }
    }

    private void GetMouseScrollInput()
    {
        foreach (Action callback in mouseScrollCallbacks)
        {
            callback();
        }
    }
    #endregion

    /*
    void OnGUI()
    {
        Event e = Event.current;
        if (e.type != EventType.Repaint && e.type != EventType.Layout) 
        {
            Debug.Log(e.type);
        }
    }*/
}

