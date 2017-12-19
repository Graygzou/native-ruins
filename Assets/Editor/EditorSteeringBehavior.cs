using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor;
#endif

[CustomEditor(typeof(SteeringBehavior))]
public class MyScriptEditor : Editor
{
    SerializedProperty m_wallAvoidanceOn;
    SerializedProperty m_obstacleAvoidanceOn;
    SerializedProperty m_wanderOn;
    SerializedProperty m_fleeOn;
    SerializedProperty m_seekOn;
    SerializedProperty m_boundingSphereRadius;
    SerializedProperty m_obstacleMaxDistance;
    SerializedProperty m_target_p;
    SerializedProperty m_minDetectionBoxLength;

    void OnEnable()
    {
        // Fetch the objects from the GameObject script to display in the inspector
        m_wallAvoidanceOn = serializedObject.FindProperty("wallAvoidanceOn");
        m_obstacleAvoidanceOn = serializedObject.FindProperty("obstacleAvoidanceOn");
        m_wanderOn = serializedObject.FindProperty("wanderOn");
        m_fleeOn = serializedObject.FindProperty("fleeOn");
        m_seekOn = serializedObject.FindProperty("seekOn");
        m_boundingSphereRadius = serializedObject.FindProperty("boundingSphereRadius");
        m_obstacleMaxDistance = serializedObject.FindProperty("obstacleMaxDistance");
        m_target_p = serializedObject.FindProperty("target_p");
        m_minDetectionBoxLength = serializedObject.FindProperty("minDetectionBoxLength");
    }

    override public void OnInspectorGUI()
    {
        //The variables and GameObject from the MyGameObject script are displayed in the Inspector with appropriate labels
        EditorGUILayout.PropertyField(m_wallAvoidanceOn, new GUIContent("Wall Avoidance:"));
        EditorGUILayout.PropertyField(m_obstacleAvoidanceOn, new GUIContent("Obstacle Avoidance:"));
        EditorGUILayout.PropertyField(m_wanderOn, new GUIContent("Wander:"));
        EditorGUILayout.PropertyField(m_fleeOn, new GUIContent("Flee:"));
        EditorGUILayout.PropertyField(m_seekOn, new GUIContent("Seek:"));
        EditorGUILayout.PropertyField(m_target_p, new GUIContent("Target:"));
        // TODO le refaire
        //using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(m_fleeOn || m_seekOn)))
        //{
        //    if (group.visible == true)
        //    {
        //        EditorGUI.indentLevel++;
        //        EditorGUILayout.PropertyField(m_target_p, new GUIContent("Target:"));
        //        EditorGUI.indentLevel--;
        //    }
        //}

        EditorGUILayout.PropertyField(m_boundingSphereRadius, new GUIContent("Detection Lenght:"));
        EditorGUILayout.PropertyField(m_obstacleMaxDistance, new GUIContent("Wander Radius:"));
        EditorGUILayout.PropertyField(m_minDetectionBoxLength, new GUIContent("Lenght Box Detection:"));

        // Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }
}
