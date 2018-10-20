using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    private string mainSceneName = "Map Island";

    [SerializeField]
    private Button loadButton;

    [SerializeField]
    private GameObject pause = null;

    [SerializeField]
    private GameObject help = null;

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

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause.SetActive(!pause.activeSelf);
        }

    }

    private void Start()
    {
        PlayerPrefs.SetInt("load_scene", 0);

        // Enable or not the launch game button
        if (PlayerPrefs.HasKey("xPlayer"))
        {
            loadButton.enabled = true;
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

    //Lance la scène Map Island et détruit la scène actuelle "Menu_demarrer"
    public void LancerPartie()
    {
        //Permet de verifier qu'il y ait bien une sauvegarde
        if(PlayerPrefs.HasKey("xPlayer"))
        {
            PlayerPrefs.SetInt("load_scene", 1);
            //Chargement de la scene
            SceneManager.LoadScene(PlayerPrefs.GetString("scene"));
        }
        else
        {
            Debug.LogWarning("The player should have saved data to be able to click on this button..");
        }
    }

    //Créer une nouvelle partie et écrase l'ancienne
    public void NouvellePartie()
    {
        SceneManager.LoadScene(mainSceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }

    public void TransitionToNextPanel(MenuPanel nextPanel)
    {
        // Enable the trigger for the given panel
        _instance.animator.SetInteger("PanelNumber", (int)nextPanel);
        _instance.animator.SetTrigger("Close");
    }

}
