using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    #region Singleton
    private static MenuManager _instance;

    public static MenuManager Instance { get { return _instance; } }
    #endregion

    #region Serialize fields
    [SerializeField]
    private GameObject pause;

    [SerializeField]
    private GameObject help;
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

}
