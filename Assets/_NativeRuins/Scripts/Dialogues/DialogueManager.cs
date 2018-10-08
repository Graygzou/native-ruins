using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    #region SerializeField
    [SerializeField]
    public Text nameText;
    [SerializeField]
    public Text dialogueText;
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private AudioClip sonDialog;
    [SerializeField]
    private AudioClip sonLettre;
    #endregion

    private Queue<Dialogue> dialoguesQueue;
    private Queue<Switch> actionsQueue;
    private Queue<string> sentences;

    private AudioSource audioSource;
    private GameObject judy;

    private bool isTyping;
    private bool isProcessing;
    private string currentSentence;

    // Use this for initialization
    void Awake () {
        dialoguesQueue = new Queue<Dialogue>();
        actionsQueue = new Queue<Switch>();
        sentences = new Queue<string>();

        isTyping = false;
        isProcessing = false;
        judy = GameObject.FindWithTag("Player");
        audioSource = GetComponent<AudioSource>();
    }
	
    //Lancer le dialogue
	public void StartDialogue (Dialogue dialogue, Switch action) {
        // Put the dialogue in the queue ans the switch
        this.dialoguesQueue.Enqueue(dialogue);
        this.actionsQueue.Enqueue(action);

        // if the dialogueManager is empty at the moment
        if (this.dialoguesQueue.Count == 1 && !isProcessing) {
            // Process the current dialogue
            judy.GetComponent<MovementController>().setDialogue(true);
            audioSource.clip = sonDialog;
            audioSource.Play();
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
            audioSource.Stop();
        }
    }

    //Afficher lettre par lettre les phrases dans le dialogue
    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            audioSource.clip = sonLettre;
            audioSource.Play();
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
            judy.GetComponent<MovementController>().setDialogue(false);
            StopAllCoroutines();
            audioSource.Stop();
            audioSource.clip = sonDialog;
            audioSource.Play();
            animator.SetBool("isOpen", false);
        }
    }
}
