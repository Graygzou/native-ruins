using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class WheelItemCreatorTool
{
    private const string PATH_TO_SCRIPTABLE_OBJ_FOLDER = "Assets/_NativeRuins/ScriptablesObjects/";

    [MenuItem("Tools/Transformation/Create a wheel item")]
    public static TransformationForm CreateWheelItemMenu()
    {
        TransformationForm asset = null;
        float value = Mathf.Round(Random.Range(0, 100));
        try
        {
            asset = ScriptableObject.CreateInstance<TransformationForm>();
            // Try to get the folder to raise an error if the file already exists.
            Resources.Load(PATH_TO_SCRIPTABLE_OBJ_FOLDER + "Transformation/Form" + value + ".asset");
            AssetDatabase.CreateAsset(asset, PATH_TO_SCRIPTABLE_OBJ_FOLDER + "Transformation/Form" + value + ".asset");
            AssetDatabase.SaveAssets();
        }
        catch (UnityException e)
        {
            Debug.LogError("Trying to add an existing asset : " + PATH_TO_SCRIPTABLE_OBJ_FOLDER + "Transformation/Form" + value + ".asset!  Rename the existing asset to avoid any conflicts!");
        }

        return asset;
    }
}