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
        dialogue.sentences[0] = "Ca y est !!!! Je peux enfin quitter cette île !!!!!!!!! Il était temps ...";
 
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public void TriggerDialogueTotemOurs()
    {
        dialogue.name = "Aide";
        dialogue.sentences = new string[3];
        dialogue.sentences[0] = "Félicitation! Vous venez de trouver le totem Ours. Cet item vous permets de vous transformer en ours.";
        dialogue.sentences[1] = "Dans cette forme, vous êtes plus résistante et vous pourrez accéder à de nouvelles énigmes.";
        dialogue.sentences[2] = "Une roue de transformation est maintenant accéssible si vous maintenez A. Vous pouvez choisir votre forme à l'aide du clic gauche de la souris";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public void TriggerDialogueTotemPuma()
    {
        dialogue.name = "Judy";
        dialogue.sentences = new string[2];
        dialogue.sentences[0] = "Félicitation! Vous venez de trouver le totem Puma. Cet item vous permets de vous transformer en puma.";
        dialogue.sentences[1] = "Dans cette forme, vous êtes moins résistante mais vous pourrez obtenir de meilleurs mouvements et ainsi accéder à de nouvelles énigmes.";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public void TriggerDialogueVoile()
    {
        dialogue.name = "Judy";
        dialogue.sentences = new string[1];
        dialogue.sentences[0] = "Félicitation! Vous venez de trouver une voile. Cet item vous permettra de construire un radeau.";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public void TriggerDialogueCorde()
    {
        dialogue.name = "Judy";
        dialogue.sentences = new string[1];
        dialogue.sentences[0] = "Félicitation! Vous venez de trouver une corde. Cet item vous permettra de construire un radeau.";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public void TriggerDialogueArc()
    {
        dialogue.name = "Judy";
        dialogue.sentences = new string[6];
        dialogue.sentences[0] = "Félicitation! Vous venez de trouver un arc. Cet item vous permettra de chasser et de vous défendre.";
        dialogue.sentences[1] = "Des éléments sur l'île vous permettront de créer des flèches.";
        dialogue.sentences[2] = "Pour l'utiliser: ";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

}
