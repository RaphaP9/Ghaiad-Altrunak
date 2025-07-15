using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUpgradeCardUI : MonoBehaviour
{
    [Header("Runtime Filled")]
    [SerializeField] private AbilityUpgradeCardInfo abilityUpgradeCardInfo;
    [SerializeField] private AbilityLevelInfo abilityLevelInfo;

    public AbilityUpgradeCardInfo AbilityUpgradeCardInfo => abilityUpgradeCardInfo;

    public event EventHandler<OnAbilityUpgradeCardEventArgs> OnAbilityUpgradeCardSet;

    public class OnAbilityUpgradeCardEventArgs : EventArgs
    {
        public AbilityUpgradeCardInfo abilityUpgradeCardInfo;
        public AbilityLevelInfo abilityLevelInfo;
    }

    public void SetAbilityUpgradeCardInfo(AbilityUpgradeCardInfo abilityUpgradeCardInfo, AbilityLevelInfo abilityLevelInfo)
    {
        this.abilityUpgradeCardInfo = abilityUpgradeCardInfo;
        this.abilityLevelInfo = abilityLevelInfo;

        OnAbilityUpgradeCardSet?.Invoke(this, new OnAbilityUpgradeCardEventArgs { abilityUpgradeCardInfo = abilityUpgradeCardInfo, abilityLevelInfo = abilityLevelInfo });
    }
}
