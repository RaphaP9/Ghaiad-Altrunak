using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreatSO", menuName = "ScriptableObjects/Inventory/Treat")]
public class TreatSO : InventoryObjectSO
{
    public override InventoryObjectType GetInventoryObjectType() => InventoryObjectType.Treat;

    public override bool CanPurchaseDueToInventory()
    {
        if (TreatsInventoryManager.Instance == null) return false;
        if (TreatsInventoryManager.Instance.IsInventoryFull()) return false;

        return true;
    }
}
