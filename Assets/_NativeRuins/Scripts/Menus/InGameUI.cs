using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField]
    private CanvasGroup HUDCanvas;

    [Header("CrossHair UI")]
    [SerializeField]
    private CanvasGroup crosshair;
    [SerializeField]
    private Transform largeurCrossHair;
    [SerializeField]
    private Transform hauteurCrossHair;

    [Header("Transformation Ui")]
    [SerializeField]
    private CanvasGroup transformationWheel;

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

    public void DisplayTransformationWheel()
    {
        transformationWheel.alpha = 1.0f;
    }

    public void CloseTransformationWheel()
    {
        transformationWheel.alpha = 0.0f;
    }
}
