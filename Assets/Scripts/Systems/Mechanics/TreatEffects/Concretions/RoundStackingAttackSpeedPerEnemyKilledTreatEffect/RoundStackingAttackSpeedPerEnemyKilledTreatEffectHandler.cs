using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundStackingAttackSpeedPerEnemyKilledTreatEffectHandler : RoundStackingTreatEffectHandler
{
    public static RoundStackingAttackSpeedPerEnemyKilledTreatEffectHandler Instance { get; private set; }

    private RoundStackingAttackSpeedPerEnemyKilledTreatEffectSO RoundStackingAttackSpeedPerEnemyKilledTreatEffectSO => treatEffectSO as RoundStackingAttackSpeedPerEnemyKilledTreatEffectSO;

    protected override void OnEnable()
    {
        base.OnEnable();
        EnemyHealth.OnAnyEnemyDeath += EnemyHealth_OnAnyEnemyDeath;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EnemyHealth.OnAnyEnemyDeath -= EnemyHealth_OnAnyEnemyDeath;
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

    protected override string GetRefferencialGUID() => RoundStackingAttackSpeedPerEnemyKilledTreatEffectSO.refferencialGUID;

    protected override void AddStacks(int quantity)
    {
        base.AddStacks(quantity);
        AddProportionalStatForStacks(RoundStackingAttackSpeedPerEnemyKilledTreatEffectSO.statPerStack);
    }

    #region Subscriptions
    private void EnemyHealth_OnAnyEnemyDeath(object sender, EntityHealth.OnEntityDeathEventArgs e)
    {
        if (!isCurrentlyActiveByInventoryObjects) return;
        if (!isMeetingCondition) return;
        if (!onRound) return;
        if (e.damageSource.GetDamageSourceClassification() != DamageSourceClassification.Character) return;
        AddStacks(1);
    }
    #endregion
}