using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour {

    //Lance la scène Map Island et détruit la scène actuelle "Menu_demarrer"
	public void LancerPartie()
    {
        SceneManager.LoadScene("Map Island");
    }

    //Créer une nouvelle partie et écrase l'ancienne
    public void NouvellePartie()
    {
        
    }

    //Quitter le jeu
    public void Quitter()
    {
        Application.Quit();
    }

    //Sauvegarder le jeu
    public void Sauvegarder()
    {
        
    }

    //Recommencer permet de relancer le jeu avant la mort du personnage
    public void Recommencer()
    {

    }

    //Permet de revenir au menu demarrer pour quitter le jeu
    public void QuitterMapIsland()
    {
        SceneManager.LoadScene("Menu_demarrer");
    }
}
