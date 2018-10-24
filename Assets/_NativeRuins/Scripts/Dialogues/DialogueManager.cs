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
    private Queue<Trigger> actionsQueue;
    private Queue<string> sentences;

    private AudioSource audioSource;
    private CanvasGroup canvas;
    private GameObject judy;

    private bool isTyping;
    private bool isProcessing;
    private string currentSentence;

    // Use this for initialization
    void Awake () {
        dialoguesQueue = new Queue<Dialogue>();
        actionsQueue = new Queue<Trigger>();
        sentences = new Queue<string>();

        isTyping = false;
        isProcessing = false;
        judy = GameObject.FindWithTag("Player");
        audioSource = GetComponent<AudioSource>();
        canvas = GetComponent<CanvasGroup>();
    }
	
    //Lancer le dialogue
	public void StartDialogue (Dialogue dialogue, Trigger action) {
        DisplayDialogueCanvas();

        // Put the dialogue in the queue ans the switch
        dialoguesQueue.Enqueue(dialogue);
        actionsQueue.Enqueue(action);

        // if the dialogueManager is empty at the moment
        if (dialoguesQueue.Count == 1 && !isProcessing) {
            // Process the current dialogue
            judy.GetComponent<PlayerProperties>().LaunchDialogue();
            audioSource.clip = sonDialog;
            audioSource.Play();
            animator.SetBool("isOpen", true);
            FillSentences(dialoguesQueue.Dequeue());
        }
    }

    public void InterruptDialogue()
    {
        sentences.Clear();
        dialoguesQueue.Clear();
        actionsQueue.Clear();

        HideDialogueCanvas();
    }

    private void FillSentences(Dialogue dialogue) {
        isProcessing = true;
        /*
        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }*/
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
            currentSentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(currentSentence));
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
        Trigger action = actionsQueue.Dequeue();
        if(action != null) {
            SwitchManager.StartAction(action);
        }

        // Check if a dialogue need to be played.
        if (dialoguesQueue.Count >= 1) {
            FillSentences(dialoguesQueue.Dequeue());
        } else {
            // Stop the dialogue
            isProcessing = false;
            judy.GetComponent<PlayerProperties>().CloseDialogue();
            StopAllCoroutines();
            audioSource.Stop();
            audioSource.clip = sonDialog;
            audioSource.Play();
            animator.SetBool("isOpen", false);

            HideDialogueCanvas();
        }
    }

    private void DisplayDialogueCanvas()
    {
        canvas.alpha = 1.0f;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
    }

    private void HideDialogueCanvas()
    {
        canvas.alpha = 0.0f;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
    }
}
