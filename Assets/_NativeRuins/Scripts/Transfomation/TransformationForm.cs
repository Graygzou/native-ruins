using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TransformationForm {

    public TransformationType type;

    public GameObject form;

    public PlayerProperties stats;

    public Sprite icon;

    public Color color;

    public Sprite background;

    public bool isUnlocked;

};
