using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBonFire : MonoBehaviour {


	private bool o_isBonFire = false;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		GetInputs ();
    }

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag.Equals ("Player") && o_isBonFire) {
			o_isBonFire = false;
			GameObject.Find ("Affichages/Interaction/ButtonInteragir").SetActive(false);
            Debug.Log("Sort");
        }
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.tag.Equals ("Player") && !InventoryManager.bag_open) {
			o_isBonFire = true;
			GameObject.Find ("Affichages/Interaction/ButtonInteragir").SetActive(true);
		}
	}

	private void GetInputs(){
		if (Input.GetKeyDown (KeyCode.E) && o_isBonFire) {
            //Range arme avant d'ouvrir le menu de sauvegarde
            GameObject.Find("InventoryManager").GetComponent<InventoryManager>().PutWeaponInBag();
            //Apparition du menu de 
            GameObject.Find("Affichages/Menus/Menu_sauvegarder").SetActive(!GameObject.Find("Affichages/Menus/Menu_sauvegarder").activeSelf);
            Debug.Log("E appuyé");
        }
	}
}
