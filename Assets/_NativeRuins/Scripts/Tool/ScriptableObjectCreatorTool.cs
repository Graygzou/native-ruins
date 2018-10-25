using UnityEngine;
using System.Collections;
using UnityEditor;

public class ScriptableObjectCreatorTool
{
    [MenuItem("Tools/Inventory/Create InventoryItem List")]
    public static InventoryItemList CreateInventoryItemList()
    {
        InventoryItemList asset = ScriptableObject.CreateInstance<InventoryItemList>();

        AssetDatabase.CreateAsset(asset, "Assets/InventoryItemList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }

    [MenuItem("Tools/Dialogue/Create Dialogue List")]
    public static DialogueList CreateDialogueList()
    {
        DialogueList asset = ScriptableObject.CreateInstance<DialogueList>();

        AssetDatabase.CreateAsset(asset, "Assets/DialogueList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }

    [MenuItem("Tools/Dialogue/Create Dialogue")]
    public static Dialogue CreateDialogueMenu()
    {
        Dialogue asset = null;
        float value = Random.value;
        try
        {
            asset = ScriptableObject.CreateInstance<Dialogue>();
            Resources.Load("Assets/Dialogue"+ value + ".asset");
            AssetDatabase.CreateAsset(asset, "Assets/Dialogue" + value + ".asset");
            AssetDatabase.SaveAssets();
        }
        catch (UnityException e)
        {
            Debug.LogError("Trying to add an existing asset : Assets/Dialogue" + value + ".asset!  Rename the existing asset to avoid any conflicts!");
        }
        
        return asset;
    }


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
}