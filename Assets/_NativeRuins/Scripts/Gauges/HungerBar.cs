using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This class acts just like a container for the hungerBar HUD. 
 * The player will access those UI thanks to that class/container.
 */
public class HungerBar : MonoBehaviour
{
    #region Components
    [SerializeField] private RectTransform hungerSprite;
    #endregion

    public float GetSizeHungerBar()
    {
        return hungerSprite.sizeDelta.x;
    }

    private void SetSizeHungerBar(float size)
    {
        float newHungerValue = Mathf.Clamp(size, 0, PlayerProperties.MAX_HUNGER_PLAYER);
        hungerSprite.sizeDelta = new Vector2(newHungerValue, hungerSprite.sizeDelta.y);
    }

    public void ChangeHungerBar(float amount)
    {
        SetSizeHungerBar(hungerSprite.sizeDelta.x + amount);
    }

    public void RestoreHungerFromData(float amount)
    {
        SetSizeHungerBar(amount * PlayerProperties.MAX_HUNGER_PLAYER);
    }

    public bool IsFull()
    {
        return hungerSprite.sizeDelta.x == PlayerProperties.MAX_HUNGER_PLAYER;
    }
}
