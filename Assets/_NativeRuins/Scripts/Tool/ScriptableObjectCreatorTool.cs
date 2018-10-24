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
        Dialogue asset = ScriptableObject.CreateInstance<Dialogue>();

        AssetDatabase.CreateAsset(asset, "Assets/Dialogue.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }


    public static Dialogue CreateDialogue(int num)
    {
        Dialogue asset = ScriptableObject.CreateInstance<Dialogue>();

        AssetDatabase.CreateAsset(asset, "Assets/Dialogue" + num + ".asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}