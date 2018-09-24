using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LifeBar))]
public class LifeBarEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LifeBar lifeBarScript = (LifeBar)target;
        if(GUILayout.Button("Kill Player"))
        {
            lifeBarScript.RestoreLifeFromData(0.0f);
        }
    }
}
