using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUpgradeCardPressingHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AbilityUpgradeCardUI abilityUpgradeCardUI;

    [Header("UI Components")]
    [SerializeField] private Button abilityUpgradeCardButton;

    public event EventHandler<OnAbilityUpgradeCardEventArgs> OnAbilityUpgradeCardPressed;
    public static event EventHandler<OnAbilityUpgradeCardEventArgs> OnAnyAbilityUpgradeCardPressed;


    public class OnAbilityUpgradeCardEventArgs : EventArgs
    {
        public AbilityUpgradeCardInfo abilityUpgradeCardInfo;
    }

    private void OnEnable()
    {
        abilityUpgradeCardUI.OnAbilityUpgradeCardSet += AbilityUpgradeCardUI_OnAbilityUpgradeCardSet;
    }

    private void OnDisable()
    {
        abilityUpgradeCardUI.OnAbilityUpgradeCardSet -= AbilityUpgradeCardUI_OnAbilityUpgradeCardSet;
    }

    private void HandlePressing(AbilityUpgradeCardInfo abilityUpgradeCardInfo)
    {
        OnAbilityUpgradeCardPressed?.Invoke(this, new OnAbilityUpgradeCardEventArgs { abilityUpgradeCardInfo = abilityUpgradeCardInfo });
        OnAnyAbilityUpgradeCardPressed?.Invoke(this, new OnAbilityUpgradeCardEventArgs { abilityUpgradeCardInfo = abilityUpgradeCardInfo });
    }

    private void UpdateButtonListener(AbilityUpgradeCardInfo abilityUpgradeCardInfo)
    {
        abilityUpgradeCardButton.onClick.RemoveAllListeners();
        abilityUpgradeCardButton.onClick.AddListener(() => HandlePressing(abilityUpgradeCardInfo));
    }

    private void AbilityUpgradeCardUI_OnAbilityUpgradeCardSet(object sender, AbilityUpgradeCardUI.OnAbilityUpgradeCardEventArgs e)
    {
        UpdateButtonListener(e.abilityUpgradeCardInfo);
    }
}
