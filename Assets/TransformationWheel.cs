using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is used as a container. It allow to store all the wheel data.
/// </summary>
public class TransformationWheel : MonoBehaviour
{
    public GameObject defaultIconPrefab;

    public GameObject wheelObject;

    private GameObject[] wheelIcons;
    private int nbElements;

    public void CreateWheel(int size)
    {
        wheelIcons = new GameObject[size];
        nbElements = 0;
    }

    public void AddIconToWheel(GameObject icon)
    {
        if(nbElements < wheelIcons.Length)
        {
            nbElements++;
            wheelIcons[nbElements] = icon;
        }
    }

    public GameObject GetWheelIcon(int indice)
    {
        GameObject icon = null;
        if (indice > 0 && indice < wheelIcons.Length)
        {
            icon = wheelIcons[indice];
        }
        return icon;
    }

    public void RemoveLastIconToWheel(GameObject icon)
    {
        if(nbElements > 0)
        {
            nbElements--;
            wheelIcons[nbElements] = null;
        }
    }

    public void UpdateWheelIcons()
    {
        // Retrieve the dictionnary
        Dictionary<TransformationType, TransformationForm> dict = FormsController.Instance.GetAllForms();

        // If we need to instantiate GameObject, we do so (only if dict size update..)
        if (dict.Keys.Count > wheelObject.transform.childCount)
        {
            // Instantiate GameObjects
            foreach (TransformationType form in dict.Keys)
            {
                // Add it to the transformationWheel array
                Instantiate(defaultIconPrefab, wheelObject.transform);
            }
        }
        else if(dict.Keys.Count < wheelObject.transform.childCount)
        {
            //Remove GameObjects
            for (int i = transform.childCount; i > dict.Keys.Count; i--)
            {
                Transform child = wheelObject.transform.GetChild(i);
                child.transform.parent = null;
                Destroy(child.gameObject);
            }
        }

        foreach (TransformationForm form in dict.Values)
        {
            Debug.Log(form);
            

            // Setup all others settings

            // Place it well (with correct rotation and stuff)
        }
    }
}
