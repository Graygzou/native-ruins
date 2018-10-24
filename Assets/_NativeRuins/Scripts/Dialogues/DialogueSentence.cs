using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This is not a ScriptableObject because their is no point of manipulating a single Dialogue Sentence alone.
 */
[System.Serializable]
public class DialogueSentence {
    // The name of the locutor
    public string name = "?????"; 

    // The face of the locutor
    public Texture2D locutorImage = null;

    // The sound make when triggering 
    public AudioClip soundExpression = null;

    // the sentence said by the locutor
    [TextArea(3, 10)] //pour bloquer la taille de la ou on ecrit les dialogues dans le manager
    public string sentence;

    public bool playSong = false;
}
