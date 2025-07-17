using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private ShopSettingsSO shopSettingsSO;

    [Header("Settings")]
    [SerializeField] private bool resetRerollCostEachShopOpen;

    [Header("RuntimeFilled")]
    [SerializeField] private bool isShopLocked;
    [SerializeField] private int currentRerollCost;
    [SerializeField] private int rerollTimes;

    public bool IsShopLocked => isShopLocked;
    public int CurrentRerollCost => currentRerollCost;
    public int RerollTimes => rerollTimes;

    public static event EventHandler<OnShopItemsEventArgs> OnShopItemsGenerated;
    public static event EventHandler<OnRerollCostEventArgs> OnRerollCostSet;

    public static event EventHandler<OnRerollCostEventArgs> OnReroll;
    public static event EventHandler<OnRerollCostEventArgs> OnRerollDenied;

    public class OnShopItemsEventArgs : EventArgs
    {
        public List<InventoryObjectSO> inventoryObjectSOs;
    }

    public class OnRerollCostEventArgs : EventArgs
    {
        public int rerollCost;
    }

    private void OnEnable()
    {
        ShopOpeningManager.OnShopOpen += ShopOpeningManager_OnShopOpen;

        ObjectsShopUIHandler.OnRerollClick += ShopUIHandler_OnRerollClick;
        ObjectsShopUIHandler.OnLockShopToggled += ShopUIHandler_OnLockShopToggled;
    }

    private void OnDisable()
    {
        ShopOpeningManager.OnShopOpen -= ShopOpeningManager_OnShopOpen;

        ObjectsShopUIHandler.OnRerollClick -= ShopUIHandler_OnRerollClick;
        ObjectsShopUIHandler.OnLockShopToggled -= ShopUIHandler_OnLockShopToggled;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeVariables();
        ResetRerollCost();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one ShopManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeVariables()
    {
        isShopLocked = false;
    }

    private void GenerateNewShopItems()
    {
        List<InventoryObjectSO> newGeneratedItems = ShopGenerator.Instance.GenerateShopObjectsList(1);
        OnShopItemsGenerated?.Invoke(this, new OnShopItemsEventArgs { inventoryObjectSOs = newGeneratedItems });
    }

    private void HandleReroll()
    {
        if (!GoldManager.Instance.CanSpendGold(currentRerollCost))
        {
            DenyReroll();
            return;
        }

        Reroll();
    }

    private void Reroll()
    {
        GoldManager.Instance.SpendGold(currentRerollCost);

        OnReroll?.Invoke(this, new OnRerollCostEventArgs { rerollCost = currentRerollCost });

        IncreaseRerollTimes(1);
        SetRerollCostDueToRerollTimes();

        GenerateNewShopItems();
    }

    private void DenyReroll()
    {
        OnRerollDenied?.Invoke(this, new OnRerollCostEventArgs { rerollCost = currentRerollCost });
    }

    private void ResetRerollCost()
    {
        SetRerollTimes(0);
        SetRerollCostDueToRerollTimes();
    }

    private void SetRerollCostDueToRerollTimes()
    {
        int newRerollCost = CalculateRerollCost(rerollTimes);
        SetRerollCost(newRerollCost);
        OnRerollCostSet?.Invoke(this, new OnRerollCostEventArgs { rerollCost = newRerollCost });
    }


    private void SetRerollTimes(int times) => rerollTimes = times;
    private void IncreaseRerollTimes(int quantity) => rerollTimes += quantity; 
    private int CalculateRerollCost(int rerollTimes) => shopSettingsSO.rerollBaseCost + rerollTimes* shopSettingsSO.rerollCostIncreasePerReroll;  
    private void SetRerollCost(int cost) => currentRerollCost = cost;

    private void SetIsShopLocked(bool isShopLocked) => this.isShopLocked = isShopLocked;


    #region Subscriptions

    private void ShopOpeningManager_OnShopOpen(object sender, System.EventArgs e)
    {
        if (resetRerollCostEachShopOpen) ResetRerollCost();
        if (!isShopLocked) GenerateNewShopItems();
    }

    private void ShopUIHandler_OnLockShopToggled(object sender, ObjectsShopUIHandler.OnLockShopToggledEventArgs e)
    {
        SetIsShopLocked(e.isOn);
    }

    private void ShopUIHandler_OnRerollClick(object sender, System.EventArgs e)
    {
        HandleReroll();        
    }
    #endregion
}
