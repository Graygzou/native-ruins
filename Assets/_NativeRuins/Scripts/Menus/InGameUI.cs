﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private CanvasGroup HUDCanvas;
    [SerializeField] public LifeBar lifeBar;
    [SerializeField] public HungerBar hungerBar;
    [SerializeField] public EnergyBar energyBar;

    [Header("CrossHair UI")]
    [SerializeField] private CanvasGroup crosshair;
    [SerializeField] private Transform largeurCrossHair;
    [SerializeField] private Transform hauteurCrossHair;

    [Header("Transformation UI")]
    [SerializeField] private CanvasGroup transformationCanvas;
    [SerializeField] private TransformationWheel transformationScript;

    //public Transform aimCamHolder;
    private Vector3 _initLargeurCrossHair;
    private Vector3 _initHauteurCrossHair;

    void Awake()
    {
        if(largeurCrossHair == null)
        {
            largeurCrossHair = crosshair.transform.GetChild(0);
        }
        if (hauteurCrossHair == null)
        {
            hauteurCrossHair = crosshair.transform.GetChild(1);
        }
        _initLargeurCrossHair = largeurCrossHair.transform.localPosition;
        _initHauteurCrossHair = hauteurCrossHair.transform.localPosition;
    }

    public void DisplayHUD()
    {
        HUDCanvas.alpha = 1.0f;
    }

    public void HideHUD()
    {
        HUDCanvas.alpha = 0.0f;
    }

    public void EnableCrossHair()
    {
        crosshair.alpha = 1.0f;

        // Set his first position
        largeurCrossHair.transform.position = Input.mousePosition + _initLargeurCrossHair;
        hauteurCrossHair.transform.position = Input.mousePosition + _initHauteurCrossHair;
    }

    #region Players HUD methods



    #endregion


    #region TransformationWheel methods
    public void DisplayTransformationWheel()
    {
        transformationCanvas.alpha = 1.0f;
    }

    public void CloseTransformationWheel()
    {
        transformationCanvas.alpha = 0.0f;
    }

    public void SetActivePumaIcon(bool state)
    {
        if (state)
        {
            transformationScript.pumaImage.sprite = transformationScript.pumaIcon;
        }
        else
        {
            transformationScript.pumaImage.sprite = transformationScript.pumaLockIcon;
        }
    }

    public void SetActiveBearIcon(bool state)
    {
        if (state)
        {
            transformationScript.bearImage.sprite = transformationScript.bearIcon;
        }
        else
        {
            transformationScript.bearImage.sprite = transformationScript.bearLockIcon;
        }
    }

    public void UpdateWheelSelection(Vector3 positionMouse, bool bearUnlocked, bool pumaUnlocked)
    {
        // Si en dehors du centre de la roue :
        if (positionMouse.x != 0.0f && positionMouse.y != 0.0f)
        {
            //Debug.Log("Update... X:" + positionMouse.x + ", y:" + positionMouse.y);

            bool isCurrentFormSelected = false;
            // SELECTION HUMAIN
            if (isCurrentFormSelected = (positionMouse.x <= Mathf.Cos(Mathf.PI/4) || positionMouse.x >= Mathf.Cos(3 * Mathf.PI / 4)))
            {
                Debug.Log("Hoora !");
                FormsController.Instance.SetSelectedForm(FormsController.TransformationType.Human);
            }
            transformationScript.humanSelected.SetActive(isCurrentFormSelected);
            /*
            // SELECTION OURS
            if (isCurrentFormSelected = ((positionMouse.y < positionMouse.x * a2 + b2) && (positionMouse.x > centreScreen.x) && bearUnlocked))
            {
                FormsController.Instance.SetSelectedForm(FormsController.TransformationType.Bear);
            }
            transformationScript.bearSelected.SetActive(isCurrentFormSelected);

            // SELECTION PUMA
            if (isCurrentFormSelected = ((positionMouse.y < positionMouse.x * a1 + b1) && (positionMouse.x < centreScreen.x) && pumaUnlocked))
            {
                FormsController.Instance.SetSelectedForm(FormsController.TransformationType.Puma);
            }
            transformationScript.pumaSelected.SetActive(isCurrentFormSelected);*/
        }
        else
        {
            transformationScript.humanSelected.SetActive(false);
            transformationScript.bearSelected.SetActive(false);
            transformationScript.pumaSelected.SetActive(false);
        }
    }



    public void UpdateWheelSelectionMouse(Vector3 positionMouse, bool bearUnlocked, bool pumaUnlocked)
    {
        Debug.Log("Update... X:" + positionMouse.x + ", y:" + positionMouse.y);
        // Données utiles à la sélection
        Vector3 centreScreen = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 difference = positionMouse - centreScreen;

        // Si en dehors du centre de la roue :
        if (difference.magnitude > 125)
        {
            // Si sur le tiers du dessus :
            // coefficient directeur de la droite "gauche"
            float a1 = -182f / 312f;
            // "ordonnée à l'origine"
            float b1 = centreScreen.y - a1 * centreScreen.x;
            // coefficient directeur de la droite "droite"
            float a2 = -a1;
            // "ordonnée à l'origine"
            float b2 = centreScreen.y - a2 * centreScreen.x;

            bool isCurrentFormSelected = false;
            // SELECTION HUMAIN
            if (isCurrentFormSelected = ((positionMouse.y > positionMouse.x * a1 + b1) && (positionMouse.y > positionMouse.x * a2 + b2)))
            {
                FormsController.Instance.SetSelectedForm(FormsController.TransformationType.Human);
            }
            transformationScript.humanSelected.SetActive(isCurrentFormSelected);

            // SELECTION OURS
            if (isCurrentFormSelected = ((positionMouse.y < positionMouse.x * a2 + b2) && (positionMouse.x > centreScreen.x) && bearUnlocked))
            {
                FormsController.Instance.SetSelectedForm(FormsController.TransformationType.Bear);
            }
            transformationScript.bearSelected.SetActive(isCurrentFormSelected);

            // SELECTION PUMA
            if (isCurrentFormSelected = ((positionMouse.y < positionMouse.x * a1 + b1) && (positionMouse.x < centreScreen.x) && pumaUnlocked))
            {
                FormsController.Instance.SetSelectedForm(FormsController.TransformationType.Puma);
            }
            transformationScript.pumaSelected.SetActive(isCurrentFormSelected);
        }
        else
        {
            transformationScript.humanSelected.SetActive(false);
            transformationScript.bearSelected.SetActive(false);
            transformationScript.pumaSelected.SetActive(false);
        }
    }
    #endregion
}
