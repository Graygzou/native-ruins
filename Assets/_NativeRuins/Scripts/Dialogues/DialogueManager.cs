using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    public Text nameText;
    public Text dialogueText;

    public Animator animator;

    private Queue<Dialogue> dialoguesQueue;
    private Queue<Switch> actionsQueue;
    private Queue<string> sentences;

    private AudioSource sonDialog;
    private AudioSource sonLettre;
    private GameObject Judy;

    private bool isTyping;
    private bool isProcessing;
    private string currentSentence;

    // Use this for initialization
    void Awake () {
        this.dialoguesQueue = new Queue<Dialogue>();
        this.actionsQueue = new Queue<Switch>();
        this.sentences = new Queue<string>();

        isTyping = false;
        isProcessing = false;
        Judy = GameObject.FindWithTag("Player");
        AudioSource[] son = this.GetComponents<AudioSource>();
        sonDialog = son[0];
        sonLettre = son[1];
    }
	
    //Lancer le dialogue
	public void StartDialogue (Dialogue dialogue, Switch action) {
        // Put the dialogue in the queue ans the switch
        this.dialoguesQueue.Enqueue(dialogue);
        this.actionsQueue.Enqueue(action);

        // if the dialogueManager is empty at the moment
        if (this.dialoguesQueue.Count == 1 && !isProcessing) {
            // Process the current dialogue
            Judy.GetComponent<MovementController>().setDialogue(true);
            sonDialog.Play();
            animator.SetBool("isOpen", true);
            FillSentences(this.dialoguesQueue.Dequeue());
        }
    }

    private void FillSentences(Dialogue dialogue) {
        isProcessing = true;
        foreach (string sentence in dialogue.sentences) {
            this.sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    //Afficher les phrases suivantes du dialogue
    public void DisplayNextSentence()
    {
        // if the sentence isn't finished, fast print it.
        if (isTyping) {
            StopAllCoroutines();
            dialogueText.text = currentSentence;
            isTyping = false;
        } else {
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }
            this.currentSentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(this.currentSentence));
            sonLettre.Stop();
        }
    }

    //Afficher lettre par lettre les phrases dans le dialogue
    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            sonLettre.Play();
            dialogueText.text += letter;
            yield return null;
        }
        isTyping = false;
    }

    //Fin du dialogue
    public void EndDialogue() {
        // Play the corresponding switch
        Switch action = actionsQueue.Dequeue();
        if(action != null) {
            SwitchManager.StartAction(action);
        }

        // Check if a dialogue need to be played.
        if (this.dialoguesQueue.Count >= 1) {
            this.FillSentences(this.dialoguesQueue.Dequeue());
        } else {
            // Stop the dialogue
            isProcessing = false;
            Judy.GetComponent<MovementController>().setDialogue(false);
            StopAllCoroutines();
            sonLettre.Stop();
            sonDialog.Play();
            animator.SetBool("isOpen", false);
        }
    }
}
