using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBonFire : MonoBehaviour {


	private bool o_isBonFire = false;
    private AudioSource sonFeu;
    private bool sitted;

    // Use this for initialization
    void Start () {
        sonFeu = this.GetComponent<AudioSource>();
        sitted = false;
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
		if (other.gameObject.tag.Equals ("Player") && !InventoryManager.instance.bag_open) {
			o_isBonFire = true;
			GameObject.Find ("Affichages/Interaction/ButtonInteragir").SetActive(true);
		}
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player")) sonFeu.Play(); 
    }

    private void GetInputs(){
		if (Input.GetKeyDown (KeyCode.E) && o_isBonFire && !sitted) {
            sitted = true;
            //Range arme avant d'ouvrir le menu de sauvegarde
            GameObject.Find("InventoryManager").GetComponent<InventoryManager>().PutWeaponInBag();
            GameObject playerRoot = GameObject.Find("Player");
            playerRoot.GetComponent<FormsController>().Transformation(0);

            // Play the sitting animation
            try
            {
                // If the bonfire is a real one (not craft)
                GetComponent<Switch>().Activate();
                StartCoroutine("SitDownNearFire");
            }
            catch(Exception e) {}
        } else if(Input.GetKeyDown(KeyCode.E) && o_isBonFire && sitted) {
            sitted = false;
            StartCoroutine("StandUpNearFire");
        }
	}

    IEnumerator SitDownNearFire() {
        GameObject judy = GameObject.FindWithTag("Player");
        judy.GetComponent<ActionsNew>().SitDown();
        // Remove control of judy
        judy.GetComponent<MovementController>().setIsSaving(true);
        yield return new WaitForSeconds(3.6f);
        // Enable the save menu
        GameObject.Find("Affichages/Menus/Menu_sauvegarder").SetActive(!GameObject.Find("Affichages/Menus/Menu_sauvegarder").activeSelf);
    }

    IEnumerator StandUpNearFire() {
        GameObject judy = GameObject.FindWithTag("Player");
        judy.GetComponent<ActionsNew>().StandUp();
        // Disable the save menu
        GameObject.Find("Affichages/Menus/Menu_sauvegarder").SetActive(!GameObject.Find("Affichages/Menus/Menu_sauvegarder").activeSelf);
        yield return new WaitForSeconds(4.4f);
        // Can control judy
        judy.GetComponent<MovementController>().setIsSaving(false);
        try
        {
            // If the bonfire is a real one (not craft)
            GetComponent<Switch>().StopCutScene();
        }
        catch (Exception e) { }
    }
}
