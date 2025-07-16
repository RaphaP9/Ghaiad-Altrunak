using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteTreatEffectHandler : TreatEffectHandler
{
    public static ExecuteTreatEffectHandler Instance { get; private set; }

    private ExecuteTreatEffectSO ExecuteTreatConfigSO => treatEffectSO as ExecuteTreatEffectSO;

    public static event EventHandler<OnExecuteTreatExecutionEventArgs> OnExecuteTreatExecution; 

    public class OnExecuteTreatExecutionEventArgs : EventArgs
    {
        public EnemyHealth enemyHealth;
        public int healthEnemyHadToExecute;
    }

    private void OnEnable()
    {
        EnemyHealth.OnAnyEnemyHealthTakeDamage += EnemyHealth_OnAnyEnemyHealthTakeDamage;
    }

    private void OnDisable()
    {
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

    private void HandleEnemyExecution(object sender, EntityHealth.OnEntityHealthTakeDamageEventArgs e)
    {
        EnemyHealth enemyHealth = sender as EnemyHealth;

        if (!enemyHealth.IsAlive()) return; //Do not execute if already dead
        if (e.newHealth > ExecuteTreatConfigSO.healthExecuteThreshold) return; //Do not execute if above threshold
        if (e.damageSource.GetDamageSourceClassification() != DamageSourceClassification.Character) return; //Must be damaged by character

        ExecuteDamageData executeDamageData = new ExecuteDamageData(true, ExecuteTreatConfigSO, true);

        enemyHealth.Execute(executeDamageData);
        OnExecuteTreatExecution?.Invoke(this, new OnExecuteTreatExecutionEventArgs { enemyHealth = enemyHealth, healthEnemyHadToExecute = e.newHealth });
    }

    #region Subscriptions
    private void EnemyHealth_OnAnyEnemyHealthTakeDamage(object sender, EntityHealth.OnEntityHealthTakeDamageEventArgs e)
    {
        if (!isCurrentlyActiveByInventoryObjects) return;
        if (!isMeetingCondition) return; //In this treat condition is always true

        HandleEnemyExecution(sender , e);
    }
    #endregion
}
