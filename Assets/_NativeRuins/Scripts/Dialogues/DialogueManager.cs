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
    public Button dialogueContinueButton;
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

    public delegate void EventNextSentence();
    public static event EventNextSentence OnClicked;

    public delegate void SentenceFinished();
    public static event SentenceFinished OnSentenceFinish;

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

        // Put the dialogue in the queue ans the switch
        dialoguesQueue.Enqueue(dialogue);
        actionsQueue.Enqueue(action);

        // if the dialogueManager is empty at the moment
        if (dialoguesQueue.Count == 1 && !isProcessing) {
            // Process the current dialogue
            InitDialogueUI();
        }
    }

    public void InitDialogueUI()
    {
        judy.GetComponent<PlayerProperties>().LaunchDialogue();
        audioSource.clip = sonDialog;
        audioSource.Play();
        animator.SetTrigger("Open");

        //DisplayDialogueCanvas();
    }

    public void InterruptDialogue()
    {
        sentences.Clear();
        dialoguesQueue.Clear();
        actionsQueue.Clear();

        animator.SetTrigger("Close");
        //HideDialogueCanvas();
    }

    private void FillSentences(Dialogue dialogue) {
        isProcessing = true;
        /*
        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }*/
        DisplayNextSentence();
    }

    public void DisplayNextSentence2(DialogueSentence sentence)
    {
        currentSentence = sentence.sentence;
        StartCoroutine(TypeSentence(sentence.sentence));
    }

    //Afficher les phrases suivantes du dialogue
    public void DisplayNextSentence()
    {
        // if the sentence isn't finished, fast print it.
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentSentence;
            isTyping = false;
        }
        else
        {
            OnClicked();
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

        //OnSentenceFinish();
    }

    //Fin du dialogue
    public void EndDialogue() {
        // Play the corresponding switch
        /*
        Trigger action = actionsQueue.Dequeue();
        if(action != null) {
            SwitchManager.StartAction(action);
        }

        // Check if a dialogue need to be played.
        if (dialoguesQueue.Count >= 1) {
            FillSentences(dialoguesQueue.Dequeue());
        } else {*/
        // Stop the dialogue
        isProcessing = false;
        animator.SetTrigger("Close");

        dialogueContinueButton.interactable = false;

        StopAllCoroutines();
        
        audioSource.Stop();
        audioSource.clip = sonDialog;
        audioSource.Play();
        

        //HideDialogueCanvas();

        judy.GetComponent<PlayerProperties>().CloseDialogue();
        //}
    }
    /*
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
    }*/
}
