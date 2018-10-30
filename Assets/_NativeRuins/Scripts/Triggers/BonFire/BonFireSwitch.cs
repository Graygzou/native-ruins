using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonFireSwitch : Trigger
{
    /*
    override public IEnumerator PlayCutSceneStart() {
        ActivateSwitch();
        yield return new WaitForSeconds(3.6f);
        // Enable the save menu
        GameObject.Find("Affichages/Menus/Menu_sauvegarder").SetActive(!GameObject.Find("Affichages/Menus/Menu_sauvegarder").activeSelf);

        if (gameObject.GetComponent<SwitchObject>() != null) {
            // Activate all his children
            gameObject.GetComponent<SwitchObject>().ActivateChildren();
        }
    }

    override public IEnumerator PlayCutSceneEnd() {
        DiactivateSwitch();
        GameObject judy = GameObject.FindWithTag("Player");
        yield return new WaitForSeconds(4.4f);
        judy.GetComponent<PlayerProperties>().DisableSaving();
        StopCutScene();

        if (gameObject.GetComponent<SwitchObject>() != null) {
            // Activate all his children
            gameObject.GetComponent<SwitchObject>().ActivateChildren();
        }
    }

    // The switch does what he's meant for here.
    override protected void ActivateSwitch() {
        GameObject judy = GameObject.FindWithTag("Player");
        judy.GetComponent<ActionsNew>().SitDown();
        // Remove control of judy
        judy.GetComponent<PlayerProperties>().EnableSaving();
    }

    override protected void DiactivateSwitch() {
        GameObject judy = GameObject.FindWithTag("Player");
        judy.GetComponent<ActionsNew>().StandUp();
        // Disable the save menu
        GameObject.Find("Affichages/Menus/Menu_sauvegarder").SetActive(!GameObject.Find("Affichages/Menus/Menu_sauvegarder").activeSelf);
    }*/
}
