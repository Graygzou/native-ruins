using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Phase))]
public class PhaseDrawable : PropertyDrawer
{
    private static float SPACING = 17f;

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
            Rect cameraRect = new Rect(position.x, position.y + SPACING, position.width, position.height);
            EditorGUI.PropertyField(cameraRect, property.FindPropertyRelative("attachedCamera"), new GUIContent("Attached camera"), true);
            EditorGUI.EndProperty();

            EditorGUI.BeginProperty(position, label, property);
            Rect startIndexRecdt = new Rect(position.x, position.y + 2 * SPACING, 130, position.height);
            EditorGUI.LabelField(startIndexRecdt, "Phase range");

            Rect dialogueStartLabel = new Rect(position.x + 125, position.y + 2 * SPACING, 130, position.height);
            EditorGUI.LabelField(dialogueStartLabel, "dialogue n°");

            EditorGUI.BeginChangeCheck();
            Rect startIndexRect = new Rect(position.x + 196, position.y + 2 * SPACING, 70, position.height);
            EditorGUI.PropertyField(startIndexRect, property.FindPropertyRelative("startDialogueIndex"), GUIContent.none, true);

            Rect startIndexRecdtd = new Rect(position.x + 241, position.y + 2 * SPACING, 150, position.height);
            EditorGUI.LabelField(startIndexRecdtd, "to dialogue n°");

            Rect endIndexRect = new Rect(position.x + 331, position.y + 2 * SPACING, 70, position.height);
            EditorGUI.PropertyField(endIndexRect, property.FindPropertyRelative("endDialogueIndex"), GUIContent.none, true);
            EditorGUI.EndChangeCheck();
            if (GUI.changed)
            {
                CheckDialogueIndex(property);
            }
            EditorGUI.EndProperty();

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            SerializedProperty actions = property.FindPropertyRelative("_actions");
            Rect actionsRect = new Rect(position.x + 11, position.y + 3 * SPACING, position.width - 110, EditorGUI.GetPropertyHeight(actions));
            EditorGUI.PropertyField(actionsRect, actions, new GUIContent("Actions"), true);
            EditorGUI.EndChangeCheck();
            if (GUI.changed)
            {
                MatchRefToActions(position, property, actions);
            }
            if(actions.isExpanded)
            {
                Rect labelRefs = new Rect(position.x + 300, position.y + 4 * SPACING + 2, 118, position.height);
                EditorGUI.LabelField(labelRefs, "dialogue Refs");

                EditorGUI.BeginChangeCheck();
                // Display the references changes
                for (int i = 0; i < property.FindPropertyRelative("_dialogueSentenceReferences").arraySize; i++)
                {
                    Rect labelRef = new Rect(position.x + 305, position.y + 5 * SPACING + (i * 18) + 2, 60, position.height);
                    EditorGUI.LabelField(labelRef, "n°");

                    Rect refRect = new Rect(position.x + 327, position.y + 5 * SPACING + (i * 18) + 2, 75, position.height);
                    EditorGUI.PropertyField(refRect, property.FindPropertyRelative("_dialogueSentenceReferences").GetArrayElementAtIndex(i), GUIContent.none, true);
                }
                EditorGUI.EndChangeCheck();
                if (GUI.changed)
                {
                    CheckDialogueReference(property);
                }
            }
            EditorGUI.EndProperty();

            EditorGUI.BeginProperty(position, label, property);
            Rect canPlayerMoveRect = new Rect(position.x, position.y + 3 * SPACING + EditorGUI.GetPropertyHeight(actions), position.width, position.height);
            EditorGUI.PropertyField(canPlayerMoveRect, property.FindPropertyRelative("canPlayerMove"), new GUIContent("Can Player Move"), true);
            EditorGUI.EndProperty();
        }

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;
    }

    private void MatchRefToActions(Rect position, SerializedProperty property, SerializedProperty actions)
    {
        SerializedProperty refArray = property.FindPropertyRelative("_dialogueSentenceReferences");
        // Match the size of the two arrays
        if (actions.arraySize > refArray.arraySize)
        {
            // Add dummy elements
            for (int i = refArray.arraySize; i < actions.arraySize; i++)
            {
                property.FindPropertyRelative("_dialogueSentenceReferences").InsertArrayElementAtIndex(i);
            }
        }
        else if (actions.arraySize < refArray.arraySize)
        {
            // Remove last elements
            for (int i = refArray.arraySize - 1; i >= actions.arraySize; i--)
            {
                //Debug.Log("Elem :" + property.FindPropertyRelative("_dialogueSentenceReferences").GetArrayElementAtIndex(i).intValue);
                property.FindPropertyRelative("_dialogueSentenceReferences").DeleteArrayElementAtIndex(i);
            }
        }
        Debug.Log(property.FindPropertyRelative("_dialogueSentenceReferences").arraySize);
    }

    private void CheckDialogueReference(SerializedProperty property)
    {
        int startIndex = property.FindPropertyRelative("startDialogueIndex").intValue;
        int endIndex = property.FindPropertyRelative("endDialogueIndex").intValue;

        SerializedProperty currentRef;
        for (int i = 0; i < property.FindPropertyRelative("_dialogueSentenceReferences").arraySize; i++)
        {
            currentRef = property.FindPropertyRelative("_dialogueSentenceReferences").GetArrayElementAtIndex(i);
            currentRef.intValue = currentRef.intValue > endIndex ? endIndex : (currentRef.intValue < startIndex ? startIndex : currentRef.intValue);
        }
    }

    private void CheckDialogueIndex(SerializedProperty property)
    {
        int maxNumberDialogue = property.FindPropertyRelative("maxNumberDialogue").intValue;
        int startIndex = property.FindPropertyRelative("startDialogueIndex").intValue;
        int endIndex = property.FindPropertyRelative("endDialogueIndex").intValue;
        if(startIndex <= 0)
        {
            property.FindPropertyRelative("startDialogueIndex").intValue = 1;
        }
        if(endIndex > maxNumberDialogue)
        {
            property.FindPropertyRelative("endDialogueIndex").intValue = maxNumberDialogue;
        }
        if(endIndex < startIndex)
        {
            property.FindPropertyRelative("startDialogueIndex").intValue = endIndex;
        } else if(startIndex > endIndex)
        {
            property.FindPropertyRelative("endDialogueIndex").intValue = startIndex;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property) > 16 ? EditorGUI.GetPropertyHeight(property) - 90 : EditorGUI.GetPropertyHeight(property);
    }
}
