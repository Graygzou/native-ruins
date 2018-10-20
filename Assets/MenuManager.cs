using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    #region Enums
    public enum MenuPanel : int
    {
        Panel0 = 0,
        Panel1 = 1,
        Panel2 = 2,
        Panel3 = 3,
    }
    #endregion

    #region Singleton
    private static MenuManager _instance;

    public static MenuManager Instance { get { return _instance; } }
    #endregion

    #region Serialize fields
    [SerializeField]
    private GameObject pause;

    [SerializeField]
    private GameObject help;

    private Animator animator;
    #endregion

    protected void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        _instance.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause.SetActive(!pause.activeSelf);
        }

    }

    public void DisplayHelpMenu()
    {
        _instance.help.SetActive(true);
    }

    public void CloseHelpMenu()
    {
        _instance.help.SetActive(false);
    }

    public void TransitionToNextPanel(MenuPanel nextPanel)
    {
        // Enable the trigger for the given panel
        _instance.animator.SetInteger("PanelNumber", (int)nextPanel);
        _instance.animator.SetTrigger("Close");
    }

}
