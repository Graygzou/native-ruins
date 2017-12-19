using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour {
  
    private void Start()
    {
        PlayerPrefs.SetInt("load_scene", 0);
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

        } else //creer une nouvelle partie
        {
            NouvellePartie();
        }
    }

    //Créer une nouvelle partie et écrase l'ancienne
    public void NouvellePartie()
    {
        SceneManager.LoadScene("Map Island");
    }

    //Quitter le jeu
    public void Quitter()
    {
        Application.Quit();
    }

    
}
