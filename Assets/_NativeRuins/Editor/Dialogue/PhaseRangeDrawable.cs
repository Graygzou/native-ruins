using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PhaseRange))]
public class PhaseRangeDrawable : PropertyDrawer
{
    private const float SPACING = 17f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Retrieve all SerializedProperty property from the class
        SerializedProperty startIndex = property.FindPropertyRelative("startDialogueIndex");
        SerializedProperty endIndex = property.FindPropertyRelative("endDialogueIndex");

        EditorGUI.BeginProperty(position, label, property);
        Rect startIndexRecdt = new Rect(position.x, position.y + 2 * SPACING, 130, position.height);
        EditorGUI.LabelField(startIndexRecdt, "Phase range");

        Rect dialogueStartLabel = new Rect(position.x, position.y + 2 * SPACING, 130, position.height);
        EditorGUI.LabelField(dialogueStartLabel, "Phase range", "dialogue n°", new GUIStyle());

        EditorGUI.BeginChangeCheck();
        Rect startIndexRect = new Rect(position.x + 196, position.y + 2 * SPACING, 70, position.height);
        EditorGUI.PropertyField(startIndexRect, startIndex, GUIContent.none, true);

        Rect startIndexRecdtd = new Rect(position.x + 241, position.y + 2 * SPACING, 150, position.height);
        EditorGUI.LabelField(startIndexRecdtd, "to dialogue n°");

        Rect endIndexRect = new Rect(position.x + 331, position.y + 2 * SPACING, 70, position.height);
        EditorGUI.PropertyField(endIndexRect, endIndex, GUIContent.none, true);
        EditorGUI.EndProperty();

    }

}
