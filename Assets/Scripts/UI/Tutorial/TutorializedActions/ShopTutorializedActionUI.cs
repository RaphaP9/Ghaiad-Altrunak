using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTutorializedActionUI : TutorializedActionUI
{
    private bool eventConditionMet = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        ShopOpeningManager.OnShopClose += ShopOpeningManager_OnShopClose;
        ShopOpeningManager.OnShopCloseImmediately += ShopOpeningManager_OnShopCloseImmediately;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ShopOpeningManager.OnShopClose -= ShopOpeningManager_OnShopClose;
        ShopOpeningManager.OnShopCloseImmediately -= ShopOpeningManager_OnShopCloseImmediately;
    }

    public override TutorializedAction GetTutorializedAction() => TutorializedAction.Shop;

    #region Virtual Methods
    protected override bool CheckCondition()
    {
        if (!isDetectingCondition) return false;
        return eventConditionMet;
    }
    #endregion

    #region Subscriptions
    private void ShopOpeningManager_OnShopClose(object sender, EventArgs e)
    {
        eventConditionMet = true;
    }

    private void ShopOpeningManager_OnShopCloseImmediately(object sender, EventArgs e)
    {
        eventConditionMet = true;
    }
    #endregion
}
