using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldDropEnemiesInArenaTreatEffectHandler : TreatEffectHandler
{
    public static GoldDropEnemiesInArenaTreatEffectHandler Instance { get; private set; }

    private GoldDropEnemiesInArenaTreatEffectSO GoldDropEnemiesInArenaTreatEffectSO => treatEffectSO as GoldDropEnemiesInArenaTreatEffectSO;

    public static event EventHandler OnGoldEnemiesArenaTreatEffectEnabled;


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

    #region VirtualMethods
    protected override void OnTreatEffectEnablementByConditionMethod()
    {
        base.OnTreatEffectEnablementByConditionMethod();

        OnGoldEnemiesArenaTreatEffectEnabled?.Invoke(this, EventArgs.Empty);
        TemporalNumericStatModifierManager.Instance.AddStatModifiers(GoldDropEnemiesInArenaTreatEffectSO.refferencialGUID, GoldDropEnemiesInArenaTreatEffectSO);
    }

    protected override void OnTreatEffectDisablementByConditionMethod()
    {
        base.OnTreatEffectDisablementByConditionMethod();
        TemporalNumericStatModifierManager.Instance.RemoveStatModifiersByGUID(GoldDropEnemiesInArenaTreatEffectSO.refferencialGUID);
    }

    protected override bool EnablementCondition() => EnemiesManager.Instance.GetActiveEnemiesCount() >= GoldDropEnemiesInArenaTreatEffectSO.enemiesInArenaThreshold;
    #endregion
}
