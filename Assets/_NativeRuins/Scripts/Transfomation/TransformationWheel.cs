using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is used as a container. It allow to store all the wheel data.
/// </summary>
public class TransformationWheel : MonoBehaviour
{
    private const float WHEEL_WIDTH = 20f;

    [Header("Parent")]
    [SerializeField] private GameObject wheelObject;

    [Header("Icons settings")]
    [SerializeField] private GameObject defaultIconPrefab;
    [SerializeField] private Vector3 positionTopElement;

    private WheelIcon[] wheelIcons;

    public WheelIcon GetWheelIcon(int indice)
    {
        return wheelIcons[indice];
    }

    public int GetNbIcon()
    {
        return wheelIcons.Length;
    }

    public void CreateWheelIcons()
    {
        // Retrieve the dictionnary
        Dictionary<TransformationType, TransformationForm> dict = FormsController.Instance.GetAllForms();

        wheelIcons = new WheelIcon[dict.Keys.Count];

        // Update the Hierarchy
        DestroyExistingHierarchy(wheelObject.transform.childCount);
        CreateNewHierarchy(dict.Keys.Count);

        float angle = 360f / dict.Keys.Count;
        float currentAngle = 0.0f;

        // CreateHighlightForm(dict.Keys.Count, angle);

        // For all child GameObject of the wheel
        foreach (TransformationType type in dict.Keys)
        {
            Debug.Log("ANGLEEE :" + currentAngle + ", TYPE :" + type);

            // Get the script of the child
            WheelIcon iconScript = wheelObject.transform.GetChild((int)type).GetComponent<WheelIcon>();

            // Should be setup elsewhere... TODO?
            iconScript.type = type;

            // Setup the icon
            iconScript.SetupIcon(positionTopElement, currentAngle, (angle/2), dict[type].icon);

            currentAngle += angle;
        }
    }

    /// <summary>
    /// This method is called when creating the wheel to create custom highlight form depending on the number of children.
    /// </summary>
    private void CreateHighlightForm(int childrenNumber, float angle)
    {
        // TODO
        // Use the script found.
        // use this too : wheelWidth
    }

    #region Hierarchy methods
    /// <summary>
    /// Method used to remove all the children of the current Wheel GameObject
    /// </summary>
    private void DestroyExistingHierarchy(int nbChildWheel)
    {
        for (int i = nbChildWheel - 1; i >= 0; i--)
        {
            Transform child = wheelObject.transform.GetChild(i);
            child.transform.SetParent(null);
            if(Application.isEditor)
            {
                GameObject.DestroyImmediate(child.gameObject);
            }
            else
            {
                GameObject.Destroy(child.gameObject);
            }
            
        }
    }

    /// <summary>
    /// Method used to created all the icons of the transformation wheel
    /// </summary>
    private void CreateNewHierarchy(int dictionnaryLength)
    {
        // Instantiate GameObjects
        for (int i = 0; i < dictionnaryLength; i++)
        {
            // Add it to the transformationWheel array
            wheelIcons[i] = Instantiate(defaultIconPrefab, wheelObject.transform).GetComponent<WheelIcon>();

            Debug.Log("Created child : " + wheelIcons[i]);
        }
    }
    #endregion
}
