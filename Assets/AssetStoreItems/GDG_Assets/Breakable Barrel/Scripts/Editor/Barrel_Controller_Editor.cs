using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects, CustomEditor(typeof(Barrel_Controller))]
public class Barrel_Controller_Editor : Editor {
	
	
	private SerializedObject objectMaster;
	private SerializedProperty
		DL1Enabled,
		DL1BreakStrength,
		ShrinkDL1Collider,
		PlayDL0BreakSound,
		PlayDL0ParticleSystem,
		
		DL2Enabled,
		DL2BreakStrength,
		ShrinkDL2Collider,
		PlayDL1BreakSound,
		PlayDL1ParticleSystem,
		
		DL3Enabled,
		DL3BreakStrength,
		ShrinkDL3Collider,
		PlayDL2BreakSound,
		PlayDL2ParticleSystem,
		
		ShrinkColliderSize,
		ScaleTime,
		ChunkLifetime,
		
		OutsideMaterials,
		InsideMaterial,
		OverrideMaterials,
	
		overrideMass,
		objectMass,
		breakThoughLevel,
		PhysicsMaterial,
		
		audioVolume,
		breakSounds,
		explosionSound,
		
		BreakParticleSystem,
		ParticleSystemLifetime,
		
		cargoObjects,
		
		tagArray,
		explosionStrength;
	
	
		private bool _expanded;
	
		void OnEnable ()
		{
		objectMaster = new SerializedObject (targets);
		
		DL1Enabled = objectMaster.FindProperty ("DL1Enabled");
		DL1BreakStrength = objectMaster.FindProperty ("DL1BreakStrength");
		ShrinkDL1Collider = objectMaster.FindProperty ("shrinkDL1Collider");
		PlayDL0BreakSound = objectMaster.FindProperty ("playDL0BreakSound");
		PlayDL0ParticleSystem = objectMaster.FindProperty ("playDL0ParticleSystem");
		
		DL2Enabled = objectMaster.FindProperty ("DL2Enabled");	
		DL2BreakStrength = objectMaster.FindProperty ("DL2BreakStrength");
		ShrinkDL2Collider = objectMaster.FindProperty ("shrinkDL2Collider");
		PlayDL1BreakSound = objectMaster.FindProperty ("playDL1BreakSound");
		PlayDL1ParticleSystem = objectMaster.FindProperty ("playDL1ParticleSystem");
		
		DL3Enabled = objectMaster.FindProperty ("DL3Enabled");	
		DL3BreakStrength = objectMaster.FindProperty ("DL3BreakStrength");
		ShrinkDL3Collider = objectMaster.FindProperty ("shrinkDL3Collider");
		PlayDL2BreakSound = objectMaster.FindProperty ("playDL2BreakSound");
		PlayDL2ParticleSystem = objectMaster.FindProperty ("playDL2ParticleSystem");
		
		ShrinkColliderSize = objectMaster.FindProperty ("shrinkColliderSize");
		ScaleTime = objectMaster.FindProperty ("scaleTime");
		ChunkLifetime = objectMaster.FindProperty ("chunkLifetime");
		
		OutsideMaterials = objectMaster.FindProperty ("outsideMaterial");
		InsideMaterial = objectMaster.FindProperty ("insideMaterial");
		OverrideMaterials = objectMaster.FindProperty ("overrideMaterials");
		
		overrideMass = objectMaster.FindProperty ("overrideMass");
		objectMass = objectMaster.FindProperty ("objectMass");
		breakThoughLevel = objectMaster.FindProperty ("breakThoughLevel");
		PhysicsMaterial = objectMaster.FindProperty ("physicsMat");
		
		audioVolume = objectMaster.FindProperty ("audioVolume");
		breakSounds = objectMaster.FindProperty ("breakSounds");
		
		BreakParticleSystem = objectMaster.FindProperty ("breakParticleSystem");
		ParticleSystemLifetime = objectMaster.FindProperty ("particleSystemLifetime");
		
		cargoObjects = objectMaster.FindProperty ("cargoObjects");
		
		tagArray = objectMaster.FindProperty("tagArray");
		explosionStrength = objectMaster.FindProperty ("explosionStrength");
		explosionSound = objectMaster.FindProperty ("explosionSound");
		
	}
	
	public override void OnInspectorGUI ()
	{
		objectMaster.Update ();
		
		GUILayout.Label ("Fracture Options");
		EditorGUILayout.PropertyField (DL1Enabled);
		EditorGUILayout.PropertyField (DL1BreakStrength);
		EditorGUILayout.PropertyField (DL2Enabled);
		EditorGUILayout.PropertyField (DL2BreakStrength);
		EditorGUILayout.PropertyField (DL3Enabled);
		EditorGUILayout.PropertyField (DL3BreakStrength);
		EditorGUILayout.PropertyField (ShrinkDL1Collider);
		EditorGUILayout.PropertyField (ShrinkDL2Collider);
		EditorGUILayout.PropertyField (ShrinkDL3Collider);
		EditorGUILayout.PropertyField (ShrinkColliderSize);
		EditorGUILayout.PropertyField (ScaleTime);
		EditorGUILayout.PropertyField (ChunkLifetime);
		
		EditorGUILayout.Space ();
		
	GUILayout.Label ("Physics");
		EditorGUILayout.PropertyField (overrideMass);
		EditorGUILayout.PropertyField (objectMass);
		EditorGUILayout.PropertyField (breakThoughLevel);
		EditorGUILayout.PropertyField (PhysicsMaterial);
	
	EditorGUILayout.Space ();
		
		GUILayout.Label ("Materials");
		EditorGUILayout.PropertyField (OverrideMaterials);
		EditorGUILayout.PropertyField (OutsideMaterials);
		EditorGUILayout.PropertyField (InsideMaterial);
		
		EditorGUILayout.Space ();
		
		GUILayout.Label ("Sound");
		EditorGUILayout.PropertyField (audioVolume);
		EditorGUILayout.PropertyField (PlayDL0BreakSound);
		EditorGUILayout.PropertyField (PlayDL1BreakSound);
		EditorGUILayout.PropertyField (PlayDL2BreakSound);
	ArrayGUI (breakSounds, "breakSounds");
	
	EditorGUILayout.Space ();
	
	GUILayout.Label ("Particles");
	EditorGUILayout.PropertyField (BreakParticleSystem);
		EditorGUILayout.PropertyField (PlayDL0ParticleSystem);
		EditorGUILayout.PropertyField (PlayDL1ParticleSystem);
		EditorGUILayout.PropertyField (PlayDL2ParticleSystem);
		EditorGUILayout.PropertyField (ParticleSystemLifetime);
		
		EditorGUILayout.Space ();
		
		GUILayout.Label ("Cargo");
		ArrayGUI (cargoObjects, "cargoObjects");
		
		EditorGUILayout.Space ();
		
        ArrayGUI (tagArray, "NonBreaking tags");
		
		GUILayout.Label ("Explode Options");
			EditorGUILayout.PropertyField (explosionStrength);
			EditorGUILayout.PropertyField (explosionSound);
		
		objectMaster.ApplyModifiedProperties ();
	}
	
	void ArrayGUI (SerializedProperty obj, string name)
	{
		int size = obj.arraySize;
		
		int newSize = EditorGUILayout.IntField (name + " Size", size);
		
		if (newSize != size)
			obj.arraySize = newSize;
		
		EditorGUI.indentLevel = 1;
		
		for (int i = 0; i < obj.arraySize; i++) {
			EditorGUILayout.PropertyField (obj.GetArrayElementAtIndex (i));
		}
	}
}
