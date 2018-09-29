using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    #region Consts
    private const float MAX_ENERGY_PLAYER = 180f;
    #endregion

    #region Components settings
    [SerializeField]
    private RectTransform energySprite;
    #endregion

    #region Energy settings
    [SerializeField]
    private float energyDecreasingFactor = 1f;
    [SerializeField]
    private float energizingBackFactor = 2f;
    [SerializeField]
    private float energizingBackAfterEmptyFactor = 2f;
    #endregion

    public bool canRun { private set; get; }
    // Should be readonly parameters..
    [System.NonSerialized]
    private bool energyIsAt0 = false;

    void Update () {
        if(canRun)
        {
            energyIsAt0 = !(GetCurrentEnergy() >= 1f);
            canRun = !energyIsAt0;
            if (Input.GetKey(KeyCode.LeftShift) && canRun)
            {
                ChangeEnergyBar(-energyDecreasingFactor);
            }
            else
            {
                ChangeEnergyBar(energizingBackFactor);
            }
        }
        else
        {
            ChangeEnergyBar(energizingBackAfterEmptyFactor);
            canRun = (GetCurrentEnergy() == MAX_ENERGY_PLAYER);
        }
	}

    public float GetCurrentEnergy()
    {
        return energySprite.sizeDelta.x;
    }

    public void SetSizeEnergyBar(float size)
    {
        float newHungerValue = Mathf.Clamp(size, 0, MAX_ENERGY_PLAYER);
        energySprite.sizeDelta = new Vector2(newHungerValue, energySprite.sizeDelta.y);
    }

    private void ChangeEnergyBar(float amount)
    {
        SetSizeEnergyBar(energySprite.sizeDelta.x + amount);
    }
}