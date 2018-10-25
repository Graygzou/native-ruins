using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Phase))]
public class PhaseDrawable : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        // Draw label
        //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = indent + 1;

        // Calculate rects
        
        Rect foldoutRect = new Rect(position.x, position.y, position.width, 16);
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, "Phase");

        if (property.isExpanded)
        {
            EditorGUI.BeginProperty(position, label, property);
            Rect cameraRect = new Rect(position.x, position.y + 16, position.width, position.height);
            EditorGUI.PropertyField(cameraRect, property.FindPropertyRelative("attachedCamera"), new GUIContent("Attached camera"), true);
            EditorGUI.EndProperty();

            EditorGUI.BeginProperty(position, label, property);
            Rect startIndexRect = new Rect(position.x, position.y + 32, position.width, position.height);
            EditorGUI.PropertyField(startIndexRect, property.FindPropertyRelative("startDialogueIndex"), new GUIContent("Start index"), true);
            EditorGUI.EndProperty();

            EditorGUI.BeginProperty(position, label, property);
            Rect endIndexRect = new Rect(position.x, position.y + 48, position.width, position.height);
            EditorGUI.PropertyField(endIndexRect, property.FindPropertyRelative("endDialogueIndex"), new GUIContent("End index"), true);
            EditorGUI.EndProperty();

            EditorGUI.BeginProperty(position, label, property);
            SerializedProperty actions = property.FindPropertyRelative("actions");
            var actionsRect = new Rect(position.x + 11, position.y + 64, position.width, EditorGUI.GetPropertyHeight(actions));
            EditorGUI.PropertyField(actionsRect, actions, new GUIContent("Actions"), true);
            EditorGUI.EndProperty();

            EditorGUI.BeginProperty(position, label, property);
            Rect canPlayerMoveRect = new Rect(position.x, position.y + 64 + EditorGUI.GetPropertyHeight(actions), position.width, position.height);
            EditorGUI.PropertyField(canPlayerMoveRect, property.FindPropertyRelative("canPlayerMove"), new GUIContent("Can Player Move"), true);
            EditorGUI.EndProperty();

            Rect canPlayerMoveRecdt = new Rect(position.x, position.y + 80 + EditorGUI.GetPropertyHeight(actions), position.width - 50, position.height);
            EditorGUI.PropertyField(canPlayerMoveRecdt, property.FindPropertyRelative("myVector"), true);
            //EditorGUILayout.PropertyField(property.FindPropertyRelative("myVector"));

        }
        
        // Set indent back to what it was
        EditorGUI.indentLevel = indent;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property) > 16 ? EditorGUI.GetPropertyHeight(property) - 11 : EditorGUI.GetPropertyHeight(property);
    }
}
