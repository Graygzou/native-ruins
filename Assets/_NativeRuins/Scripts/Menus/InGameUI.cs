using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private CanvasGroup hudCanvas;
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
        hudCanvas.alpha = 1.0f;
    }

    public void HideHUD()
    {
        hudCanvas.alpha = 0.0f;
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

    public void SetActiveIcon(int indice, bool state)
    {
        if (state)
        {
            transformationScript.GetWheelIcon(indice);
        }
        else
        {
            //transformationScript.GetWheelIcon(indice).GetComponent<Image>().sprite = ;
        }
    }

    public void CreateWheelIcons()
    {
        transformationScript.CreateWheelIcons();
    }

    public void UpdateWheelSelection(Vector3 positionMouse, bool bearUnlocked, bool pumaUnlocked)
    {
        if(positionMouse.x == 0.0f && positionMouse.y == 0.0f)
        {
            UpdateWheelSelectionMouse(Input.mousePosition, true, true);
        }
        // Si en dehors du centre de la roue :
        else
        {
            //Debug.Log("Update... X:" + positionMouse.x + ", y:" + positionMouse.y);
            bool foundSelectedForm = false;

            // for all the icons created && !foundSelectedForm
            for(int i = 0; i < transformationScript.GetNbIcon(); i++)
            {
                WheelIcon child = transformationScript.GetWheelIcon(i);
                if(child != null) {
                    if (foundSelectedForm = positionMouse.x <= child.maxX && positionMouse.x >= child.minX && positionMouse.y <= child.maxY && positionMouse.y >= child.minY)
                    {
                        FormsController.Instance.SetSelectedForm(child.type);
                        child.SetColor(Color.red);
                    }
                    else
                    {
                        child.SetColor(Color.white);
                    }
                }
            }
        }
    }

    public void UpdateWheelSelectionMouse(Vector3 positionMouse, bool bearUnlocked, bool pumaUnlocked)
    {
        // Données utiles à la sélection
        Vector3 centreScreen = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 difference = positionMouse - centreScreen;

        // Si en dehors du centre de la roue et une sourie presente et qu'elle est utilisée
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

            // TODOOOOOOO
            /*
            bool isCurrentFormSelected = false;
            // SELECTION HUMAIN
            if (isCurrentFormSelected = ((positionMouse.y > positionMouse.x * a1 + b1) && (positionMouse.y > positionMouse.x * a2 + b2)))
            {
                FormsController.Instance.SetSelectedForm(TransformationType.Human);
            }
            //transformationScript.humanSelected.SetActive(isCurrentFormSelected);

            // SELECTION OURS
            if (isCurrentFormSelected = ((positionMouse.y < positionMouse.x * a2 + b2) && (positionMouse.x > centreScreen.x) && bearUnlocked))
            {
                FormsController.Instance.SetSelectedForm(TransformationType.Bear);
            }
            //transformationScript.bearSelected.SetActive(isCurrentFormSelected);

            // SELECTION PUMA
            if (isCurrentFormSelected = ((positionMouse.y < positionMouse.x * a1 + b1) && (positionMouse.x < centreScreen.x) && pumaUnlocked))
            {
                FormsController.Instance.SetSelectedForm(TransformationType.Puma);
            }*/
            //transformationScript.pumaSelected.SetActive(isCurrentFormSelected);
        }
        else
        {
            for (int i = 0; i < transformationScript.GetNbIcon(); i++)
            {
                WheelIcon child = transformationScript.GetWheelIcon(i);
                if (child != null)
                {
                    child.SetColor(Color.white);
                }
            }
            FormsController.Instance.SetSelectedForm(TransformationType.None);
        }
    }
    #endregion
}
