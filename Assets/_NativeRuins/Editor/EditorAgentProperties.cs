using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor;
#endif

[CustomEditor(typeof(AgentProperties))]
[CanEditMultipleObjects]
public class EditorAgentProperty : Editor
{
    SerializedProperty m_isMean, m_maxHealth, m_maxSpeed, m_mass;
    SerializedProperty m_damages, m_hungryIndicator;
    SerializedProperty m_visionRange, m_awarenessRange;
    SerializedProperty m_tauntRange;

    void OnEnable()
    {
        // Fetch the objects from the GameObject script to display in the inspector
        m_maxHealth = serializedObject.FindProperty("maxHealth");
        m_maxSpeed = serializedObject.FindProperty("maxSpeed");
        m_mass = serializedObject.FindProperty("mass");
        m_isMean = serializedObject.FindProperty("isMean");
        m_damages = serializedObject.FindProperty("damages");
        m_hungryIndicator = serializedObject.FindProperty("hungryIndicator");
        m_visionRange = serializedObject.FindProperty("isAlert");
        m_awarenessRange = serializedObject.FindProperty("playerTooClose");
        m_tauntRange = serializedObject.FindProperty("tauntRange");
    }

    override public void OnInspectorGUI()
    {
        //The variables and GameObject from the MyGameObject script are displayed in the Inspector with appropriate labels
        EditorGUILayout.PropertyField(m_maxHealth, new GUIContent("Max Health:"));
        EditorGUILayout.PropertyField(m_maxSpeed, new GUIContent("Max Speed:"));
        EditorGUILayout.PropertyField(m_mass, new GUIContent("Mass:"));
        EditorGUILayout.PropertyField(m_isMean, new GUIContent("IsMean:"));
        EditorGUILayout.PropertyField(m_damages, new GUIContent("Damages:"));
        EditorGUILayout.PropertyField(m_hungryIndicator, new GUIContent("Hunger:"));
        EditorGUILayout.PropertyField(m_visionRange, new GUIContent("VisionRange:"));
        EditorGUILayout.PropertyField(m_awarenessRange, new GUIContent("AwarenessRange:"));
        EditorGUILayout.PropertyField(m_tauntRange, new GUIContent("Taunt Range:"));

        // Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }
}