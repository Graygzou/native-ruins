using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public Dialogue dialogue;

    public void TriggerDialogueDebut()
    {
        dialogue.name = "Judy";
        dialogue.sentences = new string[4];
        dialogue.sentences[0] = "Ouuuuhhhhh ......... J'ai mal à la tête ..... Qu'est ce qu'il m'est arrivé ? Ou est ce que je suis ?";
        dialogue.sentences[1] = "Je suis sur une île ?! Mais comment c'est possible ? Je ne me souviens de rien.. Ca ne m'a pas l'air très habité..";
        dialogue.sentences[2] = "Comment je vais partir d'ici ? ... Mmmmmhhhh ... Et si je construisais un radeau !";
        dialogue.sentences[3] = "Il doit bien y avoir de quoi construire un radeau ici! Commençons par explorer cette plage !";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public void TriggerDialogueFin()
    {
        dialogue.name = "Judy";
        dialogue.sentences = new string[1];
 
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public void TriggerDialogueTotemOurs()
    {
        dialogue.name = "Judy";
        dialogue.sentences = new string[1];

        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public void TriggerDialogueTotemPuma()
    {
        dialogue.name = "Judy";
        dialogue.sentences = new string[1];

        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public void TriggerDialogueVoile()
    {
        dialogue.name = "Judy";
        dialogue.sentences = new string[6];


        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public void TriggerDialogueCorde()
    {
        dialogue.name = "Judy";
        dialogue.sentences = new string[6];

        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

}
