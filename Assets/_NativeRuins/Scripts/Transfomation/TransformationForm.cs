using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is made to be printed in the transformation wheel as item.
/// </summary>
[System.Serializable]
public class TransformationForm : WheelItem {

    [Header("Transformation fields")]
    public TransformationType type;

    public Animator animator;

    public SkinnedMeshRenderer mesh;

    public PlayerProperties stats;
};
