using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour {

    private void addInInventory(ObjectsType obj)
    {
        int indice = 0;
        while (indice < PlayerPrefs.GetInt("" + obj))
        {
            InventoryManager.AddObjectOfType(obj);
        }
    }

    private void checkTotem(int totemBool, int typeForm)
    {
        FormsController script = GameObject.Find("Player").GetComponent<FormsController>();
        if (typeForm == 0) //Puma
        {
            if (totemBool == 0) //Pas de totem
            {
                script.setPumaUnlocked(false);
            } else //Totem
            {
                script.setPumaUnlocked(true);
            }
        } else //Ours
        {
            if (totemBool == 0) //Pas de totem
            {
                script.setBearUnlocked(false);
            }
            else //Totem
            {
                script.setBearUnlocked(true);
            }
        }
        
    }

    //Lance la scène Map Island et détruit la scène actuelle "Menu_demarrer"
	public void LancerPartie()
    {
        //Permet de verifier qu'il y ait bien une sauvegarde
        if(PlayerPrefs.HasKey("xPlayer"))
        {
            //Chargement de la scene
            SceneManager.LoadScene(PlayerPrefs.GetString("scene"));

            //Positionnement du joueur
            GameObject.Find("Player").transform.position = new Vector3(PlayerPrefs.GetFloat("xPlayer"), PlayerPrefs.GetFloat("yPlayer"), PlayerPrefs.GetFloat("zPlayer"));

            //Jauges de vie et faim
            GameObject.Find("Gauges/Life").GetComponent<LifeBar>().setSizeLifeBar(PlayerPrefs.GetFloat("life"));
            GameObject.Find("Gauges/Hunger").GetComponent<HungerBar>().setSizeHungerBar(PlayerPrefs.GetFloat("hunger"));

            //Chargement de l'inventaire
            GameObject.Find("InventoryManager").GetComponent<InventoryManager>().GetInventory().Clear();
            addInInventory(ObjectsType.Arrow);
            addInInventory(ObjectsType.Bow);
            addInInventory(ObjectsType.Fire);
            addInInventory(ObjectsType.Flint);
            addInInventory(ObjectsType.Meat);
            addInInventory(ObjectsType.Mushroom);
            addInInventory(ObjectsType.Plank);
            addInInventory(ObjectsType.Raft);
            addInInventory(ObjectsType.Sail);
            addInInventory(ObjectsType.Rope);
            addInInventory(ObjectsType.Torch);
            addInInventory(ObjectsType.Wood);
            
            //Chargement totems obtenus
            checkTotem(PlayerPrefs.GetInt("pumaUnlocked"), 0);
            checkTotem(PlayerPrefs.GetInt("bearUnlocked"), 1);

            //Reactualisation des animaux et des objets sur la map


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

    //Sauvegarder le jeu
    public void Sauvegarder()
    {
        //Nom de la scene
        PlayerPrefs.SetString("scene",SceneManager.GetActiveScene().name);

        //Position du joueur
        PlayerPrefs.SetFloat("xPlayer",GameObject.Find("Player").transform.position.x);
        PlayerPrefs.SetFloat("yPlayer", GameObject.Find("Player").transform.position.y);
        PlayerPrefs.SetFloat("zPlayer", GameObject.Find("Player").transform.position.z);

        //Jauges vie et faim
        PlayerPrefs.SetFloat("life", GameObject.Find("Gauges/Life").GetComponent<LifeBar>().getSizeLifeBar());
        PlayerPrefs.SetFloat("hunger", GameObject.Find("Gauges/Hunger").GetComponent<HungerBar>().getSizeHungerBar());

        //Inventaire
        int nbArrow = 0;
        int nbMushroom = 0;
        int nbMeat = 0;
        int nbFlint = 0;
        int nbWood = 0;
        int nbBow = 0;
        int nbTorch = 0;
        int nbFire = 0;
        int nbPlank = 0;
        int nbSail = 0;
        int nbRope = 0;
        int nbRaft = 0;
        foreach (ObjectsType obj in GameObject.Find("InventoryManager").GetComponent<InventoryManager>().GetInventory())
        {
            if (obj == ObjectsType.Arrow) nbArrow++;
            if (obj == ObjectsType.Bow) nbBow++;
            if (obj == ObjectsType.Fire) nbFire++;
            if (obj == ObjectsType.Flint) nbFlint++;
            if (obj == ObjectsType.Meat) nbMeat++;
            if (obj == ObjectsType.Mushroom) nbMushroom++;
            if (obj == ObjectsType.Plank) nbPlank++;
            if (obj == ObjectsType.Raft) nbRaft++;
            if (obj == ObjectsType.Rope) nbRope++;
            if (obj == ObjectsType.Sail) nbSail++;
            if (obj == ObjectsType.Torch) nbTorch++;
            if (obj == ObjectsType.Wood) nbWood++;
        }
        PlayerPrefs.SetInt("" + ObjectsType.Arrow, nbArrow);
        PlayerPrefs.SetInt("" + ObjectsType.Mushroom, nbMushroom);
        PlayerPrefs.SetInt("" + ObjectsType.Meat, nbMeat);
        PlayerPrefs.SetInt("" + ObjectsType.Flint, nbFlint);
        PlayerPrefs.SetInt("" + ObjectsType.Wood, nbWood);
        PlayerPrefs.SetInt("" + ObjectsType.Bow, nbBow);
        PlayerPrefs.SetInt("" + ObjectsType.Torch, nbTorch);
        PlayerPrefs.SetInt("" + ObjectsType.Fire, nbFire);
        PlayerPrefs.SetInt("" + ObjectsType.Plank, nbPlank);
        PlayerPrefs.SetInt("" + ObjectsType.Sail, nbSail);
        PlayerPrefs.SetInt("" + ObjectsType.Rope, nbRope);
        PlayerPrefs.SetInt("" + ObjectsType.Raft, nbRaft);

        //Connaitre transformation debloquee
        PlayerPrefs.SetInt("pumaUnlocked", GameObject.Find("Player").GetComponent<FormsController>().isPumaUnlocked());
        PlayerPrefs.SetInt("bearUnlocked", GameObject.Find("Player").GetComponent<FormsController>().isBearUnlocked());
        
        //Afficher message de sauvegarde
        DialogueTrigger dialogue = new DialogueTrigger();
        dialogue.TriggerSauvegarde();
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
