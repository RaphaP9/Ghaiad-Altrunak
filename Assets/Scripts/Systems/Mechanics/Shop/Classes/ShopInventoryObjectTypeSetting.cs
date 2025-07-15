using UnityEngine;

[System.Serializable]
public class ShopInventoryObjectTypeSetting
{
    public InventoryObjectType inventoryObjectType;
    [Range(0, 100)] public int weight;
    [Range(0, 5)] public int cap;
}