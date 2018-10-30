using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour, IManager {

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

    [Header("UI Prefabs")]
    [SerializeField]
    private GameObject mainUIPrefab;
    private GameObject _currentMainUI;
    private MainUI _mainUIScript;

    [SerializeField]
    private GameObject inGameUIPrefab;
    private GameObject _currentInGameUI;
    private InGameUI _InGameUIScript;

    [Header("Others Settings")]
    [SerializeField]
    private GameObject pause = null;
    [SerializeField]
    private GameObject help = null;

    private Canvas _mainUICanvas;

    #region Init methods
    void IManager.Init()
    {
        // Create the UI prefab if needed
        RetrieveOrCreateMainUI();

        // Enable or not the launch game button
        PlayerPrefs.SetInt("load_scene", 0);
        _mainUIScript.ChangeStateLoadButton(PlayerPrefs.HasKey("xPlayer"));
        

        // Set the canvas's render mode to Screen Space - Camera
        _mainUICanvas.renderMode = RenderMode.ScreenSpaceCamera;
        _mainUICanvas.worldCamera = Camera.main;

        _mainUIScript.Title.SetActive(true);
        // Let know the animator of events
        _mainUIScript.MainMenuAnimator.SetTrigger("Open");
        _mainUIScript.MainMenuAnimator.SetInteger("PanelNumber", (int)MenuPanel.MainMenu);
    }

    void IManager.InitMainScene()
    {
        RetrieveOrCreateMainUI();
        // Create the HUD if needed
        RetrieveOrCreatedInGameUI();

        DisplayHUD();

        // Se fait connaitre aupres du joueur
        GameObject.FindWithTag("Player").GetComponent<PlayerProperties>().SetMenuManager(this);

        // Set the canvas's render mode to World Space Render
        _mainUICanvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }
    #endregion

    #region MainUI Methods

    public void RetrieveOrCreateMainUI ()
    {
        if (!GameObject.FindGameObjectWithTag(mainUIPrefab.tag))
        {
            Debug.Log("Info: Create mainUI in the hierarchy");
            _currentMainUI = Instantiate(mainUIPrefab);
        }
        else
        {
            _currentMainUI = GameObject.FindGameObjectWithTag(mainUIPrefab.tag);
        }
        _mainUICanvas = _currentMainUI.GetComponent<Canvas>();
        _mainUIScript = _currentMainUI.GetComponent<MainUI>();
    }

    public void TransitionToNextPanelMain(MenuPanel nextPanel)
    {
        // Enable the trigger for the given panel
        _mainUIScript.MainMenuAnimator.SetInteger("PanelNumber", (int)nextPanel);
        _mainUIScript.MainMenuAnimator.SetTrigger("Close");
    }

    #endregion

    #region InGameUI Methods

    private void RetrieveOrCreatedInGameUI()
    {
        if (!GameObject.FindGameObjectWithTag(inGameUIPrefab.tag))
        {
            Debug.Log("Info: Create InGameUI in the hierarchy");
            _currentInGameUI = Instantiate(inGameUIPrefab);
        }
        else
        {
            _currentInGameUI = GameObject.FindGameObjectWithTag(inGameUIPrefab.tag);
        }
        _InGameUIScript = _currentInGameUI.GetComponent<InGameUI>();
    }

    public void DisplayHUD()
    {
        _InGameUIScript.DisplayHUD();
    }

    public void EnableCrossHair()
    {
        _currentInGameUI.GetComponent<InGameUI>().EnableCrossHair();
    }

    public void DisplayTransformationWheel()
    {
        _InGameUIScript.DisplayTransformationWheel();
    }

    public void CloseTransformationWheel()
    {
        _InGameUIScript.CloseTransformationWheel();
    }

    public float GetCurrentSizeLifeBar()
    {
        return _InGameUIScript.lifeBar.GetCurrentSizeLifeBar();
    }

    public void UpdateLifeBar(float amount)
    {
        _InGameUIScript.lifeBar.ChangeLifeBar(amount);
    }

    public float GetSizeHungerBar()
    {
        return _InGameUIScript.hungerBar.GetSizeHungerBar();
    }

    public void UpdateHungerBar(float amount)
    {
        _InGameUIScript.hungerBar.ChangeHungerBar(amount);
    }

    public float GetSizeEnergyBar()
    {
        return _InGameUIScript.energyBar.GetCurrentEnergy();
    }

    public void UpdateEnergyBar(float amount)
    {
        _InGameUIScript.energyBar.SetSizeEnergyBar(amount);
    }

    public bool IsLifeBarFull()
    {
        return _InGameUIScript.lifeBar.IsFull();
    }
    #endregion

}
