﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sauvegarde : MonoBehaviour {

    private DialogueTrigger dialogue;
    private GameObject Player;
    private GameObject LifeBar;
    private GameObject HungerBar;
    private GameObject Inventory;

    [SerializeField] private RectTransform arrow2D;
    [SerializeField] private RectTransform bow2D;
    [SerializeField] private RectTransform fire2D;
    [SerializeField] private RectTransform flint2D;
    [SerializeField] private RectTransform meat2D;
    [SerializeField] private RectTransform mushroom2D;
    [SerializeField] private RectTransform plank2D;
    [SerializeField] private RectTransform raft2D;
    [SerializeField] private RectTransform sail2D;
    [SerializeField] private RectTransform rope2D;
    [SerializeField] private RectTransform torch2D;
    [SerializeField] private RectTransform wood2D;

    private RectTransform GetObject2D(ObjectsType obj)
    {
        switch (obj)
        {
            case ObjectsType.Arrow:
                return arrow2D;
            case ObjectsType.Bow:
                return bow2D;
            case ObjectsType.Fire:
                return fire2D;
            case ObjectsType.Flint:
                return flint2D;
            case ObjectsType.Meat:
                return meat2D;
            case ObjectsType.Mushroom:
                return mushroom2D;
            case ObjectsType.Plank:
                return plank2D;
            case ObjectsType.Raft:
                return raft2D;
            case ObjectsType.Rope:
                return rope2D;
            case ObjectsType.Sail:
                return sail2D;
            case ObjectsType.Torch:
                return torch2D;
            case ObjectsType.Wood:
                return wood2D;
            default:
                return null;
        }

    }


    private void addInInventory(ObjectsType obj)
    {
        if(PlayerPrefs.GetInt("" + obj) > 0)
        {
            if(obj.Equals(ObjectsType.Bow))
            {
                GameObject.Find("Terrain/Bow/Chest_bow/Particles_Fireflies").SetActive(false);
                GameObject.Find("Terrain/Bow/Bow3D").SetActive(false);
            }

            if (obj.Equals(ObjectsType.Rope))
            {
                GameObject.Find("EnigmeCorde/Corde/Particles_Fireflies").SetActive(false);
                GameObject.Find("EnigmeCorde/Corde/Rope3D").SetActive(false);
            }

            if (obj.Equals(ObjectsType.Sail))
            {
                GameObject.Find("EnigmeVoile/Voile/Particles_Fireflies").SetActive(false);
                GameObject.Find("EnigmeVoile/Voile/Fabric3D").SetActive(false);
            }
        }
        
        int indice = 0;
        RectTransform obj2D = GetObject2D(obj);
        Debug.Log("obj = " + obj + " " + obj2D);
        while (indice < PlayerPrefs.GetInt("" + obj))
        {
            InventoryManager.AddObjectOfType(obj);
            RectTransform clone = Instantiate(obj2D) as RectTransform;
            clone.SetParent(GameObject.Find("InventoryManager/Canvas/Bag").transform, false);
            indice++;
        }
    }

    private void checkTotem(int totemBool, int typeForm)
    {
        FormsController script = Player.GetComponent<FormsController>();
        if (typeForm == 0) //Puma
        {
            if (totemBool == 0) //Pas de totem
            {
                script.setPumaUnlocked(false);
            }
            else //Totem
            {
                script.setPumaUnlocked(true);
            }
        }
        else //Ours
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

    // Use this for initialization
    void Start () {
        Player = GameObject.Find("Player");
        LifeBar = GameObject.Find("Affichages/Gauges/Life");
        HungerBar = GameObject.Find("Affichages/Gauges/Hunger");
        Inventory = GameObject.Find("Affichages/InventoryManager");
        int r = PlayerPrefs.GetInt("load_scene");
        //Si le bouton Lancer Partie du Menu principal a ete clique alors on charge les donnees
        if (PlayerPrefs.GetInt("load_scene")==1)
        {
            Debug.Log("Chargement scene");

            //Positionnement du joueur
            Player.transform.position = new Vector3(PlayerPrefs.GetFloat("xPlayer"), PlayerPrefs.GetFloat("yPlayer"), PlayerPrefs.GetFloat("zPlayer"));

            //Jauges de vie et faim
            LifeBar.GetComponent<LifeBar>().setSizeLifeBar(PlayerPrefs.GetFloat("life"));
            HungerBar.GetComponent<HungerBar>().setSizeHungerBar(PlayerPrefs.GetFloat("hunger"));

            //Chargement de l'inventaire
            Inventory.GetComponent<InventoryManager>().GetInventory().Clear();
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

            //Enleve les totems, voile et corde deja trouves
            if(PlayerPrefs.GetInt("pumaUnlocked") == 1)
            {
                GameObject.FindWithTag("TotemPuma").SetActive(false);
            }
            if (PlayerPrefs.GetInt("pumaUnlocked") == 1)
            {
                GameObject.FindWithTag("TotemOurs").SetActive(false);
            }
            //Reactualisation des animaux et des objets sur la map

        }
    }


    //Sauvegarder le jeu
    public void Sauvegarder()
    {
        //Nom de la scene
        PlayerPrefs.SetString("scene", SceneManager.GetActiveScene().name);

        //Position du joueur
        PlayerPrefs.SetFloat("xPlayer", Player.transform.position.x);
        PlayerPrefs.SetFloat("yPlayer", Player.transform.position.y);
        PlayerPrefs.SetFloat("zPlayer", Player.transform.position.z);

        //Jauges vie et faim
        PlayerPrefs.SetFloat("life", LifeBar.GetComponent<LifeBar>().getSizeLifeBar());
        PlayerPrefs.SetFloat("hunger", HungerBar.GetComponent<HungerBar>().getSizeHungerBar());

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
        foreach (ObjectsType obj in Inventory.GetComponent<InventoryManager>().GetInventory())
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
        PlayerPrefs.SetInt("pumaUnlocked", Player.GetComponent<FormsController>().isPumaUnlocked());
        PlayerPrefs.SetInt("bearUnlocked", Player.GetComponent<FormsController>().isBearUnlocked());

        //Afficher message de sauvegarde
        dialogue = GameObject.Find("Affichages/Dialogues/DialogueTrigger").GetComponent<DialogueTrigger>();
        dialogue.TriggerSauvegarde();
    }

    //Permet de revenir au menu demarrer pour quitter le jeu
    public void QuitterMapIsland()
    {
        SceneManager.LoadScene("Menu_demarrer");
    }

    //Recommencer permet de relancer le jeu avant la mort du personnage
    public void Recommencer()
    {
        //Permet de verifier qu'il y ait bien une sauvegarde
        if (PlayerPrefs.HasKey("xPlayer"))
        {
            PlayerPrefs.SetInt("load_scene", 1);
            //Chargement de la scene
            SceneManager.LoadScene(PlayerPrefs.GetString("scene"));

        }
        else //creer une nouvelle partie
        {
            SceneManager.LoadScene("Map Island - final");
        }
    }
}