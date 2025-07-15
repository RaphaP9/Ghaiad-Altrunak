using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.U2D;

public class ObjectsShopUIHandler : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Transform shopInventoryObjectCardsContainer;
    [Space]
    [SerializeField] private Transform commonCardPrefab;
    [SerializeField] private Transform uncommonCardPrefab;
    [SerializeField] private Transform rareCardPrefab;
    [SerializeField] private Transform epicCardPrefab;
    [SerializeField] private Transform legendaryCardPrefab;
    [Space]
    [SerializeField] private Button rerollButton;
    [SerializeField] private Toggle lockShopToggle;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public static event EventHandler OnRerollClick;
    public static event EventHandler<OnLockShopToggledEventArgs> OnLockShopToggled;

    public class OnLockShopToggledEventArgs : EventArgs
    {
        public bool isOn;
    }

    private void OnEnable()
    {
        ShopManager.OnShopItemsGenerated += ShopManager_OnNewShopItemsGenerated;
        ShopManager.OnRerollCostSet += ShopManager_OnRerollCostSet;
    }

    private void OnDisable()
    {
        ShopManager.OnShopItemsGenerated -= ShopManager_OnNewShopItemsGenerated;
        ShopManager.OnRerollCostSet -= ShopManager_OnRerollCostSet;
    }

    private void Awake()
    {
        InitializeLockShopToggle();
        InitializeListeners();
    }

    private void InitializeLockShopToggle()
    {
        lockShopToggle.isOn = false;
    }

    private void InitializeListeners()
    {
        rerollButton.onClick.AddListener(Reroll);
        lockShopToggle.onValueChanged.AddListener(ToggleShopLock);
    }

    private void Reroll()
    {
        OnRerollClick?.Invoke(this, EventArgs.Empty);
    }

    private void ToggleShopLock(bool isOn)
    {
        OnLockShopToggled?.Invoke(this, new OnLockShopToggledEventArgs{isOn = isOn});
    }

    private void GenerateNewShopItems(List<InventoryObjectSO> inventoryObjectSOs)
    {
        ClearShopInventoryObjectsContainer();

        foreach (InventoryObjectSO inventoryObjectSO in inventoryObjectSOs)
        {
            CreateInventoryObjectShopItem(inventoryObjectSO);
        }
    }

    private void ClearShopInventoryObjectsContainer()
    {
        foreach (Transform child in shopInventoryObjectCardsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateInventoryObjectShopItem(InventoryObjectSO inventoryObjectSO)
    {
        Transform cardPrefab = ChooseCardPrefabByRarity(inventoryObjectSO);
        Transform shopInventoryObjectCard = Instantiate(cardPrefab, shopInventoryObjectCardsContainer);

        ShopObjectCardUI shopObjectCardUI = shopInventoryObjectCard.GetComponent<ShopObjectCardUI>();

        if(shopObjectCardUI == null)
        {
            if (debug) Debug.Log("Instantiated Shop Object Card does not contain a ShopObjectCardUI component. Set will be ignored.");
            return;
        }

        shopObjectCardUI.SetInventoryObject(inventoryObjectSO);
    }

    private Transform ChooseCardPrefabByRarity(InventoryObjectSO inventoryObjectSO)
    {
        switch (inventoryObjectSO.objectRarity)
        {
            case Rarity.Common:
            default:
                return commonCardPrefab;
            case Rarity.Uncommon:
                return uncommonCardPrefab;
            case Rarity.Rare:
                return rareCardPrefab;
            case Rarity.Epic:
                return epicCardPrefab;
            case Rarity.Legendary:
                return legendaryCardPrefab;
        }
    }

    #region Subscriptions
    private void ShopManager_OnRerollCostSet(object sender, ShopManager.OnRerollCostEventArgs e)
    {
        //Debug.Log($"Reroll cost: {e.rerollCost}");
    }

    private void ShopManager_OnNewShopItemsGenerated(object sender, ShopManager.OnShopItemsEventArgs e)
    {
        GenerateNewShopItems(e.inventoryObjectSOs);
    }
    #endregion
}
