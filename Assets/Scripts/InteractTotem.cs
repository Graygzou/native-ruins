using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTotem : MonoBehaviour {

    private bool o_isTotem = false;
    private GameObject Judy;
    private AudioSource son;
    private DialogueTrigger dialogue;

    // Use this for initialization
    void Start()
    {
        son = this.GetComponentInParent<AudioSource>();
        dialogue = GameObject.Find("Affichages/Dialogues/DialogueTrigger").GetComponent<DialogueTrigger>();
        Judy = GameObject.Find("Player");
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
            GameObject.Find("Affichages/Interaction/ButtonInteragir").SetActive(false);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") && !InventoryManager.bag_open)
        {
            o_isTotem = true;
            GameObject.Find("Affichages/Interaction/ButtonInteragir").SetActive(true);
        }
    }

    private void GetInputs()
    {
        if (Input.GetKeyDown(KeyCode.E) && o_isTotem)
        {
            
            
            if (Judy.GetComponent<FormsController>().isBearUnlocked() == 0) //Si il y a encore le totem ours
            {
                //Range arme avant d'ouvrir le menu de sauvegarde
                GameObject.Find("InventoryManager").GetComponent<InventoryManager>().PutWeaponInBag();
                GameObject playerRoot = GameObject.Find("Player");
                playerRoot.GetComponent<FormsController>().Transformation(0);
                son.Play();
                GameObject.Find("EnigmeTotemOurs/pfb_EarthTotem").SetActive(false);
                Judy.GetComponent<FormsController>().setBearUnlocked(true);
                dialogue.TriggerDialogueTotemOurs();
            } else //le totem ours a deja ete recupere donc il ne reste que celui du puma
            {
                //Range arme avant d'ouvrir le menu de sauvegarde
                GameObject.Find("InventoryManager").GetComponent<InventoryManager>().PutWeaponInBag();
                GameObject playerRoot = GameObject.Find("Player");
                playerRoot.GetComponent<FormsController>().Transformation(0);
                son.Play();
                GameObject.Find("EnigmeTotemPuma/pfb_AirTotem").SetActive(false);
                Judy.GetComponent<FormsController>().setPumaUnlocked(true);
                dialogue.TriggerDialogueTotemPuma();
            }
            o_isTotem = false;
            GameObject.Find("Affichages/Interaction/ButtonInteragir").SetActive(false);
        }
    }
}
