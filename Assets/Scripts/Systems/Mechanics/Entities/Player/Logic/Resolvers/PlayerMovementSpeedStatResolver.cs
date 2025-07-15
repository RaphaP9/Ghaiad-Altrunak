using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSpeedStatResolver : EntityMovementSpeedStatResolver
{
    private CharacterIdentifier CharacterIdentifier => entityIdentifier as CharacterIdentifier;

    protected override void OnEnable()
    {
        base.OnEnable();
        MovementSpeedStatResolver.OnMovementSpeedResolverUpdated += MovementSpeedStatResolver_OnMovementSpeedResolverUpdated;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        MovementSpeedStatResolver.OnMovementSpeedResolverUpdated -= MovementSpeedStatResolver_OnMovementSpeedResolverUpdated;
    }

    protected override float CalculateStat()
    {
        float resolvedValue = MovementSpeedStatResolver.Instance.ResolveStatFloat(CharacterIdentifier.CharacterSO.baseMovementSpeed) * (1 - entitySlowStatusEffectHandler.SlowPercentageResolvedValue);
        return resolvedValue;
    }

    private void MovementSpeedStatResolver_OnMovementSpeedResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateStat();
    }
}