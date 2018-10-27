using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Phase))]
public class PhaseDrawable : PropertyDrawer
{
    private const float SPACING = 17f;

    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Retrieve all SerializedProperty property from the class
        SerializedProperty attachedCamera = property.FindPropertyRelative("attachedCamera");
        SerializedProperty startDialogueIndex = property.FindPropertyRelative("startDialogueIndex");
        SerializedProperty endDialogueIndex = property.FindPropertyRelative("endDialogueIndex");
        SerializedProperty dialogueSentenceReferences = property.FindPropertyRelative("_dialogueSentenceReferences");
        SerializedProperty canPlayerMove = property.FindPropertyRelative("canPlayerMove");

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
            EditorGUI.PropertyField(cameraRect, attachedCamera, new GUIContent("Attached camera"), true);
            EditorGUI.EndProperty();

            EditorGUI.BeginProperty(position, label, property);
            float currentY = position.y + 2 * SPACING;
            
            Rect dialogueStartLabel = new Rect(position.x, currentY, position.width - 500, position.height);
            EditorGUI.LabelField(dialogueStartLabel, "Phase range (dialogues)", "n°", new GUIStyle());

            EditorGUI.BeginChangeCheck();
            Rect dialogueStart = new Rect(position.width - 115, currentY, 60, position.height);
            EditorGUI.PropertyField(dialogueStart, startDialogueIndex, GUIContent.none, true);
            

            Rect endIndexLabelRect = new Rect(position.width - 80, currentY, 70, position.height);
            EditorGUI.LabelField(endIndexLabelRect, "to n°");

            Rect endIndexRect = new Rect(position.width - 47, currentY, 60, position.height);
            EditorGUI.PropertyField(endIndexRect, endDialogueIndex, GUIContent.none, true);

            /*
            Rect startIndexRecdtd = new Rect(position.x + 241, currentY, 150, position.height);
            EditorGUI.LabelField(startIndexRecdtd, "to dialogue n°");*/

            /*
            Rect endIndexRect = new Rect(position.x + 331, currentY, 70, position.height);
            EditorGUI.PropertyField(endIndexRect, endDialogueIndex, GUIContent.none, true);*/
            EditorGUI.EndChangeCheck();
            if (GUI.changed)
            {
                CheckDialogueIndex(property, startDialogueIndex, endDialogueIndex);
            }
            EditorGUI.EndProperty();

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            SerializedProperty actions = property.FindPropertyRelative("_actions");
            Rect actionsRect = new Rect(position.x + 11, position.y + 3 * SPACING, position.width - 75, EditorGUI.GetPropertyHeight(actions));
            EditorGUI.PropertyField(actionsRect, actions, new GUIContent("Actions"), true);
            EditorGUI.EndChangeCheck();
            if (GUI.changed)
            {
                MatchRefToActions(position, property, actions);
            }
            if(actions.isExpanded)
            {
                Rect labelRefs = new Rect(position.width - 75, position.y + 4 * SPACING + 2, 110, position.height);
                EditorGUI.LabelField(labelRefs, "Dial. Refs");

                EditorGUI.BeginChangeCheck();
                // Display the references changes
                for (int i = 0; i < dialogueSentenceReferences.arraySize; i++)
                {
                    Rect refLabel = new Rect(position.width - 65, position.y + 5 * SPACING + (i * 18) + 2, 50, position.height);
                    EditorGUI.LabelField(refLabel, "n°");

                    Rect refRect = new Rect(position.width - 47, position.y + 5 * SPACING + (i * 18) + 2, 60, position.height);
                    EditorGUI.PropertyField(refRect, dialogueSentenceReferences.GetArrayElementAtIndex(i), GUIContent.none, true);
                }
                EditorGUI.EndChangeCheck();
                if (GUI.changed)
                {
                    CheckDialogueReference(property, startDialogueIndex.intValue, endDialogueIndex.intValue);
                }
            }
            EditorGUI.EndProperty();

            EditorGUI.BeginProperty(position, label, property);
            Rect canPlayerMoveRect = new Rect(position.x, position.y + 3 * SPACING + EditorGUI.GetPropertyHeight(actions), position.width, position.height);
            EditorGUI.PropertyField(canPlayerMoveRect, canPlayerMove, new GUIContent("Can Player Move"), true);
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

    private void CheckDialogueReference(SerializedProperty property, int startIndex, int endIndex)
    {
        SerializedProperty currentRef;
        for (int i = 0; i < property.FindPropertyRelative("_dialogueSentenceReferences").arraySize; i++)
        {
            currentRef = property.FindPropertyRelative("_dialogueSentenceReferences").GetArrayElementAtIndex(i);
            currentRef.intValue = currentRef.intValue > endIndex ? endIndex : (currentRef.intValue < startIndex ? startIndex : currentRef.intValue);
        }
    }

    private void CheckDialogueIndex(SerializedProperty property, SerializedProperty startIndex, SerializedProperty endIndex)
    {
        int maxNumberDialogue = property.FindPropertyRelative("maxNumberDialogue").intValue;
        if(startIndex.intValue <= 0)
        {
            startIndex.intValue = 1;
        }
        if(endIndex.intValue > maxNumberDialogue)
        {
            endIndex.intValue = maxNumberDialogue;
        }
        if(startIndex.intValue > maxNumberDialogue)
        {
            startIndex.intValue = maxNumberDialogue;
        }
        if(startIndex.intValue > endIndex.intValue)
        {
            endIndex.intValue = startIndex.intValue;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property) > 16 ? EditorGUI.GetPropertyHeight(property) - 90 : EditorGUI.GetPropertyHeight(property);
    }
}
