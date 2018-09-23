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
    private float energyDecreasingFactor = 0.001f;
    [SerializeField]
    private float energizingBackFactor = 0.002f;
    #endregion

    private bool canRun = true;
    // Should be readonly parameters..
    [System.NonSerialized]
    public bool energyIsAt0 = false;
	
	void Update () {
        if(canRun)
        {
            energyIsAt0 = !(GetCurrentEnergy() >= 1f);
            canRun = !energyIsAt0;
            if (Input.GetKey(KeyCode.LeftShift) && canRun)
            {
                ChangeEnergyBar(-energyDecreasingFactor);
            }
        }
        else
        {
            ChangeEnergyBar(energizingBackFactor);
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