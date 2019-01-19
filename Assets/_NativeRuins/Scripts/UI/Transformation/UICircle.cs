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

    [SerializeField] private float startingAngle = 0.0f;
    [SerializeField] private float angleInDegree = 90.0f;
    [SerializeField] private int thickness = 5;

    private Vector2[] points;
    private int numberOfVertex;

    public void InitHighlightForm()
    {

        numberOfVertex = (numberOfPointsUsed + 1) * 2;
        points = new Vector2[numberOfVertex];

        // Starting points
        //points[0] = new Vector2(rayon, 0);
        //points[1] = new Vector2(thickness, 0);

        float dividedAngle = angleInDegree / numberOfPointsUsed;
        float currentAngle = startingAngle;
        for (int i = 0; i < numberOfVertex; i+=2)
        {
            // Rayon
            points[i] = new Vector2(rayon * Mathf.Cos(currentAngle * (Mathf.PI / 180)), rayon * Mathf.Sin(currentAngle * (Mathf.PI / 180)));
            if (!fill)
            {
                // Border thickness
                points[i+1] = new Vector2(thickness * Mathf.Cos(currentAngle * (Mathf.PI / 180)), thickness * Mathf.Sin(currentAngle * (Mathf.PI / 180)));
            }
            else
            {
                //  Center of the circle
                points[i+1] = Vector2.zero;
            }
            if(i >= numberOfVertex - 2)
            {
                // Last point to cover the entire angle
                currentAngle = angleInDegree;
            }
            else
            {
                currentAngle += dividedAngle;
            }
            
        }
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
            //Debug.Log(vector);
            vert.position = vector;
            vert.color = color;
            vh.AddVert(vert);
        }

        // Double check even if the Range should avoid that
        if(numberOfPointsUsed > 2)
        {
            // Create first triangle
            //vh.AddTriangle(0, 1, 2);

            for (int i = 0; (i+2) < numberOfVertex; i+=2)
            {
                vh.AddTriangle(i, i + 1, i + 2);
                vh.AddTriangle(i + 1, i + 2, i + 3);
            }
        }
    }
}
