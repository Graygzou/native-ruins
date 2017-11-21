﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class FormsController : MonoBehaviour
{
    public GameObject HumanForm;
    public GameObject BearForm;
    public GameObject PumaForm;

    private bool PumaUnlocked;
    private bool BearUnlocked;
    private int currentForm;
    private int selectedForm;
    public GameObject transformationWheel;

    // Use this for initialization
    void Start()
    {
        PumaUnlocked = false;
        BearUnlocked = true;
        transformationWheel.SetActive(false);
        GameObject playerRoot = GameObject.Find("Player");
        int i = 0;
        while (i < playerRoot.transform.childCount)
        {
            if (playerRoot.transform.GetChild(i).gameObject.activeSelf)
            {
                currentForm = i;
            }
            i++;
        }

    }

    public int getCurrentForm()
    {
        return currentForm;
    }

    public void Update()
    {
        //Debug.Log(currentForm);
        if (Input.GetKey(KeyCode.A))
        {
            OpenTransformationWheel();
        } else
        {
            CloseTransformationWheel();
            if (selectedForm != currentForm)
            {
                Transformation();
            }
        }
    }

    private void OpenTransformationWheel()
    {
        // Verification des formes disponibles
        if (!PumaUnlocked)
        {
            GameObject.Find("Affichages/TransformationSystem/Wheel/Fond/IconPuma").SetActive(false);
            GameObject.Find("Affichages/TransformationSystem/Wheel/Fond/IconPumaLocked").SetActive(true);
        }

        if (!BearUnlocked)
        {
            GameObject.Find("Affichages/TransformationSystem/Wheel/Fond/IconBear").SetActive(false);
            GameObject.Find("Affichages/TransformationSystem/Wheel/Fond/IconBearLocked").SetActive(true);
        }


        // Temps ralenti
        Time.timeScale = 0.1f;

        // Affichage de la roue
        transformationWheel.SetActive(true);
        
        
        // Données utiles à la sélection
        Vector3 centreScreen = new Vector3(Screen.width / 2, Screen.height/2, 0);
        Vector3 positionMouse = Input.mousePosition;
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

            // SELECTION HUMAIN
            if ((positionMouse.y > positionMouse.x*a1 + b1) && (positionMouse.y > positionMouse.x*a2 + b2))
            {
                selectedForm = 0;
                GameObject.Find("Affichages/TransformationSystem/Wheel/Fond/IconHumanSelected").SetActive(true);
            } else
            {
                GameObject.Find("Affichages/TransformationSystem/Wheel/Fond/IconHumanSelected").SetActive(false);
            }

            // SELECTION PUMA
            if ((positionMouse.y < positionMouse.x*a1 + b1) && (positionMouse.x < centreScreen.x) && PumaUnlocked)
            {
                selectedForm = 2;
                GameObject.Find("Affichages/TransformationSystem/Wheel/Fond/IconPumaSelected").SetActive(true);
            } else
            {
                GameObject.Find("Affichages/TransformationSystem/Wheel/Fond/IconPumaSelected").SetActive(false);
            }

            // SELECTION OURS
            if ((positionMouse.y < positionMouse.x * a2 + b2) && (positionMouse.x > centreScreen.x) && BearUnlocked)
            {
                selectedForm = 1;
                GameObject.Find("Affichages/TransformationSystem/Wheel/Fond/IconBearSelected").SetActive(true);
            } else
            {
                GameObject.Find("Affichages/TransformationSystem/Wheel/Fond/IconBearSelected").SetActive(false);
            }
        } else
        {
            selectedForm = currentForm;
            GameObject.Find("Affichages/TransformationSystem/Wheel/Fond/IconHumanSelected").SetActive(false);
            GameObject.Find("Affichages/TransformationSystem/Wheel/Fond/IconPumaSelected").SetActive(false);
            GameObject.Find("Affichages/TransformationSystem/Wheel/Fond/IconBearSelected").SetActive(false);
        }
    }

    private void CloseTransformationWheel()
    {
        Time.timeScale = 1;
        transformationWheel.SetActive(false);
    }

    private void Transformation()
    {
        // Desactiver forme actuelle
        if (currentForm == 0)
        {
            HumanForm.SetActive(false);
        }
        if (currentForm == 1)
        {
            BearForm.SetActive(false);
        }
        if (currentForm == 2)
        {
            PumaForm.SetActive(false);
        }

        // Activation nouvelle forme
        if (selectedForm == 0)
        {
            HumanForm.SetActive(true);
            currentForm = 0;
        }
        if (selectedForm == 1)
        {
            BearForm.SetActive(true);
            currentForm = 1;
        }
        if (selectedForm == 2)
        {
            PumaForm.SetActive(true);
            currentForm = 2;
        }
    }
}
