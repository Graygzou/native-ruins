using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TransformationForm))]
public class TransformationFormDrawer : WheelItemDrawer
{
    private const float SPACING = 17f;

    TransformationForm transformationForm;

    SerializedProperty type;
    SerializedProperty stats;
    SerializedProperty animatorController;
    SerializedProperty mesh;

    public override void OnEnable()
    {
        base.OnEnable();
        transformationForm = (TransformationForm)target;
    }

    // Draw the property inside the given rect
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        type = serializedObject.FindProperty("type");
        animatorController = serializedObject.FindProperty("animatorController");
        mesh = serializedObject.FindProperty("mesh");
        stats = serializedObject.FindProperty("stats");

        EditorGUILayout.BeginVertical();
        EditorGUILayout.PropertyField(type);
        transformationForm.type = (TransformationType)type.intValue;
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.PropertyField(animatorController);
        if (animatorController.objectReferenceValue != null)
        {
            transformationForm.animatorController = animatorController.objectReferenceValue as RuntimeAnimatorController;
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.PropertyField(mesh);
        if (mesh.objectReferenceValue != null)
        {
            transformationForm.mesh = mesh.objectReferenceValue as SkinnedMeshRenderer;
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.PropertyField(stats);
        if (stats.objectReferenceValue != null)
        {
            transformationForm.stats = stats.objectReferenceValue as PlayerProperties;
        }
        EditorGUILayout.EndVertical();
    }
}
