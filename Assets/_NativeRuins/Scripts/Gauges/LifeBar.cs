using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This class acts just like a container for the lifeBar HUD. 
 * The player will access those UI thanks to that class/container.
 */
public class LifeBar : MonoBehaviour {

    #region Components setting
    [SerializeField] private RectTransform lifeSprite;
    [SerializeField] private HungerBar hungerBar;
    #endregion

    #region FX audioClips
    [Header("Component settings")]
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip sonCri;
    [SerializeField] private AudioClip sonManger;
    [SerializeField] private AudioClip sonVieBasse;
    #endregion

    public Animator animator;

    private GameObject playerRoot;
    private ActionsNew actions;
    private bool isWeak = false;

    public float GetCurrentSizeLifeBar()
    {
        return lifeSprite.sizeDelta.x;
    }

    private void SetSizeLifeBar(float size)
    {
        float newLifeValue = Mathf.Clamp(size, 0f, PlayerProperties.MAX_LIFE_PLAYER);
        lifeSprite.sizeDelta = new Vector2(newLifeValue, lifeSprite.sizeDelta.y);
    }

    public void ChangeLifeBar(float amount)
    {
        SetSizeLifeBar(lifeSprite.sizeDelta.x + amount);
    }

    public void RestoreLifeFromData(float amount)
    {
        SetSizeLifeBar(amount * PlayerProperties.MAX_LIFE_PLAYER);
    }

    public bool IsFull()
    {
        return GetCurrentSizeLifeBar() >= PlayerProperties.MAX_LIFE_PLAYER;
    }
}
