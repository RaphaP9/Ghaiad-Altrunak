using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundStackingMovementSpeedPerGoldTreatEffectHandler : RoundStackingTreatEffectHandler
{
    public static RoundStackingMovementSpeedPerGoldTreatEffectHandler Instance { get; private set; }
    private RoundStackingMovementSpeedPerGoldTreatEffectSO RoundStackingMovementSpeedPerGoldTreatEffectSO => treatEffectSO as RoundStackingMovementSpeedPerGoldTreatEffectSO;

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

    protected override string GetRefferencialGUID() => RoundStackingMovementSpeedPerGoldTreatEffectSO.refferencialGUID;

    protected override void AddStacks(int quantity)
    {
        base.AddStacks(quantity);
        AddProportionalStatForStacks(RoundStackingMovementSpeedPerGoldTreatEffectSO.statPerStack);
    }


    #region Subscriptions
    private void GoldCollection_OnAnyGoldCollected(object sender, GoldCollection.OnGoldEventArgs e)
    {
        if (!isCurrentlyActiveByInventoryObjects) return;
        if (!isMeetingCondition) return;
        if (!onRound) return;
        AddStacks(1);
    }
    #endregion
}

