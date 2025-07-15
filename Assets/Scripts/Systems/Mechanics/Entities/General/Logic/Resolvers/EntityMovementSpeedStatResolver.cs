using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovementSpeedStatResolver : EntityFloatStatResolver
{
    [Header("Entity Components")]
    [SerializeField] protected EntitySlowStatusEffectHandler entitySlowStatusEffectHandler;

    protected virtual void OnEnable()
    {
        entitySlowStatusEffectHandler.OnSlowStatusEffectValueRecauculated += EntitySlowStatusEffectHandler_OnSlowStatusEffectValueRecauculated;
    }

    protected virtual void OnDisable()
    {
        entitySlowStatusEffectHandler.OnSlowStatusEffectValueRecauculated -= EntitySlowStatusEffectHandler_OnSlowStatusEffectValueRecauculated;
    }

    protected override float CalculateStat() => entityIdentifier.EntitySO.baseMovementSpeed * (1-entitySlowStatusEffectHandler.SlowPercentageResolvedValue); 
    protected override float CalculateBaseValue() => entityIdentifier.EntitySO.baseMovementSpeed;

    private void EntitySlowStatusEffectHandler_OnSlowStatusEffectValueRecauculated(object sender, EntitySlowStatusEffectHandler.OnSlowStatusEffectValueEventArgs e)
    {
        RecalculateStat();
    }
}

