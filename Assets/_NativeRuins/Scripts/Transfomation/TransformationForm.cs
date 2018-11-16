using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TransformationForm {

    public TransformationType type;

    public GameObject form;

    public PlayerProperties stats;

    public Sprite icon;

    public bool useBackground;

    public UICircle background;

    public Color highlightColor;

    public bool expanded;

    public bool isUnlocked;

};
