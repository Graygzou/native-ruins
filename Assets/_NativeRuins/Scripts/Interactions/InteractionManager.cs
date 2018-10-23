using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour, IManager
{
    [System.Serializable]
    public struct CutsceneInfos
    {
        [SerializeField]
        public CutScene.CutsceneName name;

        [SerializeField]
        public CutScene cutscene;
    }

    private DialogueManager dialogue;

    [SerializeField]
    private List<CutsceneInfos> cutscenes;

    public delegate void CutsceneHasEnded();
    public static event CutsceneHasEnded OnIntroCutsceneHasEnded;
    public static event CutsceneHasEnded OnBearTotemCutsceneHasEnded;
    // ...

    public void Init()
    {
        // Nothing yet ?
    }

    public void InitMainScene()
    {
        // Nothing yet ?
    }

    /**
     * Start the first cutscene where Judy arrived on the beach.
     */
    /*
   public void LaunchInitialCutscene()
   {
       // Setting up the scene
       cutsceneCamera.enabled = true;
       Camera.main.enabled = false;

       //GameObject.FindWithTag("Player").GetComponent<MovementControllerHuman>().enabled = false;
       //GameObject.Find("Player").GetComponent<FormsController>().enabled = false;

       // Execute the sleeping action
       GameObject.FindWithTag("Player").GetComponent<PlayerProperties>().Sleep();

       // Call dialogues
       /*
       TriggerDialogueDebut(GameObject.Find("PlaneFade").GetComponent<FadeCutScene>());
       TriggerDialogueDebut2(GameObject.Find("SecondCutSceneCamera").GetComponent<StandUpCutScene>());
       TriggerDialogueDebut3(GameObject.Find("SecondCutSceneCamera").GetComponent<LookAroundCutScene>());
       TriggerDialogueDebut4(GameObject.Find("ThirdCutSceneCamera").GetComponent<LostCutScene>());
       TriggerDialogueDebut5(GameObject.Find("ThirdCutSceneCamera").GetComponent<FocusCutScene>());
       TriggerDialogueDebut6(GameObject.Find("ForthCutSceneCamera").GetComponent<TitleGameCutScene>());*/

    // Fire the event to init the other managers.
    //CutsceneHasEnded();
    //}*/

    public void StartCutscene(CutScene.CutsceneName name)
    {
        Debug.Log("Info: starting " + name + " cutscene.");

        // Get the cutscene to play
        CutScene currentCutscene = FindCutscene(name);
        if(currentCutscene != null)
        {
            // Subscribe to the end of the cutscene
            CutScene.OnCutsceneEnd += WhenCutsceneEnds;

            Debug.Log("Info: activate " + name + " cutscene.");
            // Activate it
            currentCutscene.Activate();
        }
    }

    public void WhenCutsceneEnds(CutScene.CutsceneName name)
    {
        // Fire the right trigger depending on the given name
        switch (name)
        {
            case CutScene.CutsceneName.IntroductionCutscene:
                OnIntroCutsceneHasEnded();
                break;
            case CutScene.CutsceneName.BearTotemCutscene:
                OnBearTotemCutsceneHasEnded();
                break;
            default:
                break;
        }
    }

    public void SkipCutscene(CutScene.CutsceneName name)
    {
        // Fade the cutscene : THIS NEED TO BE DONE IN THE CUTSCENE ITSELF.
        FindCutscene(name).Interrupt();
    }

    private CutScene FindCutscene(CutScene.CutsceneName name)
    {
        CutScene result = null;
        bool found = false;
        int i = 0;
        while(!found && i < cutscenes.Count)
        {
            found = (cutscenes[i].name.Equals(name));
            result = found ? cutscenes[i].cutscene : result;
            i++;
        }
        return result;
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
