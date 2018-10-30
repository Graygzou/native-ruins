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
    private GameObject[] managers;

    private IManager[] managersScripts;

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
        managersScripts = new IManager[managers.Length];
    }

    private void Update()
    {
        InputManager.GetVirtualButtonInputs();
    }

    private void Init(Scene scene, LoadSceneMode mode)
    {
        InitManagers();

        if (scene.name.Equals(_mainMenuName))
        {
            InitManagersMainMenuScene();

        }
        else if(scene.name.Equals(_mainSceneName))
        {
            // init all the managers when the introduction cutscene has ended
            InteractionManager.OnIntroCutsceneHasEnded += InitManagersMainScene;

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
        for (int i = 0; i < managers.Length; i++)
        {
            // Instantiate it
            managersScripts[i] = Instantiate(managers[i]).GetComponent<IManager>();

            // Call the init method on them
            managersScripts[i].Init();
        }
    }

    private void InitManagersMainScene()
    {
        foreach (IManager manager in managersScripts)
        {
            manager.InitMainScene();
        }
    }

    private void InitManagersMainMenuScene()
    {
        foreach (IManager manager in managersScripts)
        {
            manager.InitMainMenuScene();
        }
    }

    public IManager FindManager(ManagerName manageName)
    {   
        return managersScripts[(int)manageName] as IManager;
    }
    #endregion

}
