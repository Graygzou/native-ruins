using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchManager {

    private static bool isProcessing = false;
    private static Queue<Trigger> actionsQueue = new Queue<Trigger>();
	
    //Lancer le dialogue
	public static void StartAction (Trigger action) {
        // Put the dialogue in the queue ans the switch
        actionsQueue.Enqueue(action);

        // if the SwitchManager is empty at the moment
        if (actionsQueue.Count == 1 && !isProcessing) {
            ExecuteAction(actionsQueue.Dequeue());
        }
    }

    //Afficher les phrases suivantes du dialogue
    public static void ExecuteAction(Trigger action) {
        isProcessing = true;
        // launch the animation
        action.Fire();
    }

    //Fin du switch
    public static void EndAction() {
        isProcessing = false;

        // Play the corresponding switch
        if( actionsQueue.Count >= 1) {
            ExecuteAction(actionsQueue.Dequeue());
        }
    }
}
