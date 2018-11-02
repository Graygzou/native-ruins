using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TransformationForm {

    public GameObject form;

    public PlayerProperties stats;

    public Sprite icon;

    public Sprite background;

    public Color color;

    [SerializeField]
    public bool isUnlocked;

};
