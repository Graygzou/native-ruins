﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    #region Consts
    private const float MAX_HUNGER_PLAYER = 300f;
    #endregion

    #region Components
    [SerializeField]
    private RectTransform hungerSprite;
    #endregion

    #region Hunger settings
    [SerializeField]
    private int timeMaxBeforeHungerDecrease = 5;

    [SerializeField]
    private float hungerDecreasingFactor = 0.04f;

    [SerializeField]
    private float hungerDecreasingFactorPuma = 0.08f;
    #endregion

    private float currentTimeFaim = 0.0f;
    
	private GameObject forms;

    // Update is called once per frame
    void Update () {
        currentTimeFaim += Time.deltaTime;
        if (currentTimeFaim >= timeMaxBeforeHungerDecrease)
        {
            GameObject playerRoot = GameObject.Find("Player");
            if (FormsController.Instance.GetCurrentForm() == (int)Forms.id_puma)
            {
                ChangeHungerBar(-hungerDecreasingFactorPuma);
            }
            else
            {
                ChangeHungerBar(-hungerDecreasingFactor);
            }
            currentTimeFaim = 0.0f;
        }
    }

    public float GetSizeHungerBar()
    {
        return hungerSprite.sizeDelta.x;
    }

    private void SetSizeHungerBar(float size)
    {
        float newHungerValue = Mathf.Clamp(size, 0, MAX_HUNGER_PLAYER);
        hungerSprite.sizeDelta = new Vector2(newHungerValue, hungerSprite.sizeDelta.y);
    }

    public void ChangeHungerBar(float amount)
    {
        SetSizeHungerBar(hungerSprite.sizeDelta.x + amount);
    }

    public void RestoreHungerFromData(float amount)
    {
        SetSizeHungerBar(amount * MAX_HUNGER_PLAYER);
    }

    public bool IsFull()
    {
        return hungerSprite.sizeDelta.x == MAX_HUNGER_PLAYER;
    }
}
