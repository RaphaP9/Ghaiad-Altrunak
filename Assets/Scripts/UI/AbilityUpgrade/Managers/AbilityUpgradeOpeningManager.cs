using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbilityUpgradeOpeningManager : MonoBehaviour
{
    public static AbilityUpgradeOpeningManager Instance { get; private set; }

    [Header("Runtime Filled")]
    [SerializeField] private bool isOpen;

    public static event EventHandler OnAbilityUpgradeOpen;
    public static event EventHandler OnAbilityUpgradeClose;

    public static event EventHandler OnAbilityUpgradeOpenInmediately;
    public static event EventHandler OnAbilityUpgradeCloseImmediately;

    public static event EventHandler OnAbilityUpgradeClosedFromUI;

    public bool IsOpen => isOpen;

    private void OnEnable()
    {
        AbilityUpgradeUI.OnAbilityUpgradeClosedFromUI += AbilityUpgradeUI_OnAbilityUpgradeClosedFromUI;
    }

    private void OnDisable()
    {
        AbilityUpgradeUI.OnAbilityUpgradeClosedFromUI -= AbilityUpgradeUI_OnAbilityUpgradeClosedFromUI;
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
            Debug.LogWarning("There is more than one AbilityUpgradeOpeningManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void OpenAbilityUpgradeInmediately()
    {
        if (isOpen) return;

        OnAbilityUpgradeOpenInmediately?.Invoke(this, EventArgs.Empty);
        SetIsOpen(true);
    }

    public void CloseAbilityUpgradeInmediately()
    {
        if (!isOpen) return;

        OnAbilityUpgradeCloseImmediately?.Invoke(this, EventArgs.Empty);
        SetIsOpen(false);
    }

    public void OpenAbilityUpgrade()
    {
        if (isOpen) return;

        OnAbilityUpgradeOpen?.Invoke(this, EventArgs.Empty);
        SetIsOpen(true);
    }

    public void CloseAbilityUpgrade()
    {
        if (!isOpen) return;

        OnAbilityUpgradeClose?.Invoke(this, EventArgs.Empty);
        SetIsOpen(false);
    }


    public void CloseAbilityUpgradeFromUI()
    {
        if (!isOpen) return;

        CloseAbilityUpgrade();
        OnAbilityUpgradeClosedFromUI?.Invoke(this, EventArgs.Empty);
    }

    private bool SetIsOpen(bool isOpen) => this.isOpen = isOpen;

    #region Subscriptions
    private void AbilityUpgradeUI_OnAbilityUpgradeClosedFromUI(object sender, EventArgs e)
    {
        CloseAbilityUpgradeFromUI();
    }
    #endregion
}

