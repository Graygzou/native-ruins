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
    [SerializeField]
    private GameObject wheelObject;

    [Header("Icons settings")]
    [SerializeField]
    private GameObject defaultIconPrefab;
    [SerializeField]
    private Vector3 positionTopElement;

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

        // If we need to instantiate GameObject, we do so (only if dict size update..)
        if (dict.Keys.Count > wheelObject.transform.childCount)
        {
            // Instantiate GameObjects
            for (int i = wheelObject.transform.childCount; i < dict.Keys.Count; i++)
            {
                // Add it to the transformationWheel array
                wheelIcons[i] = Instantiate(defaultIconPrefab, wheelObject.transform).GetComponent<WheelIcon>();

                Debug.Log("Created child : " + wheelIcons[i]);
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

                wheelIcons[i] = null;

                Debug.Log("Destroy child : " + wheelIcons[i]);
            }
        }

        float angle = 360f / dict.Keys.Count;
        float currentAngle = 0.0f;
        // For all child GameObject of the wheel
        foreach (TransformationType type in dict.Keys)
        {
            // Get the script of the child
            WheelIcon iconScript = wheelObject.transform.GetChild((int)type).GetComponent<WheelIcon>();

            // Should be setup elsewhere
            iconScript.type = type;

            // Setup the icon
            iconScript.SetupIcon(positionTopElement, currentAngle, (angle/2), dict[type].icon);

            currentAngle += angle;
        }
    }
}
