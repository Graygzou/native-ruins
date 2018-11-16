using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICircle : MaskableGraphic
{
    [Range(2, 200)]
    [SerializeField] private int numberOfPointsUsed = 5;
    [SerializeField] private int rayon = 7;
    [SerializeField] private bool fill = true;

    [SerializeField] private float angleInDegree = 90;
    [SerializeField] private int thickness = 5;

    Vector2[] points;

    public void InitHighlightForm()
    {
        points = new Vector2[numberOfPointsUsed];

        /*
        bottomLeftCorner = new Vector2(5, 0);
        topLeftCorner = new Vector2(7, 0);
        bottomRightCorner = new Vector2(5 * Mathf.Cos(90 * (Mathf.PI/180)), 5 * Mathf.Sin(90 * (Mathf.PI / 180)));
        topRightCorner = new Vector2(7 * Mathf.Cos(90 * (Mathf.PI / 180)), 7 * Mathf.Sin(90 * (Mathf.PI / 180)));*/

        // Starting points
        points[0] = new Vector2(rayon, 0);
        points[1] = new Vector2(thickness, 0);

        float dividedAngle = angleInDegree / (numberOfPointsUsed - 2);
        float currentAngle = dividedAngle;
        for (int i = 2; i < numberOfPointsUsed; i++)
        {
            if (i % 2 != 0)
            {
                // Rayon2
                points[i] = new Vector2(thickness * Mathf.Cos(currentAngle * (Mathf.PI / 180)), thickness * Mathf.Sin(currentAngle * (Mathf.PI / 180)));
            }
            else
            {
                // Rayon1
                points[i] = new Vector2(rayon * Mathf.Cos(currentAngle * (Mathf.PI / 180)), rayon * Mathf.Sin(currentAngle * (Mathf.PI / 180)));
            }
            currentAngle += dividedAngle;
        }

        //SetVerticesDirty();
    }

    public void CreateForm()
    {
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        InitHighlightForm();

        /*
         * TODO
        corner1.x -= rectTransform.pivot.x;
        corner1.y -= rectTransform.pivot.y;
        corner2.x -= rectTransform.pivot.x;
        corner2.y -= rectTransform.pivot.y;

        corner1.x *= rectTransform.rect.width;
        corner1.y *= rectTransform.rect.height;
        corner2.x *= rectTransform.rect.width;
        corner2.y *= rectTransform.rect.height;*/

        vh.Clear();

        UIVertex vert = UIVertex.simpleVert;

        foreach (Vector2 vector in points)
        {
            Debug.Log(vector);
            vert.position = vector;
            vert.color = color;
            vh.AddVert(vert);
        }

        if(numberOfPointsUsed > 2)
        {
            // Create first triangle
            vh.AddTriangle(0, 1, 2);

            for (int i = 1; (i+2) < numberOfPointsUsed; i++)
            {
                if (i % 2 == 0)
                {
                    vh.AddTriangle(i, i + 1, i + 2);
                }
                else
                {
                    vh.AddTriangle(i, i + 2, i + 1);
                }
            }
        }
    }
}
