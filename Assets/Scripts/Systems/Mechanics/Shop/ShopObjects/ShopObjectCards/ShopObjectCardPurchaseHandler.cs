using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopObjectCardPurchaseHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ShopObjectCardUI shopObjectCardUI;
    [SerializeField] private Button purchaseButton;

    public static event EventHandler<OnShopObjectPurchaseEventArgs> OnAnyShopObjectPurchase;
    public static event EventHandler<OnShopObjectPurchaseEventArgs> OnAnyShopObjectPurchaseDeniedByGold;
    public static event EventHandler<OnShopObjectPurchaseEventArgs> OnAnyShopObjectPurchaseDeniedByInventory;

    public event EventHandler<OnShopObjectPurchaseEventArgs> OnShopObjectPurchase;
    public event EventHandler<OnShopObjectPurchaseEventArgs> OnShopObjectPurchaseDeniedByGold;
    public event EventHandler<OnShopObjectPurchaseEventArgs> OnShopObjectPurchaseDeniedByInventory;

    public class OnShopObjectPurchaseEventArgs : EventArgs
    {
        public InventoryObjectSO inventoryObjectSO;
    }

    private void OnEnable()
    {
        shopObjectCardUI.OnInventoryObjectSet += ShopObjectCardUI_OnInventoryObjectSet;
    }

    private void OnDisable()
    {
        shopObjectCardUI.OnInventoryObjectSet -= ShopObjectCardUI_OnInventoryObjectSet;
    }

    private void HandlePurchase(InventoryObjectSO inventoryObjectSO)
    {
        if (!inventoryObjectSO.CanPurchaseDueToInventory())
        {
            DenyPurchaseByInventory(inventoryObjectSO);
            return;
        }

        if (!inventoryObjectSO.CanPurchaseDueToGold())
        {
            DenyPurchaseByGold(inventoryObjectSO);
            return;
        }


        Purchase(inventoryObjectSO);
    }

    private void Purchase(InventoryObjectSO inventoryObjectSO)
    {
        DisablePurchaseButton();

        GoldManager.Instance.SpendGold(inventoryObjectSO.price);

        OnAnyShopObjectPurchase?.Invoke(this, new OnShopObjectPurchaseEventArgs { inventoryObjectSO = inventoryObjectSO });
        OnShopObjectPurchase?.Invoke(this, new OnShopObjectPurchaseEventArgs { inventoryObjectSO = inventoryObjectSO });
    }

    private void DenyPurchaseByInventory(InventoryObjectSO inventoryObjectSO)
    {
        OnAnyShopObjectPurchaseDeniedByInventory?.Invoke(this, new OnShopObjectPurchaseEventArgs { inventoryObjectSO = inventoryObjectSO });
        OnShopObjectPurchaseDeniedByInventory?.Invoke(this, new OnShopObjectPurchaseEventArgs { inventoryObjectSO = inventoryObjectSO });
    }

    private void DenyPurchaseByGold(InventoryObjectSO inventoryObjectSO)
    {
        OnAnyShopObjectPurchaseDeniedByGold?.Invoke(this, new OnShopObjectPurchaseEventArgs { inventoryObjectSO = inventoryObjectSO });
        OnShopObjectPurchaseDeniedByGold?.Invoke(this, new OnShopObjectPurchaseEventArgs { inventoryObjectSO = inventoryObjectSO });
    }

    private void UpdateButtonListener(InventoryObjectSO inventoryObjectSO)
    {
        purchaseButton.onClick.RemoveAllListeners();
        purchaseButton.onClick.AddListener(() => HandlePurchase(inventoryObjectSO));
    }

    private void DisablePurchaseButton() => purchaseButton.enabled = false;

    #region Subscriptions
    private void ShopObjectCardUI_OnInventoryObjectSet(object sender, ShopObjectCardUI.OnInventoryObjectEventArgs e)
    {
        UpdateButtonListener(e.inventoryObjectSO);
    }
    #endregion
}
