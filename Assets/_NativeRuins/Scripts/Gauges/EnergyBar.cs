using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    #region Components settings
    [SerializeField] private RectTransform energySprite;
    #endregion

    public float GetCurrentEnergy()
    {
        return energySprite.sizeDelta.x;
    }

    public void SetSizeEnergyBar(float size)
    {
        float newHungerValue = Mathf.Clamp(size, 0, PlayerProperties.MAX_ENERGY_PLAYER);
        energySprite.sizeDelta = new Vector2(newHungerValue, energySprite.sizeDelta.y);
    }

    private void ChangeEnergyBar(float amount)
    {
        SetSizeEnergyBar(energySprite.sizeDelta.x + amount);
    }
}