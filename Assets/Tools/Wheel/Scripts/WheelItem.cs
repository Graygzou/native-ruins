using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that need to be added to the wheel have to extends this class.
/// This can be useful for weapons, transformations, inventory items, etc.
/// </summary>
public class WheelItem : ScriptableObject
{
    [Header("Basic UI Elements")]
    public Sprite icon;

    public Color color;

    public Color highlightColor;

    [Header("Others")]
    public bool isUnlocked;

    public Sprite background;
}
