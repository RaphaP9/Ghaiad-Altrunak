using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShopOpeningManager : MonoBehaviour
{
    public static ShopOpeningManager Instance { get; private set; }

    [Header("Runtime Filled")]
    [SerializeField] private bool isOpen;

    public static event EventHandler OnShopOpen;
    public static event EventHandler OnShopClose;

    public static event EventHandler OnShopOpenInmediately;
    public static event EventHandler OnShopCloseImmediately;

    public static event EventHandler OnShopClosedFromUI;

    public bool IsOpen => isOpen;

    private void OnEnable()
    {
        ShopUI.OnShopClosedFromUI += ShopUI_OnShopClosedFromUI;
    }

    private void OnDisable()
    {
        ShopUI.OnShopClosedFromUI -= ShopUI_OnShopClosedFromUI;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one ShopOpeningManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void OpenShopInmediately()
    {
        if (isOpen) return;

        OnShopOpenInmediately?.Invoke(this, EventArgs.Empty);
        SetIsOpen(true);
    }

    public void CloseShopInmediately()
    {
        if (!isOpen) return;

        OnShopCloseImmediately?.Invoke(this, EventArgs.Empty);
        SetIsOpen(false);
    }

    public void OpenShop()
    {
        if (isOpen) return;

        OnShopOpen?.Invoke(this, EventArgs.Empty);
        SetIsOpen(true);
    }

    public void CloseShop()
    {
        if (!isOpen) return;

        OnShopClose?.Invoke(this, EventArgs.Empty);
        SetIsOpen(false);
    }


    public void CloseShopFromUI()
    {
        if (!isOpen) return;

        CloseShop();
        OnShopClosedFromUI?.Invoke(this, EventArgs.Empty);
    }

    private bool SetIsOpen(bool isOpen) => this.isOpen = isOpen;

    #region Subscriptions
    private void ShopUI_OnShopClosedFromUI(object sender, EventArgs e)
    {
        CloseShopFromUI();
    }
    #endregion
}
