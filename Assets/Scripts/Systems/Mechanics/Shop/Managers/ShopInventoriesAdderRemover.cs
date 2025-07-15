using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInventoriesAdderRemover : MonoBehaviour
{
    private void OnEnable()
    {
        ShopObjectCardPurchaseHandler.OnAnyShopObjectPurchase += ShopObjectCardPurchaseHandler_OnAnyShopObjectPurchase;
    }

    private void OnDisable()
    {
        ShopObjectCardPurchaseHandler.OnAnyShopObjectPurchase -= ShopObjectCardPurchaseHandler_OnAnyShopObjectPurchase;
    }

    private void HandleShopObjectInventorAdd(InventoryObjectSO inventoryObjectSO)
    {
        switch (inventoryObjectSO.GetInventoryObjectType())
        {
            case InventoryObjectType.Object:
                AddObjectToInventory(inventoryObjectSO as ObjectSO);
                break;
            case InventoryObjectType.Treat:
                AddTreatToInventory(inventoryObjectSO as TreatSO);
                break;
        }
    }

    private void AddObjectToInventory(ObjectSO objectSO) => ObjectsInventoryManager.Instance.AddObjectToInventory(objectSO);
    private void AddTreatToInventory(TreatSO treatSO) => TreatsInventoryManager.Instance.AddTreatToInventory(treatSO);

    private void ShopObjectCardPurchaseHandler_OnAnyShopObjectPurchase(object sender, ShopObjectCardPurchaseHandler.OnShopObjectPurchaseEventArgs e)
    {
        HandleShopObjectInventorAdd(e.inventoryObjectSO);
    }
}
