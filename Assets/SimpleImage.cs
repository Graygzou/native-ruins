using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SimpleImage : Graphic
{
    [SerializeField] private Texture texture;

    public override Texture mainTexture
    {
        get
        {
            Debug.Log("la");
            return texture == null ? base.mainTexture : texture;
        }
    }

    public Texture Texture
    {
        get
        {
            return texture;
        }
        set
        {
            Debug.Log("ici");
            if (texture == value)
            {
                return;
            }
            texture = value;
            SetVerticesDirty();
            SetMaterialDirty();
        }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        Debug.Log(defaultGraphicMaterial);
        Debug.Log(mainTexture == Texture2D.whiteTexture);
        Debug.Log(texture);

        Vector2 corner1 = Vector2.zero;
        Vector2 corner2 = Vector2.zero;

        corner1.x = 0f;
        corner1.y = 0f;
        corner2.x = 1f;
        corner2.y = 1f;

        corner1.x -= rectTransform.pivot.x;
        corner1.y -= rectTransform.pivot.y;
        corner2.x -= rectTransform.pivot.x;
        corner2.y -= rectTransform.pivot.y;

        corner1.x *= rectTransform.rect.width;
        corner1.y *= rectTransform.rect.height;
        corner2.x *= rectTransform.rect.width;
        corner2.y *= rectTransform.rect.height;

        vh.Clear();

        UIVertex vert = UIVertex.simpleVert;

        vert.position = new Vector2(corner1.x, corner1.y);
        vert.color = color;
        vert.uv0 = new Vector2(1, 1);
        vh.AddVert(vert);

        vert.position = new Vector2(corner1.x, corner2.y);
        vert.color = color;
        vert.uv0 = new Vector2(1, 0);
        vh.AddVert(vert);

        vert.position = new Vector2(corner2.x, corner2.y);
        vert.color = color;
        vert.uv0 = new Vector2(0, 0);
        vh.AddVert(vert);

        vert.position = new Vector2(corner2.x, corner1.y);
        vert.color = color;
        vert.uv0 = new Vector2(0, 1);
        vh.AddVert(vert);

        /*
        vert.position = new Vector2(-100 , 0);
        vert.color = color;
        vh.AddVert(vert);*/

        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 3, 0);
        //vh.AddTriangle(0, 4, 1);
    }
}