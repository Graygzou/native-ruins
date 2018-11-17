using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class ScriptableObjectCreatorTool
{
    private const string PATH_TO_SCRIPTABLE_OBJ_FOLDER = "Assets/_NativeRuins/ScriptablesObjects/";

    #region Menu fonctions
    [MenuItem("Tools/Inventory/Create inventoryItem list")]
    public static InventoryItemList CreateInventoryItemList()
    {
        InventoryItemList asset = ScriptableObject.CreateInstance<InventoryItemList>();

        AssetDatabase.CreateAsset(asset, PATH_TO_SCRIPTABLE_OBJ_FOLDER + "Inventory/InventoryItemList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }

    [MenuItem("Tools/Dialogue/Create dialogue list")]
    public static DialogueList CreateDialogueList()
    {
        DialogueList asset = ScriptableObject.CreateInstance<DialogueList>();

        AssetDatabase.CreateAsset(asset, PATH_TO_SCRIPTABLE_OBJ_FOLDER + "Dialogue/DialogueList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }

    [MenuItem("Tools/Dialogue/Create dialogue")]
    public static Dialogue CreateDialogueMenu()
    {
        Dialogue asset = null;
        float value = Mathf.Round(Random.Range(0, 100));
        try
        {
            asset = ScriptableObject.CreateInstance<Dialogue>();
            // Try to get the folder to raise an error if the file already exists.
            Resources.Load(PATH_TO_SCRIPTABLE_OBJ_FOLDER + "Dialogue/Dialogue" + value + ".asset");
            AssetDatabase.CreateAsset(asset, PATH_TO_SCRIPTABLE_OBJ_FOLDER + "Dialogue/Dialogue" + value + ".asset");
            AssetDatabase.SaveAssets();
        }
        catch (UnityException e)
        {
            Debug.LogError("Trying to add an existing asset : " + PATH_TO_SCRIPTABLE_OBJ_FOLDER + "Dialogue/Dialogue" + value + ".asset!  Rename the existing asset to avoid any conflicts!");
        }
        
        return asset;
    }

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

    #endregion

    #region Helpers functions
    public static Dialogue CreateDialogue(int num)
    {
        Dialogue asset = null;
        if (AssetDatabase.LoadAssetAtPath("Assets/Dialogue" + num + ".asset", typeof(Dialogue)) != null)
        {
            // The asset exist so we raise an error in the editor.
            Debug.LogError("Trying to add an existing asset : Assets/Dialogue" + num + ".asset! Rename the existing asset to avoid any conflicts!");
        }
        else
        {
            // The asset doesn't exist so we can create it.
            asset = ScriptableObject.CreateInstance<Dialogue>();
            Resources.Load("Assets/Dialogue" + num + ".asset");
            AssetDatabase.CreateAsset(asset, "Assets/Dialogue" + num + ".asset");
            AssetDatabase.SaveAssets();
        }
        return asset;
    }
    #endregion
}