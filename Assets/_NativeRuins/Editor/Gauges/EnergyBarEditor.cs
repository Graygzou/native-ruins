using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnergyBar))]
public class EnergyBarEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EnergyBar energyBarScript = (EnergyBar)target;
        if(GUILayout.Button("Empty energy Bar"))
        {
            energyBarScript.SetSizeEnergyBar(0.0f);
        }
    }
}
