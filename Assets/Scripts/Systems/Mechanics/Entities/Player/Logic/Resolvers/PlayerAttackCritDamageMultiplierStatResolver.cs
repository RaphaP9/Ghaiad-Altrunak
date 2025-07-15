using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCritDamageMultiplierStatResolver : EntityAttackCritDamageMultiplierStatResolver
{
    private CharacterIdentifier CharacterIdentifier => entityIdentifier as CharacterIdentifier;

    protected virtual void OnEnable()
    {
        AttackCritDamageMultiplierStatResolver.OnAttackCritDamageMultiplierResolverUpdated += AttackCritDamageMultiplierStatResolver_OnAttackCritDamageMultiplierResolverUpdated;
    }

    protected virtual void OnDisable()
    {
        AttackCritDamageMultiplierStatResolver.OnAttackCritDamageMultiplierResolverUpdated -= AttackCritDamageMultiplierStatResolver_OnAttackCritDamageMultiplierResolverUpdated;
    }

    protected override float CalculateStat()
    {
        return AttackCritDamageMultiplierStatResolver.Instance.ResolveStatFloat(CharacterIdentifier.CharacterSO.baseAttackCritDamageMultiplier);
    }

    private void AttackCritDamageMultiplierStatResolver_OnAttackCritDamageMultiplierResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateStat();
    }
}
