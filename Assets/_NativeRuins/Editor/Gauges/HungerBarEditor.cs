using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HungerBar))]
public class HungerBarEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HungerBar hungerBarScript = (HungerBar)target;
        if(GUILayout.Button("Empty Hunger Bar"))
        {
            hungerBarScript.RestoreHungerFromData(0.0f);
        }
    }
}
