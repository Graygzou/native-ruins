using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IManager
{
    #region Enums
    public enum CutsceneName : int
    {
        introductionCutscene = 0,
    }
    #endregion

    public static Dialogue dialogue;

    private IDictionary<CutsceneName, CutScene> cutscenes;
    //private Camera cutsceneCamera;

    public delegate void CutsceneEnd();
    public static event CutsceneEnd CutsceneHasEnded;

    private void Awake()
    {
        //cutsceneCamera = GameObject.FindWithTag("CutsceneCamera").GetComponent<Camera>();

        // Create all the cutscenes
        cutscenes = new Dictionary<CutsceneName, CutScene>();
        cutscenes.Add(CutsceneName.introductionCutscene, new TitleGameCutScene());
    }

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

    public void StartCutscene(CutsceneName name)
    {
        CutScene currentCutscene = cutscenes[name];

        currentCutscene.Activate();

        CutsceneHasEnded();
    }

    public void SkipCutscene()
    {
        // Fade the cutscene


        // Fire the event to init the other managers.
        CutsceneHasEnded();
    }

    public static void TriggerSauvegarde(CutScene action)
    {
        dialogue = new Dialogue();
        dialogue.name = "Menu";
        dialogue.sentences = new string[1];
        dialogue.sentences[0] = "Votre partie a bien été sauvegardée.";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, action);
    }

    public static void TriggerDialogueDebut(CutScene action) {
        dialogue = new Dialogue();
        dialogue.name = "Judy";
        dialogue.sentences = new string[2];
        dialogue.sentences[0] = ".........           ";
        dialogue.sentences[1] = "  ... aaah..  aaah.... ma tête...";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, action);
    }

    public static void TriggerDialogueDebut2(CutScene action) {
        dialogue = new Dialogue();
        dialogue.name = "Judy";
        dialogue.sentences = new string[3];
        dialogue.sentences[0] = ".........";
        dialogue.sentences[1] = "Ou suis-je... ?";
        dialogue.sentences[2] = "Que m'est-il arrivé... ?";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, action);
    }

    public static void TriggerDialogueDebut3(CutScene action) {
        dialogue = new Dialogue();
        dialogue.name = "Judy";
        dialogue.sentences = new string[2];
        dialogue.sentences[0] = ".........";
        dialogue.sentences[1] = " Aie... J'ai mal partout...";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, action);
    }

    public static void TriggerDialogueDebut4(CutScene action) {
        dialogue = new Dialogue();
        dialogue.name = "Judy";
        dialogue.sentences = new string[2];
        dialogue.sentences[0] = "Je suis sur une île ?! Mais comment c'est possible ? Je ne me souviens de rien..";
        dialogue.sentences[1] = "Ca ne m'a pas l'air très habité..";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, action);
    }

    public static void TriggerDialogueDebut5(CutScene action) {
        dialogue = new Dialogue();
        dialogue.name = "Judy";
        dialogue.sentences = new string[2];
        dialogue.sentences[0] = "Quel-est ce cauchemard !";
        dialogue.sentences[1] = "Comment je vais partir d'ici ? ... Mmmmmhhhh ...";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, action);
    }
    public static void TriggerDialogueDebut6(CutScene action) {
        dialogue = new Dialogue();
        dialogue.name = "Judy";
        dialogue.sentences = new string[2];
        dialogue.sentences[0] = "Je sais ! Et si je construisais un radeau !";
        dialogue.sentences[1] = "Bon ne nous emballons pas trop...  Commençons par explorer cette plage !";
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
    }
}
