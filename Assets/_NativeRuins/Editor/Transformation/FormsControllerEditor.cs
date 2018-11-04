using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FormsController))]
public class FormsControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FormsController formController = (FormsController)target;
        if (GUILayout.Button("Update Transformation Wheel"))
        {
            formController.UpdateWheel();
        }
    }
}