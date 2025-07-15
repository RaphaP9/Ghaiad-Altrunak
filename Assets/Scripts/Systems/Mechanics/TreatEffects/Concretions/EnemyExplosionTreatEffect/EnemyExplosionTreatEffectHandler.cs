using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosionTreatEffectHandler : TreatEffectHandler
{
    public static EnemyExplosionTreatEffectHandler Instance { get; private set; }

    [Header("Specific Settings")]
    [SerializeField] private LayerMask explosionLayerMask;

    private EnemyExplosionTreatEffectSO EnemyExplosionTreatEffectSO => treatEffectSO as EnemyExplosionTreatEffectSO;

    public static event EventHandler<OnTreatExplosionEventArgs> OnTreatExplosion;
    public event EventHandler<OnTreatExplosionEventArgs> OnThisTreatExplosion;

    public class OnTreatExplosionEventArgs : EventArgs
    {
        public Vector2 position;
        public int damage;
        public float explosionRadius;
    }

    private void OnEnable()
    {
        EnemyHealth.OnAnyEnemyDeath += EnemyHealth_OnAnyEnemyDeath;
    }

    private void OnDisable()
    {
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

    private void HandleTreatExplosion(object sender, EntityHealth.OnEntityDeathEventArgs e)
    {
        if (e.damageSource.GetDamageSourceClassification() != DamageSourceClassification.Character) return; //Must be damaged by character

        bool probability = MechanicsUtilities.GetProbability(EnemyExplosionTreatEffectSO.explosionProbability);

        if(!probability) return;

        Vector2 position = GeneralUtilities.TransformPositionVector2((sender as EntityHealth).transform);

        DamageData damageData = new DamageData(EnemyExplosionTreatEffectSO.explosionDamage, true, EnemyExplosionTreatEffectSO, false, true, true, true);
        MechanicsUtilities.DealDamageInArea(position, EnemyExplosionTreatEffectSO.explosionRadius, damageData, explosionLayerMask);

        OnTreatExplosion?.Invoke(this, new OnTreatExplosionEventArgs { damage = damageData.damage, explosionRadius = EnemyExplosionTreatEffectSO.explosionRadius, position = position });
        OnThisTreatExplosion?.Invoke(this, new OnTreatExplosionEventArgs { damage = damageData.damage, explosionRadius = EnemyExplosionTreatEffectSO.explosionRadius, position = position });
    }

    #region Subscriptions
    private void EnemyHealth_OnAnyEnemyDeath(object sender, EntityHealth.OnEntityDeathEventArgs e)
    {
        if (!isCurrentlyActiveByInventoryObjects) return;
        if (!isMeetingCondition) return; //In this treat condition is always true

        HandleTreatExplosion(sender, e);
    }
    #endregion
}
