using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : Manager {

    #region Enums
    public enum MenuPanel : int
    {
        MainMenu = 0,
        Options = 1,
        Credits = 2,
        Pause = 3,
        Controls = 4,
    }
    #endregion

    #region Singleton
    private static MenuManager _instance;

    public static MenuManager Instance { get { return _instance; } }
    #endregion

    [Header("Scenes names")]
    [SerializeField]
    private string mainMenuName = "MenuDemarrer";
    [SerializeField]
    private string mainSceneName = "MapIslandNew";

    [Header("UI Prefabs")]
    [SerializeField]
    private GameObject mainUIPrefab;
    private GameObject _currentMainUI;

    [SerializeField]
    private GameObject inGameUIPrefab;
    private GameObject _currentInGameUI;

    [Header("Others settings")]
    [SerializeField]
    private Button loadButton;
    [SerializeField]
    private GameObject pause = null;
    [SerializeField]
    private GameObject help = null;
    [SerializeField]
    private MainUI mainUIScript;

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
        Init();
    }

    private void Start()
    {
        Canvas canvas = _currentMainUI.GetComponent<Canvas>();
        if (SceneManager.GetActiveScene().name.Equals(mainMenuName))
        {
            // Set the canvas's render mode to Screen Space - Camera
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = Camera.main;

            // Let know the animator of events
            mainUIScript.MainMenuAnimator.SetTrigger("Open");
            mainUIScript.MainMenuAnimator.SetInteger("PanelNumber", (int)MenuPanel.MainMenu);
        }
        else
        {
            // Set the canvas's render mode to World Space Render
            canvas.renderMode = RenderMode.WorldSpace;
            // Let know the animator of that
            mainUIScript.MainMenuAnimator.SetTrigger("Open");
            mainUIScript.MainMenuAnimator.SetInteger("PanelNumber", (int)MenuPanel.Pause);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("Info: Load Level Number: " + level);

        Init();

        switch (level)
        {
            case 0:
                // Nothing more
                break;
            case 1:
                InitMapIsland();
                break;
            default:
                // Nothing here
                break;
        }
        
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause.SetActive(!pause.activeSelf);
        }
    }

    #region Level UI init
    private void Init()
    {
        // Create the UI prefab if needed
        if (!GameObject.FindGameObjectWithTag(mainUIPrefab.tag))
        {
            Debug.Log("Info: Create mainUI in the hierarchy");
            _currentMainUI = Instantiate(mainUIPrefab);
        }
        else
        {
            _currentMainUI = GameObject.FindGameObjectWithTag(mainUIPrefab.tag);
            // Enable or not the launch game button
            PlayerPrefs.SetInt("load_scene", 0);
            loadButton.enabled = PlayerPrefs.HasKey("xPlayer");
        }
        mainUIScript = _currentMainUI.GetComponent<MainUI>();
    }

    private void InitMapIsland()
    {
        // Create the HUD if needed
        if (!GameObject.FindGameObjectWithTag(inGameUIPrefab.tag))
        {
            Debug.Log("Info: Create InGameUI in the hierarchy");
            _currentMainUI = Instantiate(inGameUIPrefab);
        }
        else
        {
            _currentMainUI = GameObject.FindGameObjectWithTag(inGameUIPrefab.tag);
            // Enable or not the launch game button
            PlayerPrefs.SetInt("load_scene", 0);
            loadButton.enabled = PlayerPrefs.HasKey("xPlayer");
        }
    }
    #endregion

    public void DisplayHelpMenu()
    {
        _instance.help.SetActive(true);
    }

    public void CloseHelpMenu()
    {
        _instance.help.SetActive(false);
    }

    // Créer une nouvelle partie et écrase l'ancienne
    public void NouvellePartie()
    {
        SceneManager.LoadScene(mainSceneName);
    }

    public void TransitionToNextPanelMain(MenuPanel nextPanel)
    {
        // Enable the trigger for the given panel
        mainUIScript.MainMenuAnimator.SetInteger("PanelNumber", (int)nextPanel);
        mainUIScript.MainMenuAnimator.SetTrigger("Close");
    }

}
