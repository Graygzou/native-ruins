using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class used to make the wheel icon lisible in the transformation wheel.
/// </summary>
public class WheelIcon : MonoBehaviour {

    [SerializeField]
    private Image childImage;

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
        Debug.Log("ActualAngle :" + actualAngle);
        Debug.Log("SelectionAngle :" + selectionAngle);

        maxX = Mathf.Cos(actualAngle + selectionAngle);
        minX = Mathf.Cos(actualAngle - selectionAngle);

        Debug.Log("Test1 :" + (actualAngle + selectionAngle));
        Debug.Log("Res Test1 :" + Mathf.Cos(actualAngle + selectionAngle));

        maxY = Mathf.Max(Mathf.Sin(actualAngle), Mathf.Sin(currentAngle + selectionAngle));
        minY = Mathf.Min(Mathf.Sin(actualAngle), Mathf.Sin(currentAngle - selectionAngle));

        //Debug.Log(new Vector2(0.5f, ((transform.localPosition - positionTopElement) / (GetComponent<RectTransform>().sizeDelta.y / 2)).y * 0.5f));

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
            Debug.Log("SELECTED : " + type);
        }
        childImage.color = color;
    }
}
