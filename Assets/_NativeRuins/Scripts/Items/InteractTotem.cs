using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTotem : MonoBehaviour {

    private bool o_isTotem = false;
    private GameObject judy;
    private AudioSource son;

    [SerializeField]
    private DialogueTrigger dialogue;
    [SerializeField]
    private GameObject buttonInteragir;
    [SerializeField]
    private GameObject particuleEffect;

    // Use this for initialization
    void Start()
    {
        son = this.GetComponentInParent<AudioSource>();
        judy = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            o_isTotem = false;
            buttonInteragir.SetActive(false);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") && !InventoryManager.instance.bag_open)
        {
            o_isTotem = true;
            buttonInteragir.SetActive(true);
        }
    }

    private void GetInputs()
    {
        if (Input.GetKeyDown(KeyCode.E) && o_isTotem)
        {
            if (judy.GetComponent<FormsController>().isBearUnlocked() == 0) //Si il y a encore le totem ours
            {
                //Range arme avant d'ouvrir le menu de sauvegarde
                GameObject.Find("InventoryManager").GetComponent<InventoryManager>().PutWeaponInBag();
                GameObject playerRoot = GameObject.Find("Player");
                playerRoot.GetComponent<FormsController>().Transformation(0);
                son.Play();
                particuleEffect.SetActive(false);
                judy.GetComponent<FormsController>().setBearUnlocked(true);
                DialogueTrigger.TriggerDialogueTotemOurs(null);
            }
            else //le totem ours a deja ete recupere donc il ne reste que celui du puma
            {
                //Range arme avant d'ouvrir le menu de sauvegarde
                GameObject.Find("InventoryManager").GetComponent<InventoryManager>().PutWeaponInBag();
                GameObject playerRoot = GameObject.Find("Player");
                playerRoot.GetComponent<FormsController>().Transformation(0);
                son.Play();
                particuleEffect.SetActive(false);
                judy.GetComponent<FormsController>().setPumaUnlocked(true);
                DialogueTrigger.TriggerDialogueTotemPuma(null);
            }
            // Disable the totem
            gameObject.SetActive(false);
            o_isTotem = false;
            buttonInteragir.SetActive(false);
            judy.GetComponentInChildren<ActionsNew>().Stay(100f);
        }
    }
}
