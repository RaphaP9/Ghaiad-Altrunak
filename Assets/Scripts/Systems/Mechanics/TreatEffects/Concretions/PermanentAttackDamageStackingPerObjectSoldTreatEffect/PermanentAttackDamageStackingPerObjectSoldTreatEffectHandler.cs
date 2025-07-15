using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentAttackDamageStackingPerObjectSoldTreatEffectHandler : PermanentStackingTreatEffectHandler
{
    public static PermanentAttackDamageStackingPerObjectSoldTreatEffectHandler Instance { get; private set; }

    private PermanentAttackDamageStackingPerObjectSoldTreatEffectSO PermanentAttackDamageStackingPerObjectSoldSO => treatEffectSO as PermanentAttackDamageStackingPerObjectSoldTreatEffectSO;

    protected void OnEnable()
    {
        ShopSeller.OnObjectSold += ShopSeller_OnObjectSold;
        ShopSeller.OnTreatSold += ShopSeller_OnTreatSold;
    }

    protected void OnDisable()
    {
        ShopSeller.OnObjectSold -= ShopSeller_OnObjectSold;
        ShopSeller.OnTreatSold -= ShopSeller_OnTreatSold;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected override string GetRefferencialGUID() => PermanentAttackDamageStackingPerObjectSoldSO.refferencialGUID;
    protected override NumericEmbeddedStat GetRefferencialNumericEmbeddedStatPerStack() => PermanentAttackDamageStackingPerObjectSoldSO.statPerStack;

    protected override void AddStacks(int quantity)
    {
        base.AddStacks(quantity);
        AddProportionalStatForStacks(PermanentAttackDamageStackingPerObjectSoldSO.statPerStack);
    }

    #region Subscriptions
    private void ShopSeller_OnObjectSold(object sender, ShopSeller.OnObjectSoldEventArgs e)
    {
        if (!isCurrentlyActiveByInventoryObjects) return;
        if (!isMeetingCondition) return;

        AddStacks(1);
    }

    private void ShopSeller_OnTreatSold(object sender, ShopSeller.OnTreatSoldEventArgs e)
    {
        if (!isCurrentlyActiveByInventoryObjects) return;
        if (!isMeetingCondition) return;

        AddStacks(1);
    }
    #endregion
}
