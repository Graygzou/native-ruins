using UnityEngine;
using System.Collections;

[System.Serializable]                                                           //  Our Representation of an InventoryItem
public class InventoryItem : ScriptableObject
{
    public string itemName = "New Item";                                      //  What the item will be called in the inventory
    public Texture2D itemIcon = null;                                           //  What the item will look like in the inventory
    public Rigidbody itemObject = null;                                         //  Optional slot for a PreFab to instantiate when discarding
    public bool isUnique = false;                                               //  Optional checkbox to indicate that there should only be one of these items per game
    public bool isIndestructible = false;                                       //  Optional checkbox to prevent an item from being destroyed by the player (unimplemented)
    public bool isQuestItem = false;                                            //  Examples of additional information that could be held in InventoryItem
    public bool isStackable = false;                                            //  Examples of additional information that could be held in InventoryItem
    public bool destroyOnUse = false;                                           //  Examples of additional information that could be held in InventoryItem
    public float encumbranceValue = 0;                                          //  Examples of additional information that could be held in InventoryItem  !!!
}