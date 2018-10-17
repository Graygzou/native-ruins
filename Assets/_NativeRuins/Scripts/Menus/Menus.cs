using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menus : MonoBehaviour {

    [SerializeField]
    private string mainSceneName = "Map Island";

    [SerializeField]
    private Button loadButton;

    private void Start()
    {
        PlayerPrefs.SetInt("load_scene", 0);

        // Enable or not the launch game button
        if (PlayerPrefs.HasKey("xPlayer"))
        {
            loadButton.enabled = true;
        }
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


}
