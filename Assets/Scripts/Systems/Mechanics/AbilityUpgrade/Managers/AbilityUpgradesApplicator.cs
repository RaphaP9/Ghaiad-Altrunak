using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUpgradesApplicator : MonoBehaviour
{
    private void OnEnable()
    {
        AbilityUpgradeCardPressingHandler.OnAnyAbilityUpgradeCardPressed += AbilityUpgradeCardPressingHandler_OnAnyAbilityUpgradeCardPressed;
    }

    private void OnDisable()
    {
        AbilityUpgradeCardPressingHandler.OnAnyAbilityUpgradeCardPressed -= AbilityUpgradeCardPressingHandler_OnAnyAbilityUpgradeCardPressed;
    }

    private void ApplicateAbilityUpgrade(AbilityUpgradeCardInfo abilityUpgradeCardInfo)
    {
        abilityUpgradeCardInfo.ability.AbilityLevelHandler.SetAbilityLevel(abilityUpgradeCardInfo.upgradeLevel);
    }

    private void AbilityUpgradeCardPressingHandler_OnAnyAbilityUpgradeCardPressed(object sender, AbilityUpgradeCardPressingHandler.OnAbilityUpgradeCardEventArgs e)
    {
        ApplicateAbilityUpgrade(e.abilityUpgradeCardInfo);
    }
}
