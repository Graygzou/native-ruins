using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickTransition : MonoBehaviour {

    [SerializeField]
    private MenuManager.MenuPanel transitionTo;

    public void TransitionToNextPanel()
    {
        // Enable the trigger for the given panel
        MenuManager.Instance.TransitionToNextPanel(transitionTo);
    }
}
