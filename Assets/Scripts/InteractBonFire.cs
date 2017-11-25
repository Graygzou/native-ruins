using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBonFire : MonoBehaviour {


	private bool o_isBonFire = false;
    private AudioSource sonFeu;

	// Use this for initialization
	void Start () {
        sonFeu = this.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		GetInputs ();
    }

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag.Equals ("Player") && o_isBonFire) {
            sonFeu.Stop();
			o_isBonFire = false;
			GameObject.Find ("Affichages/Interaction/ButtonInteragir").SetActive(false);
            GameObject.Find("Affichages/Menus/Menu_sauvegarder").SetActive(false);
        }
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.tag.Equals ("Player") && !InventoryManager.bag_open) {
			o_isBonFire = true;
			GameObject.Find ("Affichages/Interaction/ButtonInteragir").SetActive(true);
		}
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player")) sonFeu.Play(); 
    }

        private void GetInputs(){
		if (Input.GetKeyDown (KeyCode.E) && o_isBonFire) {
            //Range arme avant d'ouvrir le menu de sauvegarde
            GameObject.Find("InventoryManager").GetComponent<InventoryManager>().PutWeaponInBag();
            GameObject playerRoot = GameObject.Find("Player");
            playerRoot.GetComponent<FormsController>().Transformation(0);
            GameObject.Find("Affichages/Menus/Menu_sauvegarder").SetActive(!GameObject.Find("Affichages/Menus/Menu_sauvegarder").activeSelf);
        }
	}
}
