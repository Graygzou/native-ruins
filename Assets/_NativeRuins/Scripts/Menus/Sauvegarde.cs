using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sauvegarde : MonoBehaviour {

    // Used to save and load the game
    private static GameObject Player;

    #region ComponentSettings
    [Header("Component settings")]
    [SerializeField]
    private GameObject gameHUD;
    [SerializeField]
    private LifeBar lifeBar;
    [SerializeField]
    private HungerBar hungerBar;
    [SerializeField]
    private InventoryManager inventory;
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

            RectTransform obj2D = GetObject2D(obj);
            // Create item in memory
            Debug.Log("obj = " + obj + " " + obj2D + ", amount = " + PlayerPrefs.GetInt("" + obj));
            inventory.AddObjectOfType(obj, PlayerPrefs.GetInt("" + obj));

            // Create item physically
            for (int indice = 0;  indice < PlayerPrefs.GetInt("" + obj); indice++)
            {
                RectTransform clone = Instantiate(obj2D) as RectTransform;
                clone.SetParent(inventory.GetBagRectTransform(), false);
            }
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
    void Awake() {

        Player = GameObject.Find("Player");

        //Si le bouton Lancer Partie du Menu principal a ete clique alors on charge les donnees
        if (PlayerPrefs.GetInt("load_scene") == 1)
        {
            Debug.Log("Chargement scene");

            float y = PlayerPrefs.GetFloat("xPlayer");
            float u = PlayerPrefs.GetFloat("yPlayer");
            float p = PlayerPrefs.GetFloat("zPlayer");

            //Positionnement du joueur
            Player.transform.position = new Vector3(PlayerPrefs.GetFloat("xPlayer"), PlayerPrefs.GetFloat("yPlayer"), PlayerPrefs.GetFloat("zPlayer"));

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

            //Chargement totems obtenus
            checkTotem(PlayerPrefs.GetInt("pumaUnlocked"), 0);
            checkTotem(PlayerPrefs.GetInt("bearUnlocked"), 1);

            //Enleve les totems deja trouves
            if (PlayerPrefs.GetInt("pumaUnlocked") == 1)
            {
                GameObject.FindWithTag("TotemPuma").SetActive(false);
            }
            if (PlayerPrefs.GetInt("pumaUnlocked") == 1)
            {
                GameObject.FindWithTag("TotemOurs").SetActive(false);
            }

            GameObject carreNoir = GameObject.Find("CameraCutscenes/Intro/PlaneFade");
            carreNoir.SetActive(false);
        } else {
            /*
            // Setting up the scene
            GameObject.Find("FirstCutSceneCamera").GetComponent<Camera>().enabled = true;
            GameObject.FindWithTag("Player").GetComponent<MovementControllerHuman>().enabled = false;
            GameObject.Find("Player").GetComponent<FormsController>().enabled = false;
            GameObject.FindWithTag("MainCamera").GetComponent<Camera>().enabled = false;

            // Execute the sleeping action
            GameObject.FindWithTag("Player").GetComponent<ActionsNew>().StartIntro();

            // Disabled UI
            gameHUD.GetComponent<CanvasGroup>().alpha = 0.0f;

            // Call dialogues
            DialogueTrigger.TriggerDialogueDebut(GameObject.Find("PlaneFade").GetComponent<FadeCutScene>());
            DialogueTrigger.TriggerDialogueDebut2(GameObject.Find("SecondCutSceneCamera").GetComponent<StandUpCutScene>());
            DialogueTrigger.TriggerDialogueDebut3(GameObject.Find("SecondCutSceneCamera").GetComponent<LookAroundCutScene>());
            DialogueTrigger.TriggerDialogueDebut4(GameObject.Find("ThirdCutSceneCamera").GetComponent<LostCutScene>());
            DialogueTrigger.TriggerDialogueDebut5(GameObject.Find("ThirdCutSceneCamera").GetComponent<FocusCutScene>());
            DialogueTrigger.TriggerDialogueDebut6(GameObject.Find("ForthCutSceneCamera").GetComponent<TitleGameCutScene>());*/
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
        PlayerPrefs.SetInt("pumaUnlocked", Player.GetComponent<FormsController>().isPumaUnlocked());
        PlayerPrefs.SetInt("bearUnlocked", Player.GetComponent<FormsController>().isBearUnlocked());

        //Afficher message de sauvegarde
        DialogueTrigger.TriggerSauvegarde(null);
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
