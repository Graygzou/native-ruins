using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour {

    #region Enums
    public enum ManagerName : int
    {
        AudioManager = 0,
        MenuManager = 1,
        ScriptManager = 2,
        InteractionManager = 3
    }
    #endregion

    #region Singleton
    private static MainManager _instance;

    public static MainManager Instance { get { return _instance; } }
    #endregion

    [Header("Scenes names")]
    [SerializeField]
    private string _mainMenuName = "MenuDemarrer";
    public string MainMenuName { get { return _mainMenuName; } }

    [SerializeField]
    private string _mainSceneName = "NewMapIsland";
    public string MainSceneName { get { return _mainSceneName; } }

    [SerializeField]
    private MonoBehaviour[] managers;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        // This manager will be persistent
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += Init;
    }

    private void Update()
    {
        InputManager.GetVirtualButtonInputs();
    }

    private void Init(Scene scene, LoadSceneMode mode)
    {
        if(scene.name.Equals(_mainMenuName))
        {
            InitManagers();

        }
        else if(scene.name.Equals(_mainSceneName))
        {
            // init all the managers when the introduction cutscene has ended
            InteractionManager.OnIntroCutsceneHasEnded += InitManagersMainScene;

            // Subscribe the escape key so the player can escape the cutscene.
            InputManager.SubscribeButtonEvent(InputManager.ActionsLabels.Cancel, "Cancel", InputManager.EventTypeButton.Down, (FindManager(ManagerName.InteractionManager) as InteractionManager).SkipCutscene);

            // Start the first cutscene
            (FindManager(ManagerName.InteractionManager) as InteractionManager).Init();
            (FindManager(ManagerName.InteractionManager) as InteractionManager).StartCutscene(CutScene.InGameCutsceneName.IntroductionCutscene);
        }
    }

    // Créer une nouvelle partie et écrase l'ancienne
    public void NouvellePartie()
    {
        (FindManager(ManagerName.AudioManager) as AudioManager).FadeDown();
        SceneManager.LoadScene(_mainSceneName);
    }

    #region Managers methods
    private void InitManagers()
    {
        foreach(IManager manager in managers)
        {
            manager.Init();
        }
    }

    private void InitManagersMainScene()
    {
        foreach (IManager manager in managers)
        {
            manager.InitMainScene();
        }
    }

    public IManager FindManager(ManagerName manageName)
    {   
        return managers[(int)manageName] as IManager;
    }
    #endregion

}
