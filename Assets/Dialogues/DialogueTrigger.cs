using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public Dialogue dialogueDebut;
    public Dialogue dialogueFin;
    public Dialogue dialogueTotemOurs;
    public Dialogue dialogueTotemPuma;
    public Dialogue dialogueVoile;
    public Dialogue dialogueCorde;

    public void TriggerDialogueDebut()
    {
        dialogueDebut.name = "Judy";
        dialogueDebut.sentences = new string[4];
        dialogueDebut.sentences[0] = "Ouuuuhhhhh ......... J'ai mal à la tête ..... Qu'est ce qu'il m'est arrivé ? Ou est ce que je suis ?";
        dialogueDebut.sentences[1] = "Je suis sur une île ?! Mais comment c'est possible ? Je ne me souviens de rien.. Ca ne m'a pas l'air très habité..";
        dialogueDebut.sentences[2] = "Comment je vais partir d'ici ? ... Mmmmmhhhh ... Et si je construisais un radeau !";
        dialogueDebut.sentences[3] = "Il doit bien y avoir de quoi construire un radeau ici! Commençons par explorer cette plage !";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogueDebut);
    }

    public void TriggerDialogueFin()
    {
        dialogueDebut.name = "Judy";
        dialogueDebut.sentences = new string[1];
 
        FindObjectOfType<DialogueManager>().StartDialogue(dialogueFin);
    }

    public void TriggerDialogueTotemOurs()
    {
        dialogueDebut.name = "Judy";
        dialogueDebut.sentences = new string[1];

        FindObjectOfType<DialogueManager>().StartDialogue(dialogueTotemOurs);
    }

    public void TriggerDialogueTotemPuma()
    {
        dialogueDebut.name = "Judy";
        dialogueDebut.sentences = new string[1];

        FindObjectOfType<DialogueManager>().StartDialogue(dialogueTotemPuma);
    }

    public void TriggerDialogueVoile()
    {
        dialogueDebut.name = "Judy";
        dialogueDebut.sentences = new string[6];


        FindObjectOfType<DialogueManager>().StartDialogue(dialogueVoile);
    }

    public void TriggerDialogueCorde()
    {
        dialogueDebut.name = "Judy";
        dialogueDebut.sentences = new string[6];

        FindObjectOfType<DialogueManager>().StartDialogue(dialogueCorde);
    }

}
