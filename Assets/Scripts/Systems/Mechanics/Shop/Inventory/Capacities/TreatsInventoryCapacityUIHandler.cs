using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TreatsInventoryCapacityUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [Space]
    [SerializeField] private TextMeshProUGUI currentText;
    [SerializeField] private TextMeshProUGUI capacityText;

    private const string DENY_ANIMATION_NAME = "Deny";

    private void OnEnable()
    {
        TreatsInventoryManager.OnTreatsInventoryInitialized += WeaponsInventoryManager_OnWeaponsInventoryInitialized;
        TreatsInventoryManager.OnTreatAddedToInventory += WeaponsInventoryManager_OnWeaponAddedToInventory;
        TreatsInventoryManager.OnTreatRemovedFromInventory += WeaponsInventoryManager_OnWeaponRemovedFromInventory;

        ShopObjectCardPurchaseHandler.OnAnyShopObjectPurchaseDeniedByInventory += ShopObjectCardPurchaseHandler_OnAnyShopObjectPurchaseDeniedByWeaponsInventory;
    }

    private void OnDisable()
    {
        TreatsInventoryManager.OnTreatsInventoryInitialized -= WeaponsInventoryManager_OnWeaponsInventoryInitialized;
        TreatsInventoryManager.OnTreatAddedToInventory -= WeaponsInventoryManager_OnWeaponAddedToInventory;
        TreatsInventoryManager.OnTreatRemovedFromInventory -= WeaponsInventoryManager_OnWeaponRemovedFromInventory;

        ShopObjectCardPurchaseHandler.OnAnyShopObjectPurchaseDeniedByInventory -= ShopObjectCardPurchaseHandler_OnAnyShopObjectPurchaseDeniedByWeaponsInventory;
    }

    private void SetCurrentText(int currentObjects) => currentText.text = currentObjects.ToString();
    private void SetCapacityText(int capacity) => capacityText.text = capacity.ToString();


    #region Subscriptions
    private void WeaponsInventoryManager_OnWeaponsInventoryInitialized(object sender, TreatsInventoryManager.OnTreatsEventArgs e)
    {
        SetCurrentText(TreatsInventoryManager.Instance.GetTreatsInInventory());
        SetCapacityText(TreatsInventoryManager.Instance.GetInventoryCapacity());
    }

    private void WeaponsInventoryManager_OnWeaponAddedToInventory(object sender, TreatsInventoryManager.OnTreatEventArgs e)
    {
        SetCurrentText(TreatsInventoryManager.Instance.GetTreatsInInventory());
    }

    private void WeaponsInventoryManager_OnWeaponRemovedFromInventory(object sender, TreatsInventoryManager.OnTreatEventArgs e)
    {
        SetCurrentText(TreatsInventoryManager.Instance.GetTreatsInInventory());
    }

    private void ShopObjectCardPurchaseHandler_OnAnyShopObjectPurchaseDeniedByWeaponsInventory(object sender, ShopObjectCardPurchaseHandler.OnShopObjectPurchaseEventArgs e)
    {
        if (e.inventoryObjectSO.GetInventoryObjectType() != InventoryObjectType.Treat) return;
        animator.Play(DENY_ANIMATION_NAME);
    }
    #endregion
}
