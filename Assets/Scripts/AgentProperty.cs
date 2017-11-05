using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class AgentProperty : MonoBehaviour {

    // Attributes of the agent
    public int health;
    public int maxSpeed;
    public int mass;
    // To enable some transition
    public bool isMean;
    public float hungryIndicator = 0.0f;

    //public Animaux(float hea, float maxSpe, float mas) : this(hea, maxSpe, mas, 5) {
    //}

    public AgentProperty(float hea, float maxSpe, float mas) {
        
    }

    void Start() {
        //animal = GetComponent<NavMeshAgent>();
    }
    
    // POLISH
    //IEnumerator AccelerateWalk() {
    //    for (float i = 0f; i < 0.5f; i += 0.1f) {
    //        GetComponent<Animator>().SetFloat("Speed_f", i);
    //        yield return null;
    //    }
    //}

}

[CustomEditor(typeof(AgentProperty))]
public class MyScriptEditorAgent : Editor
{
    override public void OnInspectorGUI()
    {
        var myScript = target as AgentProperty;

        // For the behavior
        myScript.health = EditorGUILayout.IntField("Health:", myScript.health);
        myScript.maxSpeed = EditorGUILayout.IntField("Max Speed:", myScript.maxSpeed);
        myScript.mass = EditorGUILayout.IntField("Mass:", myScript.mass);
        myScript.isMean = EditorGUILayout.Toggle("IsMean:", myScript.isMean);
        myScript.hungryIndicator = EditorGUILayout.FloatField("Hunger:", myScript.hungryIndicator);

    }
}
