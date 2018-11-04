using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class used to make the wheel icon lisible in the transformation wheel.
/// </summary>
public class WheelIcon : MonoBehaviour {

    [SerializeField] private Image childImage;

    public TransformationType type;

    // Values used to detect the selection of the current icon
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public void SetupIcon(Vector3 positionTopElement, float currentAngle, float selectionAngle, Sprite sprite)
    {
        // Setup all others settings
        childImage.sprite = sprite;

        // Compute values usefull for selection
        float actualAngle = currentAngle + 90f;

        float aboveActualDegree = actualAngle + selectionAngle;
        float underActualDegree = actualAngle - selectionAngle;
        CosAssignments(Mathf.Cos(aboveActualDegree * Mathf.PI / 180f), Mathf.Cos(underActualDegree * Mathf.PI / 180f));
        SinAssignments(Mathf.Sin(aboveActualDegree * Mathf.PI / 180f), Mathf.Sin(underActualDegree * Mathf.PI / 180f));
 
        ComputeSpecialCases(actualAngle - selectionAngle, actualAngle + selectionAngle);

        // Setup his pivot in the center of the wheel to make it rotate around it.
        GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f + ((transform.localPosition - positionTopElement) / (childImage.GetComponent<RectTransform>().sizeDelta.y / 2)).y * 0.5f);

        // Place it well (with correct rotation for the object and the icon)
        GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, currentAngle);

        // Rotate the icon back to have him right
        childImage.gameObject.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, -currentAngle));
    }

    public void SetColor(Color color)
    {
        if(color == Color.red)
        {
            //Debug.Log("SELECTED : " + type);
        }
        childImage.color = color;
    }

    #region Assignments Helper
    private void CosAssignments(float x1, float x2)
    {
        if (x1 > x2)
        {
            maxX = x1;
            minX = x2;
        }
        else
        {
            maxX = x2;
            minX = x1;
        }
    }

    private void SinAssignments(float x1, float x2)
    {
        if (x1 > x2)
        {
            maxY = x1;
            minY = x2;
        }
        else
        {
            maxY = x2;
            minY = x1;
        }
    }

    private void ComputeSpecialCases(float underActualDegree, float aboveActualDegree)
    {
        if (underActualDegree <= 180f && aboveActualDegree >= 180f)
        {
            minX = -1.0f;
        }
        if (underActualDegree <= 90f && aboveActualDegree >= 90f)
        {
            maxY = 1.0f;
        }
        if (underActualDegree <= 270f && aboveActualDegree >= 270f)
        {
            minY = -1.0f;
        }
        if (underActualDegree <= 360f && aboveActualDegree >= 360f)
        {
            maxX = 1.0f;
        }
    }
    #endregion
}
