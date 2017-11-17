using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {
    public string name; //nom de la personne qui parle
    [TextArea(3, 10)] //pour bloquer la taille de la ou on ecrit les dialogues dans le manager
    public string[] sentences; //phrases qu'elle dit
}
