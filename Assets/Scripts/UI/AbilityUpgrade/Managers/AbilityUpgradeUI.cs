using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AbilityUpgradeUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("UI Components")]
    [SerializeField] private Button closeButton;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private const string SHOWING_ANIMATION_NAME = "Showing";
    private const string HIDDEN_ANIMATION_NAME = "Hidden";

    public static event EventHandler OnAbilityUpgradeClosedFromUI;

    private void OnEnable()
    {
        AbilityUpgradeOpeningManager.OnAbilityUpgradeOpen += AbilityUpgradeUIOpeningManager_OnAbilityUpgradeUIOpen;
        AbilityUpgradeOpeningManager.OnAbilityUpgradeClose += AbilityUpgradeUIOpeningManager_OnAbilityUpgradeUIClose;

        AbilityUpgradeOpeningManager.OnAbilityUpgradeOpenInmediately += AbilityUpgradeUIOpeningManager_OnAbilityUpgradeUIOpenInmediately;
        AbilityUpgradeOpeningManager.OnAbilityUpgradeCloseImmediately += AbilityUpgradeUIOpeningManager_OnAbilityUpgradeUICloseInmediately;
    }

    private void OnDisable()
    {
        AbilityUpgradeOpeningManager.OnAbilityUpgradeOpen -= AbilityUpgradeUIOpeningManager_OnAbilityUpgradeUIOpen;
        AbilityUpgradeOpeningManager.OnAbilityUpgradeClose -= AbilityUpgradeUIOpeningManager_OnAbilityUpgradeUIClose;

        AbilityUpgradeOpeningManager.OnAbilityUpgradeOpenInmediately -= AbilityUpgradeUIOpeningManager_OnAbilityUpgradeUIOpenInmediately;
        AbilityUpgradeOpeningManager.OnAbilityUpgradeCloseImmediately -= AbilityUpgradeUIOpeningManager_OnAbilityUpgradeUICloseInmediately;
    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        closeButton.onClick.AddListener(CloseAbilityUpgrade);
    }

    private void CloseAbilityUpgrade()
    {
        OnAbilityUpgradeClosedFromUI?.Invoke(this, EventArgs.Empty);
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

    private void AbilityUpgradeUIOpeningManager_OnAbilityUpgradeUIOpen(object sender, System.EventArgs e)
    {
        ShowUI();
    }

    private void AbilityUpgradeUIOpeningManager_OnAbilityUpgradeUIClose(object sender, System.EventArgs e)
    {
        HideUI();
    }

    private void AbilityUpgradeUIOpeningManager_OnAbilityUpgradeUIOpenInmediately(object sender, System.EventArgs e)
    {
        ShowUIImmediately();
    }

    private void AbilityUpgradeUIOpeningManager_OnAbilityUpgradeUICloseInmediately(object sender, System.EventArgs e)
    {
        HideUIImmediately();
    }
}
