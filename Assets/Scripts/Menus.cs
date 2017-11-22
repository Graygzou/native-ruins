using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour {
  
    private void Start()
    {
        PlayerPrefs.SetInt("load", 0);
    }


    //Lance la scène Map Island et détruit la scène actuelle "Menu_demarrer"
    public void LancerPartie()
    {
        Debug.Log("Appuie sur lancer partie");
        //Permet de verifier qu'il y ait bien une sauvegarde
        if(PlayerPrefs.HasKey("xPlayer"))
        {
            PlayerPrefs.SetInt("load", 1);
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
        SceneManager.LoadScene("Map Island - final");
    }

    //Quitter le jeu
    public void Quitter()
    {
        Application.Quit();
    }

    //Recommencer permet de relancer le jeu avant la mort du personnage
    public void Recommencer()
    {
        LancerPartie();
    }

    //Permet de revenir au menu demarrer pour quitter le jeu
    public void QuitterMapIsland()
    {
        SceneManager.LoadScene("Menu_demarrer");
    }
}
