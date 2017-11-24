using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public Dialogue dialogue;

    public void TriggerSauvegarde()
    {
        dialogue.name = "Menu";
        dialogue.sentences = new string[1];
        dialogue.sentences[0] = "Votre partie a bien été sauvegardée.";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public void TriggerDialogueDebut()
    {
        dialogue.name = "Judy";
        dialogue.sentences = new string[7];
        dialogue.sentences[0] = "Ouuuuhhhhh ......... J'ai mal à la tête ..... Qu'est ce qu'il m'est arrivé ? Ou est ce que je suis ?";
        dialogue.sentences[1] = "Je suis sur une île ?! Mais comment c'est possible ? Je ne me souviens de rien.. Ca ne m'a pas l'air très habité..";
        dialogue.sentences[2] = "Comment je vais partir d'ici ? ... Mmmmmhhhh ... Et si je construisais un radeau !";
        dialogue.sentences[3] = "Bon ne nous emballons pas trop...  Commençons par explorer cette plage !";
        dialogue.name = "Aide";
        dialogue.sentences[4] = "Judy peut se déplacer à l'aide des touches ZQSD du clavier. Elle peut déplacer la caméra à l'aide de la SOURIS.";
        dialogue.sentences[5] = "La barre d'ESPACE permet de la faire sauter. Pour la faire courir, maintenir la touche SHIFT. Elle peut également s'accroupir à l'aide de CTRL";
        dialogue.sentences[6] = "Un menu d'aide est à votre disposition en appuyant sur ECHAP pour vous rappelez les interactions principales.";
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
        dialogue.sentences[2] = "Une roue de transformation est maintenant accéssible si vous maintenez A. Vous pouvez choisir votre forme en passant la souris sur celle désirée.";
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
        dialogue.sentences = new string[3];
        dialogue.sentences[0] = "Félicitation! Vous venez de trouver un arc. Cet item vous permettra de chasser et de vous défendre.";
        dialogue.sentences[1] = "Des éléments sur l'île vous permettront de créer des flèches.";
        dialogue.sentences[2] = "Pour l'utiliser: Le CLIC DROIT de la souris vous permet de viser et le CLIC GAUCHE vous permet de tirer une flèche.";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

}
