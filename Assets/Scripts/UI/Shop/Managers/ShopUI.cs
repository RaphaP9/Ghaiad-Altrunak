using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("UI Components")]
    [SerializeField] private Button closeButton;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private const string SHOWING_ANIMATION_NAME = "Showing";
    private const string HIDDEN_ANIMATION_NAME = "Hidden";

    public static event EventHandler OnShopClosedFromUI;

    private void OnEnable()
    {
        ShopOpeningManager.OnShopOpen += ShopUIOpeningManager_OnShopUIOpen;
        ShopOpeningManager.OnShopClose += ShopUIOpeningManager_OnShopUIClose;

        ShopOpeningManager.OnShopOpenInmediately += ShopUIOpeningManager_OnShopUIOpenInmediately;
        ShopOpeningManager.OnShopCloseImmediately += ShopUIOpeningManager_OnShopUICloseInmediately;
    }

    private void OnDisable()
    {
        ShopOpeningManager.OnShopOpen -= ShopUIOpeningManager_OnShopUIOpen;
        ShopOpeningManager.OnShopClose -= ShopUIOpeningManager_OnShopUIClose;

        ShopOpeningManager.OnShopOpenInmediately -= ShopUIOpeningManager_OnShopUIOpenInmediately;
        ShopOpeningManager.OnShopCloseImmediately -= ShopUIOpeningManager_OnShopUICloseInmediately;
    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        closeButton.onClick.AddListener(CloseShop);
    }

    private void CloseShop()
    {
        OnShopClosedFromUI?.Invoke(this, EventArgs.Empty);
    }

    #region Show&Hide
    private void ShowUI()
    {
        animator.ResetTrigger(HIDE_TRIGGER);
        animator.SetTrigger(SHOW_TRIGGER);
    }

    private void HideUI()
    {
        animator.ResetTrigger(SHOW_TRIGGER);
        animator.SetTrigger(HIDE_TRIGGER);
    }

    private void ShowUIImmediately()
    {
        animator.ResetTrigger(SHOW_TRIGGER);
        animator.ResetTrigger(HIDE_TRIGGER);

        animator.Play(SHOWING_ANIMATION_NAME);
    }
    private void HideUIImmediately()
    {
        animator.ResetTrigger(SHOW_TRIGGER);
        animator.ResetTrigger(HIDE_TRIGGER);

        animator.Play(HIDDEN_ANIMATION_NAME);
    }

    #endregion

    private void ShopUIOpeningManager_OnShopUIOpen(object sender, System.EventArgs e)
    {
        ShowUI();
    }

    private void ShopUIOpeningManager_OnShopUIClose(object sender, System.EventArgs e)
    {
        HideUI();
    }

    private void ShopUIOpeningManager_OnShopUIOpenInmediately(object sender, System.EventArgs e)
    {
        ShowUIImmediately();
    }

    private void ShopUIOpeningManager_OnShopUICloseInmediately(object sender, System.EventArgs e)
    {
        HideUIImmediately();
    }
}
