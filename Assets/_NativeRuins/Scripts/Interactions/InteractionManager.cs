using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that hold all the in-game cutscenes. Also in-charge to setup, launch, interrupt the right cutscnes.
/// </summary>
public class InteractionManager : MonoBehaviour, IManager
{
    [SerializeField]
    private IDictionary<CutScene.InGameCutsceneName, CutScene> cutscenes;

    private DialogueManager dialogue;
    private CutScene.InGameCutsceneName activeCutsceneName;

    public delegate void CutsceneHasEnded();
    public static event CutsceneHasEnded OnIntroCutsceneHasEnded;
    public static event CutsceneHasEnded OnBearTotemCutsceneHasEnded;
    // ...

    public void Init()
    {
        cutscenes = new Dictionary<CutScene.InGameCutsceneName, CutScene>();
        // Find all the cutscenes
        foreach (CutScene cutscene in FindObjectsOfType<CutScene>())
        {
            if(!cutscenes.ContainsKey(cutscene.CutsceneName))
            {
                cutscenes.Add(cutscene.CutsceneName, cutscene);
            }
            //Debug.Log("obj:" + cutscene.gameObject + "name: " + cutscene.CutsceneName);
        }
    }

    public void InitMainMenuScene()
    {
        // Nothing yet ??
    }

    public void InitMainScene()
    {
        // Nothing yet ??
    }

    public void StartCutscene(CutScene.InGameCutsceneName name)
    {
        Debug.Log("Info: starting " + name + " cutscene.");

        // Subscribe the escape key so the player can escape the cutscene.
        InputManager.SubscribeButtonEvent(InputManager.ActionsLabels.Cancel, "Cancel", InputManager.EventTypeButton.Down, SkipCutscene);

        // Get the cutscene to play
        CutScene currentCutscene = cutscenes[name];
        if (currentCutscene != null)
        {
            // Subscribe to the end of the cutscene
            CutScene.OnCutsceneEnd += WhenCutsceneEnds;

            // Init it
            currentCutscene.Init();

            // Activate it
            Debug.Log("Info: activate " + name + " cutscene.");
            activeCutsceneName = name;
            currentCutscene.Activate();
        }
    }

    public void WhenCutsceneEnds(CutScene.InGameCutsceneName name)
    {
        // Unsubscribe the escape key so the player can escape the cutscene.
        InputManager.UnsubscribeButtonEvent(InputManager.ActionsLabels.Cancel);

        // Fire the right trigger depending on the given name
        switch (name)
        {
            case CutScene.InGameCutsceneName.IntroductionCutscene:
                OnIntroCutsceneHasEnded();
                break;
            case CutScene.InGameCutsceneName.BearTotemCutscene:
                OnBearTotemCutsceneHasEnded();
                break;
            default:
                break;
        }
    }

    public void SkipCutscene()
    {
        StartCoroutine(cutscenes[activeCutsceneName].Interrupt());
    }

    /*
    private GameObject FindCutscene(CutScene.InGameCutsceneName name)
    {
        GameObject result = null;
        bool found = false;
        int i = 0;
        while(!found && i < cutscenes.Count)
        {
            found = (cutscenes[i].cutscenePrefabs.GetComponent<CutScene>().CutsceneName.Equals(name));
            result = found ? cutscenes[i].cutscenePrefabs : result;
            i++;
        }
        return result;
    }*/

    public void DisableCutscene()
    {
        cutscenes[activeCutsceneName].Disable();
    }

    /*
    public static void TriggerSauvegarde(CutScene action)
    {
        dialogue = new Dialogue();
        dialogue.name = "Menu";
        dialogue.sentences = new string[1];
        dialogue.sentences[0] = "Votre partie a bien été sauvegardée.";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, action);
    }

    public static void TriggerDialogueInstructions(CutScene action)
    {
        dialogue = new Dialogue();
        dialogue.name = "Aide";
        dialogue.sentences = new string[6];
        dialogue.sentences[0] = "Judy peut se déplacer à l'aide des touches ZQSD du clavier.";
        dialogue.sentences[1] = "Elle peut déplacer la caméra à l'aide de la SOURIS.";
        dialogue.sentences[2] = "La barre d'ESPACE permet de la faire sauter. Pour la faire courir, maintenir la touche SHIFT. Elle peut également s'accroupir à l'aide de CTRL";
        dialogue.sentences[3] = "Pour la faire courir, maintenir la touche SHIFT.";
        dialogue.sentences[4] = "Elle peut également s'accroupir à l'aide de CTRL.";
        dialogue.sentences[5] = "Un menu d'aide est à votre disposition en appuyant sur ECHAP pour vous rappelez les interactions principales.";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, action);
    }


    public static void TriggerDialogueFin(CutScene action)
    {
        dialogue = new Dialogue();
        dialogue.name = "Judy";
        dialogue.sentences = new string[1];
        dialogue.sentences[0] = "Ca y est !!!! Je peux enfin quitter cette île !!!!!!!!! Il était temps ...";
 
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, action);
    }

    public static void TriggerDialogueTotemOurs(CutScene action)
    {
        dialogue = new Dialogue();
        dialogue.name = "Aide";
        dialogue.sentences = new string[3];
        dialogue.sentences[0] = "Félicitations ! Vous venez de trouver le totem Ours. Cet item vous permets de vous transformer en ours.";
        dialogue.sentences[1] = "Dans cette forme, vous êtes plus résistante et vous pourrez accéder à de nouvelles énigmes.";
        dialogue.sentences[2] = "Une roue de transformation est maintenant accéssible si vous maintenez A. Vous pouvez choisir votre forme en passant la souris sur celle désirée.";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, action);
    }

    public static void TriggerDialogueTotemPuma(CutScene action)
    {
        dialogue = new Dialogue();
        dialogue.name = "Aide";
        dialogue.sentences = new string[2];
        dialogue.sentences[0] = "Félicitations ! Vous venez de trouver le totem Puma. Cet item vous permet de vous transformer en puma.";
        dialogue.sentences[1] = "Dans cette forme, vous êtes moins résistante mais vous pourrez obtenir de meilleurs mouvements et ainsi accéder à de nouvelles énigmes.";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, action);
    }

    public static void TriggerDialogueVoile(CutScene action)
    {
        dialogue = new Dialogue();
        dialogue.name = "Aide";
        dialogue.sentences = new string[1];
        dialogue.sentences[0] = "Félicitations ! Vous venez de trouver une voile. Cet item vous permettra de construire un radeau.";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, action);
    }

    public static void TriggerDialogueCorde(CutScene action)
    {
        dialogue = new Dialogue();
        dialogue.name = "Aide";
        dialogue.sentences = new string[1];
        dialogue.sentences[0] = "Félicitations ! Vous venez de trouver une corde. Cet item vous permettra de construire un radeau.";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, action);
    }

    public static void TriggerDialogueArc(CutScene action)
    {
        dialogue = new Dialogue();
        dialogue.name = "Aide";
        dialogue.sentences = new string[3];
        dialogue.sentences[0] = "Félicitations ! Vous venez de trouver un arc. Cet item vous permettra de chasser et de vous défendre.";
        dialogue.sentences[1] = "Des éléments sur l'île vous permettront de créer des flèches.";
        dialogue.sentences[2] = "Pour l'utiliser : Le CLIC DROIT de la souris vous permet de viser et le CLIC GAUCHE vous permet de tirer une flèche.";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, action);
    }*/
}
