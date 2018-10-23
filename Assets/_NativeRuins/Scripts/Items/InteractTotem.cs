using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTotem : MonoBehaviour {

    [SerializeField]
    private InteractionManager dialogue;
    [SerializeField]
    private GameObject buttonInteragir;

    private bool o_isTotem = false;
    private GameObject judy;
    private AudioSource son;
    private ParticleSystem particuleEffect;

    private void Awake()
    {
        son = this.GetComponentInParent<AudioSource>();
        particuleEffect = GetComponent<ParticleSystem>();
    }

    void Start()
    {
        judy = GameObject.Find("Player");
    }

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
        if (other.gameObject.tag.Equals("Player") && !InventoryManager.Instance.bag_open)
        {
            o_isTotem = true;
            buttonInteragir.SetActive(true);
        }
    }

    private void GetInputs()
    {
        if (Input.GetKeyDown(KeyCode.E) && o_isTotem)
        {
            if (judy.GetComponent<FormsController>().IsBearUnlocked() == 0) //Si il y a encore le totem ours
            {
                //Range arme avant d'ouvrir le menu de sauvegarde
                InventoryManager.Instance.GetComponent<InventoryManager>().PutWeaponInBag();
                GameObject playerRoot = GameObject.Find("Player");
                playerRoot.GetComponent<FormsController>().Transformation(0);
                son.Play();
                particuleEffect.Stop();
                judy.GetComponent<FormsController>().SetBearUnlocked(true);
                // TODO DialogueTrigger.TriggerDialogueTotemOurs(null);
            }
            else //le totem ours a deja ete recupere donc il ne reste que celui du puma
            {
                //Range arme avant d'ouvrir le menu de sauvegarde
                InventoryManager.Instance.GetComponent<InventoryManager>().PutWeaponInBag();
                GameObject playerRoot = GameObject.Find("Player");
                playerRoot.GetComponent<FormsController>().Transformation(0);
                son.Play();
                particuleEffect.Stop();
                judy.GetComponent<FormsController>().SetPumaUnlocked(true);
                // TODO DialogueTrigger.TriggerDialogueTotemPuma(null);
            }
            // Disable the totem
            gameObject.SetActive(false);
            o_isTotem = false;
            buttonInteragir.SetActive(false);
            judy.GetComponentInChildren<ActionsNew>().Stay(100f);
        }
    }
}
