using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TransformationForm))]
public class TransformationFormDrawer : PropertyDrawer
{
    private const float SPACING = 17f;

    SerializedProperty type;
    SerializedProperty form;
    SerializedProperty stats;
    SerializedProperty icon;
    SerializedProperty background;
    SerializedProperty color;
    SerializedProperty isUnlocked;

    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Retrieve all SerializedProperty property from the class
        type = property.FindPropertyRelative("type");
        form = property.FindPropertyRelative("form");
        stats = property.FindPropertyRelative("stats");
        icon = property.FindPropertyRelative("icon");
        background = property.FindPropertyRelative("background");
        color = property.FindPropertyRelative("color");
        isUnlocked = property.FindPropertyRelative("isUnlocked");

        // Don't make child fields be indented
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = indent + 1;

        Rect foldoutRect = new Rect(position.x, position.y, 120, 16);
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, "Forms");

        EditorGUI.BeginProperty(position, label, property);
        Rect cameraRect = new Rect(position.x, position.y, position.width, position.height);
        EditorGUI.PropertyField(cameraRect, type, new GUIContent(" "), true);
        EditorGUI.EndProperty();

        if (property.isExpanded)
        {
            EditorGUI.BeginProperty(position, label, property);
            Rect dialogueStart = new Rect(position.x, position.y + SPACING, position.width, position.height);
            EditorGUI.PropertyField(dialogueStart, form, new GUIContent("GameObject"), true);
            EditorGUI.EndProperty();

            EditorGUI.BeginProperty(position, label, property);
            Rect statsRect = new Rect(position.x, position.y + 2 * SPACING, position.width, position.height);
            EditorGUI.PropertyField(statsRect, stats, new GUIContent("Stats of the form"), true);
            EditorGUI.EndProperty();

            EditorGUI.BeginProperty(position, label, property);
            Rect iconRect = new Rect(position.x, position.y + 3 * SPACING, position.width, position.height);
            EditorGUI.PropertyField(iconRect, icon, new GUIContent("Form icon"), true);
            EditorGUI.EndProperty();

            EditorGUI.BeginProperty(position, label, property);
            Rect colorRect = new Rect(position.x, position.y + 4 * SPACING, position.width, position.height);
            EditorGUI.PropertyField(colorRect, color, new GUIContent("Color"), true);
            EditorGUI.EndProperty();

            EditorGUI.BeginProperty(position, label, property);
            Rect backgroundRect = new Rect(position.x, position.y + 5 * SPACING, position.width, position.height);
            EditorGUI.PropertyField(backgroundRect, background, new GUIContent("Background"), true);
            EditorGUI.EndProperty();

            EditorGUI.BeginProperty(position, label, property);
            Rect isUnlockRect = new Rect(position.x, position.y + 6 * SPACING, position.width, position.height);
            EditorGUI.PropertyField(isUnlockRect, isUnlocked, new GUIContent("IsUnlock"), true);
            EditorGUI.EndProperty();
        }

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property) > 16 ? EditorGUI.GetPropertyHeight(property) - 25 : EditorGUI.GetPropertyHeight(property);
    }
}
