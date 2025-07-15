using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundStackingDamagePerGoldTreatEffectHandler : RoundStackingTreatEffectHandler
{
    public static RoundStackingDamagePerGoldTreatEffectHandler Instance { get; private set; }
    private RoundStackingDamagePerGoldTreatEffectSO RoundStackingDamagePerGoldTreatEffectSO => treatEffectSO as RoundStackingDamagePerGoldTreatEffectSO;

    protected override void OnEnable()
    {
        base.OnEnable();
        GoldCollection.OnAnyGoldCollected += GoldCollection_OnAnyGoldCollected;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GoldCollection.OnAnyGoldCollected -= GoldCollection_OnAnyGoldCollected;
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

    protected override string GetRefferencialGUID() => RoundStackingDamagePerGoldTreatEffectSO.refferencialGUID;

    private void HandleStacking(int quantity)
    {
        bool probability = MechanicsUtilities.GetProbability(RoundStackingDamagePerGoldTreatEffectSO.stackProbability);

        if (!probability) return;

        AddStacks(quantity);
    }

    protected override void AddStacks(int quantity)
    {
        base.AddStacks(quantity);
        AddProportionalStatForStacks(RoundStackingDamagePerGoldTreatEffectSO.statPerStack);
    }

    #region Subscriptions
    private void GoldCollection_OnAnyGoldCollected(object sender, GoldCollection.OnGoldEventArgs e)
    {
        if (!isCurrentlyActiveByInventoryObjects) return;
        if (!isMeetingCondition) return;
        if (!onRound) return;
        HandleStacking(1);
    }
    #endregion
}
