using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is used as a container. It allow to store all the wheel data.
/// </summary>
public class TransformationWheel : MonoBehaviour
{
    [Header("Parent")]
    [SerializeField] private WheelUI wheelUI;

    public WheelIcon GetWheelIcon(int indice)
    {
        return wheelUI.GetWheelIcon(indice);
    }

    public int GetNbIcon()
    {
        return wheelUI.GetNbIcon();
    }

    public void CreateWheelIcons()
    {
        wheelUI.CreateWheelIcons();
    }
}
