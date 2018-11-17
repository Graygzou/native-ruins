using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WheelItem))]
public class WheelItemDrawer : Editor
{
    private const float SPACING = 17f;

    protected WheelItem wheelItem;

    SerializedProperty stats;
    SerializedProperty icon;
    SerializedProperty background;

    public virtual void OnEnable()
    {
        wheelItem = (WheelItem)target;
    }

    // Draw the property inside the given rect
    public override void OnInspectorGUI()
    {
        Rect rect = Rect.zero;

        // Retrieve all SerializedProperty property from the class
        icon = serializedObject.FindProperty("icon");
        stats = serializedObject.FindProperty("stats");
        background = serializedObject.FindProperty("background");

        //int indent = EditorGUI.indentLevel;

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(icon, GUILayout.MinWidth(50f));
        if (icon.objectReferenceValue != null)
        {
            wheelItem.icon = icon.objectReferenceValue as Sprite;

            rect = EditorGUILayout.GetControlRect(true, 50, GUILayout.MaxWidth(100f));
            rect.yMin += 20;
            rect.yMax += 20;
            GUI.DrawTexture(rect, (icon.objectReferenceValue as Sprite).texture, ScaleMode.ScaleToFit);

            rect.height += 20;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        GUILayout.Space(rect.height);

        EditorGUILayout.BeginVertical();
        wheelItem.color = EditorGUILayout.ColorField("Color", wheelItem.color);
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        wheelItem.highlightColor = EditorGUILayout.ColorField("Highlight color", wheelItem.highlightColor);
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        wheelItem.isUnlocked = EditorGUILayout.Toggle("IsUnlocked", wheelItem.isUnlocked);
        EditorGUILayout.EndVertical();

        rect = Rect.zero;
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(background);
        if (background.objectReferenceValue != null)
        {
            wheelItem.background = background.objectReferenceValue as Sprite;

            rect = EditorGUILayout.GetControlRect(true, 50, GUILayout.MaxWidth(100f));
            GUI.DrawTexture(rect, (background.objectReferenceValue as Sprite).texture, ScaleMode.ScaleToFit);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }
}