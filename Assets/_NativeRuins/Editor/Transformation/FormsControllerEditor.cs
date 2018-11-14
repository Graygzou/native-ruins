using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FormsController))]
public class FormsControllerEditor : Editor
{
    FormsController formController;

    SerializedProperty availableFormsList;
    SerializedProperty lockIcon;
    SerializedProperty transformationWheelOpen;
    SerializedProperty transformationWheelRef;
    SerializedProperty transformationWheel;

    public void OnEnable()
    {
        availableFormsList = serializedObject.FindProperty("availableFormsList");
        lockIcon = serializedObject.FindProperty("lockIcon");
        transformationWheelOpen = serializedObject.FindProperty("transformationWheelOpen");
        transformationWheelRef = serializedObject.FindProperty("transformationWheelEditorRef");
        transformationWheel = serializedObject.FindProperty("_transformationWheel");
    }

    public override void OnInspectorGUI()
    {
        formController = (FormsController)target;

        // Draw all regular fields
        EditorGUILayout.PropertyField(availableFormsList, new GUIContent("Available Forms List"), true);
        EditorGUILayout.PropertyField(lockIcon, new GUIContent("Lock Icon"));

        GUI.enabled = false;
        EditorGUILayout.PropertyField(transformationWheelOpen, new GUIContent("Is Transformation Wheel Open"));
        GUI.enabled = true;

        EditorGUILayout.PropertyField(transformationWheelRef, new GUIContent("Use Transformation Wheel Ref"));
        GUI.enabled = transformationWheelRef.boolValue;

        // Retrieve the transformation wheel script directly
        EditorGUILayout.PropertyField(transformationWheel, new GUIContent("Transformation Wheel"));

        GUI.enabled = Application.isEditor;
        // Should be called in editor mode in order to make easy tweak
        if (GUILayout.Button("Generate Transformation Wheel"))
        {
            formController.Awake();
            formController.ResetForms();
            formController.TransformationWheel.CreateWheelIcons();
        }

        GUI.enabled = Application.isPlaying;
        // Should be call in game maybe....
        if (GUILayout.Button("Update Transformation Wheel"))
        {
            formController.UpdateWheel();
        }

        GUI.enabled = true;
        serializedObject.ApplyModifiedProperties();
    }
}