using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDamageLowHealthTreatEffectHandler : TreatEffectHandler
{
    public static BonusDamageLowHealthTreatEffectHandler Instance { get; private set; }

    private BonusDamageLowHealthTreatEffectSO BonusDamageLowHealthTreatEffectSO => treatEffectSO as BonusDamageLowHealthTreatEffectSO;

    private PlayerHealth playerHealth;

    public static event EventHandler OnBonusDamageLowHealthTreatEffectEnabled;

    private void OnEnable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation += PlayerInstantiationHandler_OnPlayerInstantiation;
    }
    private void OnDisable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation -= PlayerInstantiationHandler_OnPlayerInstantiation;
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

    #region VirtualMethods
    protected override void OnTreatEffectEnablementByConditionMethod()
    {
        base.OnTreatEffectEnablementByConditionMethod();

        OnBonusDamageLowHealthTreatEffectEnabled?.Invoke(this, EventArgs.Empty);    
        TemporalNumericStatModifierManager.Instance.AddStatModifiers(BonusDamageLowHealthTreatEffectSO.refferencialGUID, BonusDamageLowHealthTreatEffectSO);
    }

    protected override void OnTreatEffectDisablementByConditionMethod() 
    {
        base.OnTreatEffectDisablementByConditionMethod();
        TemporalNumericStatModifierManager.Instance.RemoveStatModifiersByGUID(BonusDamageLowHealthTreatEffectSO.refferencialGUID);
    }

    protected override bool EnablementCondition() => playerHealth.CurrentHealth <= BonusDamageLowHealthTreatEffectSO.healthThreshold;
    #endregion

    #region Subscriptions
    private void PlayerInstantiationHandler_OnPlayerInstantiation(object sender, PlayerInstantiationHandler.OnPlayerInstantiationEventArgs e)
    {
        playerHealth = e.playerTransform.GetComponentInChildren<PlayerHealth>();
    }
    #endregion
}
