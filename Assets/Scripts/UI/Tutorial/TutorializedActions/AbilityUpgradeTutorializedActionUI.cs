using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUpgradeTutorializedActionUI : TutorializedActionUI
{
    private bool eventConditionMet = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        AbilityUpgradeOpeningManager.OnAbilityUpgradeClose += AbilityUpgradeOpeningManager_OnAbilityUpgradeClose;
        AbilityUpgradeOpeningManager.OnAbilityUpgradeCloseImmediately += AbilityUpgradeOpeningManager_OnAbilityUpgradeCloseImmediately;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        AbilityUpgradeOpeningManager.OnAbilityUpgradeClose -= AbilityUpgradeOpeningManager_OnAbilityUpgradeClose;
        AbilityUpgradeOpeningManager.OnAbilityUpgradeCloseImmediately -= AbilityUpgradeOpeningManager_OnAbilityUpgradeCloseImmediately;
    }

    #region Virtual Methods
    public override TutorializedAction GetTutorializedAction() => TutorializedAction.AbilityUpgrade;

    protected override bool CheckCondition()
    {
        if (!isDetectingCondition) return false;
        return eventConditionMet;
    }
    #endregion

    #region Subscriptions
    private void AbilityUpgradeOpeningManager_OnAbilityUpgradeClose(object sender, EventArgs e)
    {
        eventConditionMet = true;
    }

    private void AbilityUpgradeOpeningManager_OnAbilityUpgradeCloseImmediately(object sender, EventArgs e)
    {
        eventConditionMet = true;
    }
    #endregion
}
