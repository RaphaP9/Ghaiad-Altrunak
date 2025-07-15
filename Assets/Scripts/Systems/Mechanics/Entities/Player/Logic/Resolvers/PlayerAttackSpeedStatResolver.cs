using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSpeedStatResolver : EntityAttackSpeedStatResolver
{
    private CharacterIdentifier CharacterIdentifier => entityIdentifier as CharacterIdentifier;

    protected virtual void OnEnable()
    {
        AttackSpeedStatResolver.OnAttackSpeedResolverUpdated += AttackSpeedStatResolver_OnAttackSpeedResolverUpdated;
    }

    protected virtual void OnDisable()
    {
        AttackSpeedStatResolver.OnAttackSpeedResolverUpdated -= AttackSpeedStatResolver_OnAttackSpeedResolverUpdated;
    }

    protected override float CalculateStat()
    {
        return AttackSpeedStatResolver.Instance.ResolveStatFloat(CharacterIdentifier.CharacterSO.baseAttackSpeed);
    }

    private void AttackSpeedStatResolver_OnAttackSpeedResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateStat();
    }
}