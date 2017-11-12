/// <summary>
/// 
/// /// 11/1/2013
/// Steve Peters
/// GameDevelopersGuild.com
/// Miami Fl
/// 
/// 
/// Breakable object setup tool.
/// We're using Blender 2.69 and the cell fracture tool to generate the meshes for breaking
/// steps
/// examine the heirearchy imported from Blender
/// assign the correct folder structure in our project
/// create the nesessary prefabs
/// create and extend the required scripts
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Text;
using System;

public class BreakableObjectSetupTool : EditorWindow
{	
	[SerializeField]
	string
		objectName = "";						//the name of our breakable object

	private string BreakableObjectName;

	[SerializeField]
	GameObject
		importedMesh;
	
	//keeps track of the locations of our project folders
	string mainPath = "";
	//string importedMeshPath = "";
	string materialsPath = "";
	//string texturesPath = "";
	string prefabsPath = "";
	string prefabsLowerLevelsPath = "";
	string scriptsPath = "";
	[SerializeField]
	bool
		writeClasses = true, //determines if we want the editor to create and assign the breakable objects classes for us
		hasCargo = false;	  //Sets ups scripts so that they can accomidate cargo objects
		
	//it is assumed that DL1 will be enabled 
	[SerializeField]
	bool
		DL2Enabled = false,
		DL3Enabled = false;

	//We use this during the cell count section to turn off dl2 and dl3 if they don't exist
	bool AtLeastOneDL2 = false;
	bool AtLeastOneDL3 = false;

	//prevent attempts at building prefabs without the appropriate folders / scripts
	bool folderStructureHasBeenSetup = false;	

	//variables for collider types for each destruction level
	[SerializeField]
	colliderOptions
		DL0ColliderType,
		DL1ColliderType,
		DL2ColliderType,
		DL3ColliderType;

	private Vector3 _ObjectCenter = new Vector3 (0, 0, 0);		//store the center of the DL0 object
	private string _CellName = "cell";		//The String name cell that is removed from Blender imports
	private bool _IsCompiling = false;

	//=====================================================================================
	//private variables
	//=====================================================================================
			
	//Create Controller Object as an empty in the heirarchy
	GameObject _Object_Master; 
			
	//Create DL1 Master Controller Object as an empty in the heirarchy
	GameObject _DL1_Master_Temp;
			
	//Create DL1 Master Controller Object as an empty in the heirarchy
	GameObject _DL2_Master_Temp; 
	//Create DL1 Master Controller Object as an empty in the heirarchy
	GameObject _DL3_Master_Temp;

	bool breakableTagExists = false;		//If our required tag exists, add it to our prefabs
	bool generatePrefabs = true;

	//helper function to get component type
	//http://forum.unity3d.com/threads/update-changes-addcomponent-and-strings.308309/#post-2018213
	public static System.Type GetComponentTypeByName(string name)
	{
		if (string.IsNullOrEmpty(name)) return null;
		
		var ctp = typeof(Component);
		foreach (var assemb in System.AppDomain.CurrentDomain.GetAssemblies())
		{
			foreach(var tp in assemb.GetTypes())
			{
				if (ctp.IsAssignableFrom(tp) && tp.Name == name) return tp;
			}
		}
		
		return null;
	}


	[MenuItem("Window/GDG Breakable Object Setup Tool")]
	/// <summary>
	/// Shows the window.
	/// </summary>
	public static void ShowWindow ()
	{
		EditorWindow.GetWindow (typeof(BreakableObjectSetupTool), false, "Breakable");
	}

	void Update ()
	{
		if (EditorApplication.isCompiling) {
			_IsCompiling = true;
		} else {
			_IsCompiling = false;
		}


		//generate the prefabs once the folders are generated
		if(!_IsCompiling && folderStructureHasBeenSetup && generatePrefabs)
		{
			//prevent generating multiple prefabs
			generatePrefabs = false;

			SetName ();
			SetupBreakableObjects ();

			//Fries are done
			System.Media.SystemSounds.Beep.Play ();
		}
	}
	
	/// <summary>
	/// Raises the GUI event.
	/// </summary>
	void OnGUI ()
	{	
		// Undo is needed for all varible settings. Each of these must be serializable.
		Undo.RecordObject (this, "Changed Settings");

		GUILayout.Label ("GDG Breakable Object Setup Tool", EditorStyles.boldLabel);
		EditorGUILayout.Space ();	
		
		objectName = EditorGUILayout.TextField ("Object Name", objectName);
		importedMesh = (GameObject)EditorGUILayout.ObjectField ("Imported Mesh", importedMesh, typeof(GameObject), false);
		EditorGUILayout.Space ();
		
		EditorGUILayout.Space ();		

		writeClasses = EditorGUILayout.Toggle ("Write Classes?", writeClasses);
		hasCargo = EditorGUILayout.Toggle ("Object can contain cargo?", hasCargo);
		DL2Enabled = EditorGUILayout.Toggle ("Set up DL2 Objects", DL2Enabled);
		DL3Enabled = EditorGUILayout.Toggle ("Set up DL3 Objects", DL3Enabled);
		
		if (DL3Enabled) {
			DL2Enabled = true;
		}
		
		if (!DL2Enabled) {
			DL3Enabled = false;
		}
		
		EditorGUILayout.Space ();
		
		DL0ColliderType = (colliderOptions)EditorGUILayout.EnumPopup ("DL0 Collider Type", DL0ColliderType);
		DL1ColliderType = (colliderOptions)EditorGUILayout.EnumPopup ("DL1 Collider Type", DL1ColliderType);
		if (DL2Enabled) {
			DL2ColliderType = (colliderOptions)EditorGUILayout.EnumPopup ("DL2 Collider Type", DL2ColliderType);
		} else {
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
		}
		
		if (DL3Enabled) {
			DL3ColliderType = (colliderOptions)EditorGUILayout.EnumPopup ("DL3 Collider Type", DL3ColliderType);
		} else {
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
		}
		
		EditorGUILayout.Space ();
		

#region FolderSetup
		//Set up the folder structure
		if (!_IsCompiling) {
			if (GUILayout.Button ("Generate")) {
			
				SetName ();
			
				string guid = AssetDatabase.CreateFolder ("Assets", "Breakable " + objectName);
				mainPath = AssetDatabase.GUIDToAssetPath (guid);
		
				guid = AssetDatabase.CreateFolder (mainPath, "ImportedMeshes_Don'tTouch");
				//importedMeshPath = AssetDatabase.GUIDToAssetPath (guid);
			
				guid = AssetDatabase.CreateFolder (mainPath, "Materials");
				materialsPath = AssetDatabase.GUIDToAssetPath (guid);
			
				guid = AssetDatabase.CreateFolder (materialsPath, "Textures");
				//texturesPath = AssetDatabase.GUIDToAssetPath (guid);
			
				guid = AssetDatabase.CreateFolder (mainPath, "Prefabs");
				prefabsPath = AssetDatabase.GUIDToAssetPath (guid);
			
				guid = AssetDatabase.CreateFolder (prefabsPath, "LowerLevels_Don'tTouch");
				prefabsLowerLevelsPath = AssetDatabase.GUIDToAssetPath (guid);
			
				guid = AssetDatabase.CreateFolder (mainPath, "Scripts");
				scriptsPath = AssetDatabase.GUIDToAssetPath (guid);
				AssetDatabase.CreateFolder (scriptsPath, "Editor");
			
				// Refresh the AssetDatabase after all the changes
				AssetDatabase.Refresh ();
		
				//write all of our breakable object scripts
				WriteScripts ();

				//check if Breakable tag exists
				
				for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.tags.Length; i++) {
					//Debug.Log (UnityEditorInternal.InternalEditorUtility.tags[i]);
					if (UnityEditorInternal.InternalEditorUtility.tags[i].Contains("Breakable")) {
						breakableTagExists = true;
					}
				}

				if(!breakableTagExists)
				{
					Debug.LogWarning("Breakable tag not found. If you would like all generated objects to be tagged," +
					                 "create a new Breakable tag before proceeding to the next step");
				}
				//Allow prefabs to be built
				folderStructureHasBeenSetup = true;

				//turn off class writing
				writeClasses = false;
				generatePrefabs = true;
				BreakableObjectName = objectName;
			}

		} else if(_IsCompiling)
		{
			EditorGUILayout.Space ();
			EditorGUILayout.LabelField ("Unity is compiling, please wait.");
			EditorGUILayout.Space ();
		}

#endregion
		

	}

	
	//================================================================================
	//Helper Methods
	//================================================================================
	
	/// <summary>
	/// Sets up the breakable objects.
	/// Parses the object names and assigns them to thier proper parents while adding
	/// the required scripts and colliders
	/// </summary>
	void SetupBreakableObjects ()
	{
		//Instantiate the objects into the hierarchy so that we can work on them
		GameObject oo = Instantiate (importedMesh) as GameObject;
		GameObject[] originalObject = new GameObject[oo.transform.childCount];
			
		//Create Controller Object as an empty in the heirarchy
		_Object_Master = new GameObject ();
		_Object_Master.name = BreakableObjectName + "_Master";
			
		//Create DL1 Master Controller Object as an empty in the heirarchy
		_DL1_Master_Temp = new GameObject ();
		_DL1_Master_Temp.name = BreakableObjectName + "_DL1Master_Temp";
			
	
		//Create DL2 Master Controller Object as an empty in the heirarchy
		_DL2_Master_Temp = new GameObject ();
		_DL2_Master_Temp.name = BreakableObjectName + "_DL2Master_Temp";
	
		//Create DL3 Master Controller Object as an empty in the heirarchy
		GameObject _DL3_Master_Temp = new GameObject ();
		_DL3_Master_Temp.name = BreakableObjectName + "_DL3Master";


		string[] substring = new string[10];
			
		int i = 0;

		//================================================================================
		//assign all subchunks to parents
		//================================================================================
			

		foreach (Transform child in oo.transform) {

			if(breakableTagExists)
			{
			child.gameObject.tag = "Breakable";
			}

			originalObject [i] = child.gameObject;
			i++;
		}
				
		for (i = 0; i < originalObject.Length; i++) {
			substring = originalObject [i].name.ToString ().Split ('_');
			int cellCount = 0;
				
			//get cell count
			foreach (string word in substring) {
				if (word == "cell") {
					cellCount++;
				}
			}
				
			//special case - Blender's cell fracture tool names the object cell instead of cell_000, so we have to append _000 in the right places
			for (int f = 0; f < substring.Length; f++) {
				//check if we're in the last position in the index
				if (f == substring.Length - 1 && substring [f].Equals (_CellName)) {
					substring [f] = "000";
				}
					
				//check if current substring is cell, and the next is also cell to see if we need to insert "000"
				if (f < substring.Length - 1) {
					if (substring [f].Equals (_CellName) && substring [f + 1].Equals (_CellName)) {
						substring [f] = "000";
					}
				}
			}
				
			//we're going to rename the child object without the cells so that they're easier to sort
			//we need to put all of the parsed strings into a seperate array. We also need to add '000'
			//in places where there is 2 cells in a row, or a cell at the end of the array
			string[] tempArr = new string[cellCount + 1];
			int temp = 0;
				
			//populate the first spot in the tempArray with the object name
			tempArr [0] = BreakableObjectName;
				
			//put everything into a temp string
			foreach (string word in substring) {
					
				if (!word.Equals ("cell") && !word.Equals (BreakableObjectName) && !word.Equals ("Collider")) {
					tempArr [temp + 1] = word;
					temp++;
				}
			}
				
			for (int j = 0; j < tempArr.Length; j++) {
				//check for empty spot in the array, which signify where a 000 goes
				if (tempArr [j] == null || tempArr [j].Equals ("")) {
					tempArr [j] = "000";
				}
			}
				
				
			Debug.Log (string.Join ("_", tempArr) + "+++" + string.Join ("_", substring) + "=====" + tempArr.Length);
			originalObject [i].name = string.Join ("_", tempArr);
				
			//Assign chunk to relevant parent and assign scripts, colliders, etc
			if (DL3Enabled) {
				if (cellCount == 3) {
					originalObject [i].transform.parent = _DL3_Master_Temp.transform;
					//UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent (originalObject [i], "Assets/GDG_Assets/Scripts/Editor/BreakableObjectSetupTool.cs (360,6)", BreakableObjectName + "_DL3");
					originalObject [i].AddComponent(GetComponentTypeByName(this.BreakableObjectName + "_DL3"));
					SetCollider (originalObject [i], DL3ColliderType);
					originalObject [i].SetActive (false);		//special case for DL3 objects since they are children of an active object - rigidbody children of rigidbodies == crazy shit
					AtLeastOneDL3 = true;
				}
			}

			if (DL2Enabled) {
				if (cellCount == 2) {
					originalObject [i].transform.parent = _DL2_Master_Temp.transform;
					SetCollider (originalObject [i], DL2ColliderType);
					//UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent (originalObject [i], "Assets/GDG_Assets/Scripts/Editor/BreakableObjectSetupTool.cs (372,6)", BreakableObjectName + "_DL2");
					originalObject [i].AddComponent(GetComponentTypeByName(this.BreakableObjectName + "_DL2"));
					AtLeastOneDL2 = true;
				}
			}

			if (cellCount == 1) {
				originalObject [i].transform.parent = _DL1_Master_Temp.transform;
				SetCollider (originalObject [i], DL1ColliderType);

				//UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent (originalObject [i], "Assets/GDG_Assets/Scripts/Editor/BreakableObjectSetupTool.cs (382,5)", BreakableObjectName + "_DL1");
				originalObject [i].AddComponent(GetComponentTypeByName(this.BreakableObjectName + "_DL1"));
			}
				
			if (cellCount == 0) {
				//Get a reference to the center of the main object for our DL1Master
				_ObjectCenter = originalObject [i].transform.position;
					
				originalObject [i].transform.parent = _Object_Master.transform;
				SetCollider (originalObject [i], DL0ColliderType);
				//UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent (originalObject [i], "Assets/GDG_Assets/Scripts/Editor/BreakableObjectSetupTool.cs (392,5)", BreakableObjectName + "_DL0");
				originalObject [i].AddComponent(GetComponentTypeByName(this.BreakableObjectName + "_DL0"));
			}	
				
				
				
		}

		//turn off lower subchunk levels if they don't exist
		if (!AtLeastOneDL2) {
			DL2Enabled = false;
			DL3Enabled = false;
		}

		if (!AtLeastOneDL3) {
			DL3Enabled = false;
		}
			
		//Only one objectMaster so assign its scripts here:(include custom interface editor in the future)
		//UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent (_Object_Master, "Assets/GDG_Assets/Scripts/Editor/BreakableObjectSetupTool.cs (411,3)", BreakableObjectName + "_Controller");
		_Object_Master.AddComponent(GetComponentTypeByName(this.BreakableObjectName + "_Controller"));

				
			
			
		//reuse i;
		i = 0;
			
		//get a count of the number of DL1 masters we need so that we can set up an array
		foreach (Transform child in _DL1_Master_Temp.transform) {
			i++;
			Debug.Log (child.name);		//This removes a warning in the Unity editor by using the child object
		}
			
		GameObject[] dl1Objects = new GameObject[i];	//point to all of the child object in an array, since for each is missing one each time
		GameObject[] dl1Masters = new GameObject[i];	//initialize our dl2masters with an array size equal to the nukmber of dl1 masters
		GameObject[] dl2Masters = new GameObject[i];	//initialize our dl2masters with an array size equal to the nukmber of dl1 masters

		//Now we need to divide the DL1 chunck to thier individual DL1 Masters

		string[] dl1Substring = new string[i];		//arbitrary magic 5

		//reuse i, .....  again
		i = 0;
		foreach (Transform child in _DL1_Master_Temp.transform) {
			dl1Objects [i] = child.gameObject;
			i++;
		}
			
		//Assign our DL1 Master controller script
		for (i = 0; i < dl1Masters.Length; i++) {
			dl1Masters [i] = new GameObject ();
			dl1Masters [i].name = BreakableObjectName + "_DL1Master" + i.ToString ();
			//UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent (dl1Masters [i], "Assets/GDG_Assets/Scripts/Editor/BreakableObjectSetupTool.cs (445,4)", BreakableObjectName + "_DL1_Master");
			dl1Masters [i].AddComponent(GetComponentTypeByName(this.BreakableObjectName + "_DL1_Master"));

			//Set the origin of the DL1Master so that it is aligned with the DL0 object
			//will uncerimoniously fail if we are missing a DL0 object, need to add safeties
			dl1Masters [i].transform.position = _ObjectCenter;
		}
			
		for (i = 0; i < dl1Objects.Length; i++) {

			dl1Substring = dl1Objects [i].name.ToString ().Split ('_');
			if (int.Parse (dl1Substring [1]) > dl1Objects.Length) {
				Debug.Log ("There may be a problem with your input file. Be sure that all dl1 objects are numbered sequentially, start at 0,  and don't skip any numbers.");
			}
			dl1Objects [i].transform.parent = dl1Masters [int.Parse (dl1Substring [1])].transform;
		}
			
		//Since the DL3 objects are structured as children of the dl2 objects, we need to make them the children
		//of the DL2 before we parent the DL2 to thier masters
		string[] dl3Substring = new string[10];		//arbitrary magic 5
			
			
		//store a reference to the dl2 objects
		//reuse i, .....  again
		i = 0;
		foreach (Transform child in _DL2_Master_Temp.transform) {
			i++;
			Debug.Log (child.name);//this is mainly to supress the warning in editor
		}
			
		//store pointers to the dl2 objects
		GameObject[] dl2Objects = new GameObject[i];
			
		//again, because i is such a nice letter
		i = 0;
		foreach (Transform child in _DL2_Master_Temp.transform) {
			dl2Objects [i] = child.gameObject;
			i++;
		}

	
		//reuse i, .....  again
		i = 0;
		foreach (Transform child in _DL3_Master_Temp.transform) {
			i++;
			Debug.Log (child.name);//this is mainly to supress the warning in editor
		}
			
		//store pointers to the dl2 objects
		GameObject[] dl3Objects = new GameObject[i];
			
		//again, because i is such a nice letter
		i = 0;
		foreach (Transform child in _DL3_Master_Temp.transform) {
			dl3Objects [i] = child.gameObject;
			i++;
		}
			
		//Dl3 object must match parent on string[] locations 1 & 2
			
		for (i = 0; i < dl3Objects.Length; i++) {
			dl3Substring = dl3Objects [i].name.ToString ().Split ('_');
			//Debug.Log (string.Join ("_", dl3Substring));
			for (int k = 0; k < dl2Objects.Length; k++) {
				string[] matchString = dl2Objects [k].name.ToString ().Split ('_');
				if (matchString [1] == dl3Substring [1] && matchString [2] == dl3Substring [2]) {
					dl3Objects [i].transform.parent = dl2Objects [k].transform;
				}
			}
		}
			
		if (DL2Enabled) {
			//and the same for the dl2 chunks, except that the number of dl2 chunks must match the number of dl1 Masters
			string[] dl2Substring = new string[10];		//arbitrary magic 5
			
			
			
			//Assign our DL2 Master controller script

			for (i = 0; i < dl2Masters.Length; i++) {
				dl2Masters [i] = new GameObject ();
				dl2Masters [i].name = BreakableObjectName + "_DL2Master" + i.ToString ();
				//UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent (dl2Masters [i], "Assets/GDG_Assets/Scripts/Editor/BreakableObjectSetupTool.cs (527,5)", BreakableObjectName + "_DL2_Master");
				dl2Masters [i].AddComponent(GetComponentTypeByName(this.BreakableObjectName + "_DL2_Master"));

				//assign the position of the DL2Master to its corresponding DL1Master's Child
				//There shoul be a one to one correspondance between the DL1Masters and DL2Masters
				//dl2Masters [i].transform.position = dl1Masters [i].GetComponentInChildren<Transform> ().position;
				dl2Masters [i].transform.position = dl1Masters [i].transform.GetChild(0).position;
			}
			
			
			for (i = 0; i < dl2Objects.Length; i++) {
				dl2Substring = dl2Objects [i].name.ToString ().Split ('_');
				//	Debug.Log (string.Join ("_", dl2Substring));
				dl2Objects [i].transform.parent = dl2Masters [int.Parse (dl2Substring [1])].transform;
			}
		}
			
			
		//================================================================================
		//create prefabs from parents
		//================================================================================
			
		// Refresh the AssetDatabase after all the changes
		AssetDatabase.Refresh ();
			
		//I could probably compress this a bit an use fewer arrays, but I'm going to leave it like this for clarity
		//we're essentially creating our arrays and getting a reference to the path of each prefab, so that we can assign it to the controller script later
		GameObject[] DL1MasterPrefabs = new GameObject[dl1Masters.Length];
		string[] DL1PrefabsPath = new string[dl1Masters.Length];
			
		GameObject[] DL2MasterPrefabs = new GameObject[dl2Masters.Length];
		string[] DL2PrefabsPath = new string[dl2Masters.Length];

		for (int ii = 0; ii < dl1Masters.Length; ii++) {
			DL1MasterPrefabs [ii] = CreatePrefab (prefabsLowerLevelsPath, "_DL1_Master" + ii.ToString (), dl1Masters [ii]);
			DL1PrefabsPath [ii] = AssetDatabase.GetAssetPath (DL1MasterPrefabs [ii]);
		}
			
		if (DL2Enabled) {
			for (int ii = 0; ii < dl2Masters.Length; ii++) {
				DL2MasterPrefabs [ii] = CreatePrefab (prefabsLowerLevelsPath, "_DL2_Master" + ii.ToString (), dl2Masters [ii]);
				DL2PrefabsPath [ii] = AssetDatabase.GetAssetPath (DL2MasterPrefabs [ii]);
			}
		}

		//================================================================================
		//Set Variables for our controller prefab
		//================================================================================
			
		//Set the size of the arrays
		_Object_Master.GetComponent<PhysicsController> ().breakableChunks_DL1 = new PhysicsController_DL1_Master[dl1Masters.Length];
		if (DL2Enabled) {
			_Object_Master.GetComponent<PhysicsController> ().breakableChunks_DL2 = new PhysicsController_DL2_Master[dl2Masters.Length];
		}

		for (int ii = 0; ii < dl1Masters.Length; ii++) {
			_Object_Master.GetComponent<PhysicsController> ().breakableChunks_DL1 [ii] = AssetDatabase.LoadAssetAtPath (DL1PrefabsPath [ii], typeof(PhysicsController_DL1_Master)) as PhysicsController_DL1_Master;
			if (DL2Enabled) {
				_Object_Master.GetComponent<PhysicsController> ().breakableChunks_DL2 [ii] = AssetDatabase.LoadAssetAtPath (DL2PrefabsPath [ii], typeof(PhysicsController_DL2_Master)) as PhysicsController_DL2_Master;
			}
		}

			
		//create the controller prefab last since it should have a reference to the dl1 and dl2 masters by now, which we need to preserve
		GameObject contr = CreatePrefab (prefabsPath, "_Controller", _Object_Master);
				
		//================================================================================
		//cleanup unused stuff from the scene
		//================================================================================
			
		for (int ii = 0; ii < dl1Masters.Length; ii++) {
			DestroyImmediate (dl1Masters [ii]);
		}
			
	
		for (int ii = 0; ii < dl2Masters.Length; ii++) {
			DestroyImmediate (dl2Masters [ii]);
		}

			
		DestroyImmediate (_Object_Master);
		DestroyImmediate (_DL1_Master_Temp);
		DestroyImmediate (_DL2_Master_Temp);
		DestroyImmediate (_DL3_Master_Temp);
		DestroyImmediate (oo);
			
		// Refresh the AssetDatabase after all the changes
		AssetDatabase.Refresh ();
			
		//Instantiate a prefab in the scene so the user doesn't have to go looking for it
		PrefabUtility.InstantiatePrefab (contr);
		
	}
	
	/// <summary>
	/// Sets the name from the prefab in case the user doesn't enter one
	/// </summary>
	void SetName ()
	{
		if (objectName.Equals ("")) {
			objectName = importedMesh.name.ToString ();
		}
	}
	
	/// <summary>
	/// Writes the scripts for our breakable objects
	/// </summary>
	void WriteScripts ()
	{
		//================================================================================
		//make / assign scripts
		//================================================================================
		if (writeClasses) {
				
			//declare file
			string ObjectControllerScript = scriptsPath + "/" + objectName + "_Controller" + ".cs";
			Debug.Log ("Creating Classfile: " + ObjectControllerScript);
			
			//write file
			using (StreamWriter outFile =  new StreamWriter(ObjectControllerScript)) {
				outFile.WriteLine ("using UnityEngine;");
				outFile.WriteLine ("using System.Collections;");
				outFile.WriteLine ("");

				if (hasCargo) {
					outFile.WriteLine ("public class " + objectName + "_Controller" + " : PhysicsController_Cargo {");
				} else {
					outFile.WriteLine ("public class " + objectName + "_Controller" + " : PhysicsController {");
				}
				outFile.WriteLine ("}");
			}
			
			//declare file
			string ObjectDL1MasterScript = scriptsPath + "/" + objectName + "_DL1_Master" + ".cs";
			Debug.Log ("Creating Classfile: " + ObjectDL1MasterScript);
			
			//write file
			using (StreamWriter outfile =  new StreamWriter(ObjectDL1MasterScript)) {
				outfile.WriteLine ("using UnityEngine;");
				outfile.WriteLine ("using System.Collections;");
				outfile.WriteLine ("");
				outfile.WriteLine ("public class " + objectName + "_DL1_Master" + " : PhysicsController_DL1_Master {");
				outfile.WriteLine ("}");
			}
			
			//declare file
			if (DL2Enabled) {
				string ObjectDL2MasterScript = scriptsPath + "/" + objectName + "_DL2_Master" + ".cs";
				Debug.Log ("Creating Classfile: " + ObjectDL2MasterScript);
			
				//write file
				using (StreamWriter outfile =  new StreamWriter(ObjectDL2MasterScript)) {
					outfile.WriteLine ("using UnityEngine;");
					outfile.WriteLine ("using System.Collections;");
					outfile.WriteLine ("");
					outfile.WriteLine ("public class " + objectName + "_DL2_Master" + " : PhysicsController_DL2_Master {");
					outfile.WriteLine ("}");
				}
			}
			
			string ObjectDL0Script = scriptsPath + "/" + objectName + "_DL0" + ".cs";
			Debug.Log ("Creating Classfile: " + ObjectDL0Script);
			
			//write file
			using (StreamWriter outfile =  new StreamWriter(ObjectDL0Script)) {
				outfile.WriteLine ("using UnityEngine;");
				outfile.WriteLine ("using System.Collections;");
				outfile.WriteLine ("");
				if (hasCargo) {
					outfile.WriteLine ("public class " + objectName + "_DL0" + " : PhysicsController_DL0_Cargo {");
				} else {
					outfile.WriteLine ("public class " + objectName + "_DL0" + " : PhysicsController_DL0 {");
				}
				outfile.WriteLine ("");
				outfile.WriteLine ("override public void BreakAndDestroy ()");
				outfile.WriteLine ("{		");
				outfile.WriteLine ("	//This is where I normally update my points control system with the value of the broken block");
				outfile.WriteLine ("	base.BreakAndDestroy ();");
				outfile.WriteLine ("}");
				outfile.WriteLine ("}");
			}
			
			string ObjectDL1Script = scriptsPath + "/" + objectName + "_DL1" + ".cs";
			Debug.Log ("Creating Classfile: " + ObjectDL1Script);
			
			//write file
			using (StreamWriter outfile =  new StreamWriter(ObjectDL1Script)) {
				outfile.WriteLine ("using UnityEngine;");
				outfile.WriteLine ("using System.Collections;");
				outfile.WriteLine ("");
				outfile.WriteLine ("public class " + objectName + "_DL1" + " : PhysicsController_DL1 {");
				outfile.WriteLine ("override public void BreakAndDestroy ()");
				outfile.WriteLine ("{");
				outfile.WriteLine ("	//This is where I normally update my points control system with the value of the broken block");
				outfile.WriteLine ("	base.BreakAndDestroy ();");
				outfile.WriteLine ("}");
				outfile.WriteLine ("}");
			}

			if (DL2Enabled) {
				string ObjectDL2Script = scriptsPath + "/" + objectName + "_DL2" + ".cs";
				Debug.Log ("Creating Classfile: " + ObjectDL2Script);
			
				//write file
				using (StreamWriter outfile =  new StreamWriter(ObjectDL2Script)) {
					outfile.WriteLine ("using UnityEngine;");
					outfile.WriteLine ("using System.Collections;");
					outfile.WriteLine ("");
					outfile.WriteLine ("public class " + objectName + "_DL2" + " : PhysicsController_DL2 {");
					outfile.WriteLine ("}");
				}
			}

			if (DL3Enabled) {
				string ObjectDL3Script = scriptsPath + "/" + objectName + "_DL3" + ".cs";
				Debug.Log ("Creating Classfile: " + ObjectDL3Script);
			
				//write file
				using (StreamWriter outfile =  new StreamWriter(ObjectDL3Script)) {
					outfile.WriteLine ("using UnityEngine;");
					outfile.WriteLine ("using System.Collections;");
					outfile.WriteLine ("");
					outfile.WriteLine ("public class " + objectName + "_DL3" + " : PhysicsController_DL3 {");
					outfile.WriteLine ("}");
				}
			}
				

			string ObjectEditorScript = scriptsPath + "/Editor/" + objectName + "_Controller_Editor" + ".cs";
			Debug.Log ("Creating Classfile: " + ObjectEditorScript);
			
			//write file
			using (StreamWriter outfile =  new StreamWriter(ObjectEditorScript)) {
				outfile.WriteLine ("using UnityEngine;");
				outfile.WriteLine ("using UnityEditor;");
				outfile.WriteLine ("");
				outfile.WriteLine ("[CanEditMultipleObjects, CustomEditor(typeof(" + objectName + "_Controller))]");
				outfile.WriteLine ("public class " + objectName + "_Controller_Editor : Editor {");
				outfile.WriteLine ("	");
				outfile.WriteLine ("	");
				outfile.WriteLine ("	private SerializedObject objectMaster;");
				outfile.WriteLine ("	private SerializedProperty");
				outfile.WriteLine ("		DL1Enabled,");
				outfile.WriteLine ("		DL1BreakStrength,");
				outfile.WriteLine ("		ShrinkDL1Collider,");
				outfile.WriteLine ("		PlayDL0BreakSound,");
				outfile.WriteLine ("		PlayDL0ParticleSystem,");
				outfile.WriteLine ("		");

				if (DL2Enabled) {
					outfile.WriteLine ("		DL2Enabled,");
					outfile.WriteLine ("		DL2BreakStrength,");
					outfile.WriteLine ("		ShrinkDL2Collider,");
					outfile.WriteLine ("		PlayDL1BreakSound,");
					outfile.WriteLine ("		PlayDL1ParticleSystem,");
				}

				if (DL3Enabled) {
					outfile.WriteLine ("		");
					outfile.WriteLine ("		DL3Enabled,");
					outfile.WriteLine ("		DL3BreakStrength,");
					outfile.WriteLine ("		ShrinkDL3Collider,");
					outfile.WriteLine ("		PlayDL2BreakSound,");
					outfile.WriteLine ("		PlayDL2ParticleSystem,");
				}

				outfile.WriteLine ("		");
				outfile.WriteLine ("		ShrinkColliderSize,");
				outfile.WriteLine ("		ScaleTime,");
				outfile.WriteLine ("		ChunkLifetime,");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		OutsideMaterials,");
				outfile.WriteLine ("		InsideMaterial,");
				outfile.WriteLine ("		OverrideMaterials,");
				outfile.WriteLine ("	");
				outfile.WriteLine ("		overrideMass,");
				outfile.WriteLine ("		objectMass,");
				outfile.WriteLine ("		breakThoughLevel,");
				outfile.WriteLine ("		PhysicsMaterial,");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		audioVolume,");
				outfile.WriteLine ("		breakSounds,");
				outfile.WriteLine ("		explosionSound,");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		BreakParticleSystem,");
				outfile.WriteLine ("		ParticleSystemLifetime,");
				outfile.WriteLine ("		");

				if (hasCargo) {
					outfile.WriteLine ("		cargoObjects,");
				}

				outfile.WriteLine ("		");
				outfile.WriteLine ("		tagArray,");
				outfile.WriteLine ("		explosionStrength;");
				outfile.WriteLine ("	");
				outfile.WriteLine ("	");
				outfile.WriteLine ("		private bool _expanded;");
				outfile.WriteLine ("	");
				outfile.WriteLine ("		void OnEnable ()");
				outfile.WriteLine ("		{");
				outfile.WriteLine ("		objectMaster = new SerializedObject (targets);");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		DL1Enabled = objectMaster.FindProperty (\"DL1Enabled\");");
				outfile.WriteLine ("		DL1BreakStrength = objectMaster.FindProperty (\"DL1BreakStrength\");");
				outfile.WriteLine ("		ShrinkDL1Collider = objectMaster.FindProperty (\"shrinkDL1Collider\");");
				outfile.WriteLine ("		PlayDL0BreakSound = objectMaster.FindProperty (\"playDL0BreakSound\");");
				outfile.WriteLine ("		PlayDL0ParticleSystem = objectMaster.FindProperty (\"playDL0ParticleSystem\");");
				outfile.WriteLine ("		");
				if (DL2Enabled) {
					outfile.WriteLine ("		DL2Enabled = objectMaster.FindProperty (\"DL2Enabled\");	");
					outfile.WriteLine ("		DL2BreakStrength = objectMaster.FindProperty (\"DL2BreakStrength\");");
					outfile.WriteLine ("		ShrinkDL2Collider = objectMaster.FindProperty (\"shrinkDL2Collider\");");
					outfile.WriteLine ("		PlayDL1BreakSound = objectMaster.FindProperty (\"playDL1BreakSound\");");
					outfile.WriteLine ("		PlayDL1ParticleSystem = objectMaster.FindProperty (\"playDL1ParticleSystem\");");
					outfile.WriteLine ("		");
				}

				if (DL3Enabled) {
					outfile.WriteLine ("		DL3Enabled = objectMaster.FindProperty (\"DL3Enabled\");	");
					outfile.WriteLine ("		DL3BreakStrength = objectMaster.FindProperty (\"DL3BreakStrength\");");
					outfile.WriteLine ("		ShrinkDL3Collider = objectMaster.FindProperty (\"shrinkDL3Collider\");");
					outfile.WriteLine ("		PlayDL2BreakSound = objectMaster.FindProperty (\"playDL2BreakSound\");");
					outfile.WriteLine ("		PlayDL2ParticleSystem = objectMaster.FindProperty (\"playDL2ParticleSystem\");");
				}

				outfile.WriteLine ("		");
				outfile.WriteLine ("		ShrinkColliderSize = objectMaster.FindProperty (\"shrinkColliderSize\");");
				outfile.WriteLine ("		ScaleTime = objectMaster.FindProperty (\"scaleTime\");");
				outfile.WriteLine ("		ChunkLifetime = objectMaster.FindProperty (\"chunkLifetime\");");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		OutsideMaterials = objectMaster.FindProperty (\"outsideMaterial\");");
				outfile.WriteLine ("		InsideMaterial = objectMaster.FindProperty (\"insideMaterial\");");
				outfile.WriteLine ("		OverrideMaterials = objectMaster.FindProperty (\"overrideMaterials\");");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		overrideMass = objectMaster.FindProperty (\"overrideMass\");");
				outfile.WriteLine ("		objectMass = objectMaster.FindProperty (\"objectMass\");");
				outfile.WriteLine ("		breakThoughLevel = objectMaster.FindProperty (\"breakThoughLevel\");");
				outfile.WriteLine ("		PhysicsMaterial = objectMaster.FindProperty (\"physicsMat\");");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		audioVolume = objectMaster.FindProperty (\"audioVolume\");");
				outfile.WriteLine ("		breakSounds = objectMaster.FindProperty (\"breakSounds\");");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		BreakParticleSystem = objectMaster.FindProperty (\"breakParticleSystem\");");
				outfile.WriteLine ("		ParticleSystemLifetime = objectMaster.FindProperty (\"particleSystemLifetime\");");
				outfile.WriteLine ("		");

				if (hasCargo) {
					outfile.WriteLine ("		cargoObjects = objectMaster.FindProperty (\"cargoObjects\");");
				}

				outfile.WriteLine ("		");
				outfile.WriteLine ("		tagArray = objectMaster.FindProperty(\"tagArray\");");
				outfile.WriteLine ("		explosionStrength = objectMaster.FindProperty (\"explosionStrength\");");
				outfile.WriteLine ("		explosionSound = objectMaster.FindProperty (\"explosionSound\");");
				outfile.WriteLine ("		");
				outfile.WriteLine ("	}");
				outfile.WriteLine ("	");
				outfile.WriteLine ("	public override void OnInspectorGUI ()");
				outfile.WriteLine ("	{");
				outfile.WriteLine ("		objectMaster.Update ();");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		GUILayout.Label (\"Fracture Options\");");
				outfile.WriteLine ("		EditorGUILayout.PropertyField (DL1Enabled);");
				outfile.WriteLine ("		EditorGUILayout.PropertyField (DL1BreakStrength);");

				if (DL2Enabled) {
					outfile.WriteLine ("		EditorGUILayout.PropertyField (DL2Enabled);");
					outfile.WriteLine ("		EditorGUILayout.PropertyField (DL2BreakStrength);");
				}

				if (DL3Enabled) {
					outfile.WriteLine ("		EditorGUILayout.PropertyField (DL3Enabled);");
					outfile.WriteLine ("		EditorGUILayout.PropertyField (DL3BreakStrength);");
				}

				outfile.WriteLine ("		EditorGUILayout.PropertyField (ShrinkDL1Collider);");

				if (DL2Enabled) {
					outfile.WriteLine ("		EditorGUILayout.PropertyField (ShrinkDL2Collider);");
				}

				if (DL3Enabled) {
					outfile.WriteLine ("		EditorGUILayout.PropertyField (ShrinkDL3Collider);");
				}

				outfile.WriteLine ("		EditorGUILayout.PropertyField (ShrinkColliderSize);");
				outfile.WriteLine ("		EditorGUILayout.PropertyField (ScaleTime);");
				outfile.WriteLine ("		EditorGUILayout.PropertyField (ChunkLifetime);");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		EditorGUILayout.Space ();");
				outfile.WriteLine ("		");
				outfile.WriteLine ("	GUILayout.Label (\"Physics\");");
				outfile.WriteLine ("		EditorGUILayout.PropertyField (overrideMass);");
				outfile.WriteLine ("		EditorGUILayout.PropertyField (objectMass);");
				outfile.WriteLine ("		EditorGUILayout.PropertyField (breakThoughLevel);");
				outfile.WriteLine ("		EditorGUILayout.PropertyField (PhysicsMaterial);");
				outfile.WriteLine ("	");
				outfile.WriteLine ("	EditorGUILayout.Space ();");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		GUILayout.Label (\"Materials\");");
				outfile.WriteLine ("		EditorGUILayout.PropertyField (OverrideMaterials);");
				outfile.WriteLine ("		EditorGUILayout.PropertyField (OutsideMaterials);");
				outfile.WriteLine ("		EditorGUILayout.PropertyField (InsideMaterial);");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		EditorGUILayout.Space ();");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		GUILayout.Label (\"Sound\");");
				outfile.WriteLine ("		EditorGUILayout.PropertyField (audioVolume);");
				outfile.WriteLine ("		EditorGUILayout.PropertyField (PlayDL0BreakSound);");

				if (DL2Enabled) {
					outfile.WriteLine ("		EditorGUILayout.PropertyField (PlayDL1BreakSound);");
				}

				if (DL3Enabled) {
					outfile.WriteLine ("		EditorGUILayout.PropertyField (PlayDL2BreakSound);");
				}

				outfile.WriteLine ("	ArrayGUI (breakSounds, \"breakSounds\");");
				outfile.WriteLine ("	");
				outfile.WriteLine ("	EditorGUILayout.Space ();");
				outfile.WriteLine ("	");
				outfile.WriteLine ("	GUILayout.Label (\"Particles\");");
				outfile.WriteLine ("	EditorGUILayout.PropertyField (BreakParticleSystem);");
				outfile.WriteLine ("		EditorGUILayout.PropertyField (PlayDL0ParticleSystem);");

				if (DL2Enabled) {
					outfile.WriteLine ("		EditorGUILayout.PropertyField (PlayDL1ParticleSystem);");
				}

				if (DL3Enabled) {
					outfile.WriteLine ("		EditorGUILayout.PropertyField (PlayDL2ParticleSystem);");
				}

				outfile.WriteLine ("		EditorGUILayout.PropertyField (ParticleSystemLifetime);");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		EditorGUILayout.Space ();");
				outfile.WriteLine ("		");

				if (hasCargo) {
					outfile.WriteLine ("		GUILayout.Label (\"Cargo\");");
					outfile.WriteLine ("		ArrayGUI (cargoObjects, \"cargoObjects\");");
					outfile.WriteLine ("		");
				}

				outfile.WriteLine ("		EditorGUILayout.Space ();");
				outfile.WriteLine ("		");
				outfile.WriteLine ("        ArrayGUI (tagArray, \"NonBreaking tags\");");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		GUILayout.Label (\"Explode Options\");");
				outfile.WriteLine ("			EditorGUILayout.PropertyField (explosionStrength);");
				outfile.WriteLine ("			EditorGUILayout.PropertyField (explosionSound);");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		objectMaster.ApplyModifiedProperties ();");
				outfile.WriteLine ("	}");
				outfile.WriteLine ("	");
				outfile.WriteLine ("	void ArrayGUI (SerializedProperty obj, string name)");
				outfile.WriteLine ("	{");
				outfile.WriteLine ("		int size = obj.arraySize;");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		int newSize = EditorGUILayout.IntField (name + \" Size\", size);");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		if (newSize != size)");
				outfile.WriteLine ("			obj.arraySize = newSize;");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		EditorGUI.indentLevel = 1;");
				outfile.WriteLine ("		");
				outfile.WriteLine ("		for (int i = 0; i < obj.arraySize; i++) {");
				outfile.WriteLine ("			EditorGUILayout.PropertyField (obj.GetArrayElementAtIndex (i));");
				outfile.WriteLine ("		}");
				outfile.WriteLine ("	}");
				outfile.WriteLine ("}");

			}
			// Refresh the AssetDatabase after all the changes
			AssetDatabase.Refresh ();
		}
	}
	
	void SetCollider (GameObject o, colliderOptions c)
	{
		switch (c) {
		case colliderOptions.cube:
			{
				o.AddComponent <BoxCollider>();
				break;
			}
			
		case colliderOptions.mesh:
			{
				o.AddComponent <MeshCollider>();
				o.GetComponent<MeshCollider> ().convex = true;
				break;
			}
			
		case colliderOptions.sphere:
			{
				o.AddComponent <SphereCollider>();
				break;
			}
		default:
			Debug.LogError ("collider option not set for object " + o.name.ToString () + ". Adding box collider as default.");
			o.AddComponent <BoxCollider>();
			break;
		}
	}
	
	GameObject CreatePrefab (string path, string nameExtension, GameObject prefabObject)
	{
		return PrefabUtility.CreatePrefab (path + "/" + objectName + nameExtension + ".prefab", prefabObject as GameObject);
		
		//we can set this up to return an string asset path if we need to
		//return  AssetDatabase.GetAssetPath (DLPrefab);
	}
	
	private enum colliderOptions
	{
		cube,
		sphere,
		mesh
	}
}
