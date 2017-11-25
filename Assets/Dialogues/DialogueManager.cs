using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    public Text nameText;
    public Text dialogueText;

    public Animator animator;

    private Queue<string> sentences;
    private AudioSource sonDialog;
    private AudioSource sonLettre;
    private GameObject Judy;

	// Use this for initialization
	void Start () {
        Judy = GameObject.FindWithTag("Player");
        sentences = new Queue<string>();
        AudioSource[] son = this.GetComponents<AudioSource>();
        sonDialog = son[0];
        sonLettre = son[1];
    }
	
    //Lancer le dialogue
	public void StartDialogue (Dialogue dialogue)
    {
        Judy.GetComponent<MovementController>().setDialogue(true);
        sonDialog.Play();
        animator.SetBool("isOpen",true);
        nameText.text = dialogue.name;
        sentences.Clear();
        foreach(string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    //Afficher les phrases suivantes du dialogue
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        sonLettre.Stop();
    }

    //Afficher lettre par lettre les phrases dans le dialogue
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            sonLettre.Play();
            dialogueText.text += letter;
            yield return null;
        }
    }

    //Fin du dialogue
    public void EndDialogue()
    {
        Judy.GetComponent<MovementController>().setDialogue(false);
        sonLettre.Stop();
        sonDialog.Play();
        animator.SetBool("isOpen", false);
    }
}
