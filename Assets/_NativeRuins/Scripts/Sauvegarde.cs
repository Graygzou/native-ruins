﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sauvegarde : MonoBehaviour, IManager {

    // Used to save and load the game
    [SerializeField]
    private PlayerProperties player;

    #region ComponentSettings
    [Header("Component settings")]
    [SerializeField] private GameObject gameHUD;
    [SerializeField] private LifeBar lifeBar;
    [SerializeField] private HungerBar hungerBar;
    [SerializeField] private InventoryManager inventory;

    [Header("Puzzles rewards")]
    [SerializeField] private GameObject firstRope;
    [SerializeField] private GameObject firstSail;
    [SerializeField] private GameObject firstBow;
    #endregion

    [Header("Items settings")]
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

    public void Init()
    {
        // Nothing yet ?
    }

    public void InitMainMenuScene()
    {
        // Nothing Yet ?
    }

    public void InitMainScene()
    {
        if(player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerProperties>();
        }

        //Si le bouton Lancer Partie du Menu principal a ete clique alors on charge les donnees
        if (PlayerPrefs.GetInt("load_scene") == 1)
        {
            Debug.Log("Chargement scene");

            float y = PlayerPrefs.GetFloat("xPlayer");
            float u = PlayerPrefs.GetFloat("yPlayer");
            float p = PlayerPrefs.GetFloat("zPlayer");

            //Positionnement du joueur
            player.transform.position = new Vector3(PlayerPrefs.GetFloat("xPlayer"), PlayerPrefs.GetFloat("yPlayer"), PlayerPrefs.GetFloat("zPlayer"));

            //Jauges de vie et faim
            Debug.Log("Life :" + PlayerPrefs.GetFloat("life") + ", Hunger :" + PlayerPrefs.GetFloat("hunger"));
            lifeBar.GetComponent<LifeBar>().RestoreLifeFromData(PlayerPrefs.GetFloat("life"));
            hungerBar.GetComponent<HungerBar>().RestoreHungerFromData(PlayerPrefs.GetFloat("hunger"));

            //Chargement de l'inventaire
            inventory.EmptyBag();

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

            //Chargement des formes obtenus
            ActivateForms();

            //Enleve les totems deja trouves
            if (PlayerPrefs.GetInt("pumaUnlocked") == 1)
            {
                GameObject.FindWithTag("TotemPuma").SetActive(false);
            }
            if (PlayerPrefs.GetInt("pumaUnlocked") == 1)
            {
                GameObject.FindWithTag("TotemOurs").SetActive(false);
            }
        }

        // Trigger the idle pose
        player.Idle();

        // Enable player movements
        player.EnableMovementController(TransformationType.Human);
    }

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
        if (PlayerPrefs.GetInt("" + obj) > 0)
        {
            if(obj.Equals(ObjectsType.Bow))
            {
                firstBow.GetComponent<ParticleSystem>().Stop();
                firstBow.SetActive(false);
            }

            if (obj.Equals(ObjectsType.Rope))
            {
                firstRope.GetComponent<ParticleSystem>().Stop();
                firstRope.SetActive(false);
            }

            if (obj.Equals(ObjectsType.Sail))
            {
                firstSail.GetComponent<ParticleSystem>().Stop();
                firstSail.SetActive(false);
            }

            RectTransform obj2D = GetObject2D(obj);
            // Create item in memory
            Debug.Log("obj = " + obj + " " + obj2D + ", amount = " + PlayerPrefs.GetInt("" + obj));

            // Create item physically
            inventory.AddObjectOfType(obj, obj2D, PlayerPrefs.GetInt("" + obj));
        }
    }

    private void ActivateForms()
    {
        FormsController script = player.GetComponent<FormsController>();
        foreach(TransformationType type in script.GetAvailableForms())
        {
            script.SetFormState(TransformationType.Bear, PlayerPrefs.GetInt(type.ToString() + "Unlocked") != 0);
        }
    }

    public void EnableUI() {
        gameHUD.GetComponent<CanvasGroup>().alpha = 1.0f;
    }


    //Sauvegarder le jeu
    public void Sauvegarder()
    {
        //Nom de la scene
        PlayerPrefs.SetString("scene", SceneManager.GetActiveScene().name);

        //Position du joueur
        GameObject Judy = GameObject.FindWithTag("Player");
        PlayerPrefs.SetFloat("xPlayer", Judy.transform.position.x);
        PlayerPrefs.SetFloat("yPlayer", Judy.transform.position.y);
        PlayerPrefs.SetFloat("zPlayer", Judy.transform.position.z);

        //Jauges vie et faim
        PlayerPrefs.SetFloat("life", lifeBar.GetComponent<LifeBar>().GetCurrentSizeLifeBar());
        PlayerPrefs.SetFloat("hunger", hungerBar.GetComponent<HungerBar>().GetSizeHungerBar());

        PlayerPrefs.SetInt("" + ObjectsType.Arrow, inventory.GetNumberItems(ObjectsType.Arrow));
        PlayerPrefs.SetInt("" + ObjectsType.Mushroom, inventory.GetNumberItems(ObjectsType.Mushroom));
        PlayerPrefs.SetInt("" + ObjectsType.Meat, inventory.GetNumberItems(ObjectsType.Meat));
        PlayerPrefs.SetInt("" + ObjectsType.Flint, inventory.GetNumberItems(ObjectsType.Flint));
        PlayerPrefs.SetInt("" + ObjectsType.Wood, inventory.GetNumberItems(ObjectsType.Wood));
        PlayerPrefs.SetInt("" + ObjectsType.Bow, inventory.GetNumberItems(ObjectsType.Bow));
        PlayerPrefs.SetInt("" + ObjectsType.Torch, inventory.GetNumberItems(ObjectsType.Torch));
        PlayerPrefs.SetInt("" + ObjectsType.Fire, inventory.GetNumberItems(ObjectsType.Fire));
        PlayerPrefs.SetInt("" + ObjectsType.Plank, inventory.GetNumberItems(ObjectsType.Plank));
        PlayerPrefs.SetInt("" + ObjectsType.Sail, inventory.GetNumberItems(ObjectsType.Sail));
        PlayerPrefs.SetInt("" + ObjectsType.Rope, inventory.GetNumberItems(ObjectsType.Rope));
        PlayerPrefs.SetInt("" + ObjectsType.Raft, inventory.GetNumberItems(ObjectsType.Raft));

        //Connaitre transformation debloquee
        PlayerPrefs.SetInt(TransformationType.Puma.ToString() + "Unlocked", player.GetComponent<FormsController>().IsFormUnlocked(TransformationType.Puma));
        PlayerPrefs.SetInt(TransformationType.Bear.ToString() + "Unlocked", player.GetComponent<FormsController>().IsFormUnlocked(TransformationType.Bear));

        //Afficher message de sauvegarde
        // TODO DialogueTrigger.TriggerSauvegarde(null);
    }

    //Permet de revenir au menu demarrer pour quitter le jeu
    public void QuitterMapIsland()
    {
        inventory.EmptyBag();
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
            SceneManager.LoadScene("Map Island");
        }
    }
}