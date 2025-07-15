using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CritAttackHealTreatEffectHandler : TreatEffectHandler
{
    public static CritAttackHealTreatEffectHandler Instance { get; private set; }

    private CritAttackHealTreatEffectSO CritAttackHealTreatEffectSO => treatEffectSO as CritAttackHealTreatEffectSO;

    private PlayerHealth playerHealth;

    public static event EventHandler OnHealByTreat;

    private void OnEnable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation += PlayerInstantiationHandler_OnPlayerInstantiation;
        EnemyHealth.OnAnyEnemyHealthTakeDamage += EnemyHealth_OnAnyEnemyHealthTakeDamage;
    }

    private void OnDisable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation -= PlayerInstantiationHandler_OnPlayerInstantiation;
        EnemyHealth.OnAnyEnemyHealthTakeDamage -= EnemyHealth_OnAnyEnemyHealthTakeDamage;
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

    private void HandleHeal()
    {
        bool probability = MechanicsUtilities.GetProbability(CritAttackHealTreatEffectSO.healProbability);

        if (!probability) return;

        HealData healData = new HealData(CritAttackHealTreatEffectSO.healPerCrit, CritAttackHealTreatEffectSO);
        playerHealth.Heal(healData);

        OnHealByTreat?.Invoke(this, EventArgs.Empty);
    }

    #region Subscriptions
    private void PlayerInstantiationHandler_OnPlayerInstantiation(object sender, PlayerInstantiationHandler.OnPlayerInstantiationEventArgs e)
    {
        playerHealth = e.playerTransform.GetComponentInChildren<PlayerHealth>();
    }

    private void EnemyHealth_OnAnyEnemyHealthTakeDamage(object sender, EntityHealth.OnEntityHealthTakeDamageEventArgs e)
    {
        if (!isCurrentlyActiveByInventoryObjects) return;
        if (!isMeetingCondition) return;
        if (!e.isCrit) return;
        if (e.damageSource.GetDamageSourceClassification() != DamageSourceClassification.Character) return;

        HandleHeal();
    }
    #endregion
}
