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
        DialogueTrigger = 3
    }
    #endregion

    #region Singleton
    private static MainManager _instance;

    public static MainManager Instance { get { return _instance; } }
    #endregion

    [Header("Scenes names")]
    [SerializeField]
    private string mainMenuName = "MenuDemarrer";
    [SerializeField]
    private string mainSceneName = "NewMapIsland";

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

    private void Init(Scene scene, LoadSceneMode mode)
    {
        if(scene.name.Equals(mainMenuName))
        {
            InitManagers();

        }
        else if(scene.name.Equals(mainSceneName))
        {
            // init all the managers when the introduction cutscene has ended
            InteractionManager.OnIntroCutsceneHasEnded += InitManagersMainScene;

            // Start the first cutscene
            (FindManager(ManagerName.DialogueTrigger) as InteractionManager).StartCutscene(CutScene.CutsceneName.IntroductionCutscene);

            // TODO Subscribe the escape button if the player want to escape the cutscene.
            //InputManager.SubscribeButtonEvent(InputManager.ActionsLabels.Cancel, "Cancel", InputManager.EventTypeButton.Down, (FindManager(ManagerName.AudioManager) as DialogueTrigger).SkipCutscene);
        }
    }

    // Créer une nouvelle partie et écrase l'ancienne
    public void NouvellePartie()
    {
        (FindManager(ManagerName.AudioManager) as AudioManager).FadeDown();
        SceneManager.LoadScene(mainSceneName);
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
