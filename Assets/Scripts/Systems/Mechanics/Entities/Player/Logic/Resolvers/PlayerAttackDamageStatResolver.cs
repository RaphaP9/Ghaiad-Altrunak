using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackDamageStatResolver : EntityAttackDamageStatResolver
{
    private CharacterIdentifier CharacterIdentifier => entityIdentifier as CharacterIdentifier;

    protected virtual void OnEnable()
    {
        AttackDamageStatResolver.OnAttackDamageResolverUpdated += AttackDamageStatResolver_OnAttackDamageResolverUpdated;
    }

    protected virtual void OnDisable()
    {
        AttackDamageStatResolver.OnAttackDamageResolverUpdated -= AttackDamageStatResolver_OnAttackDamageResolverUpdated;
    }

    protected override int CalculateStat()
    {
        return AttackDamageStatResolver.Instance.ResolveStatInt(CharacterIdentifier.CharacterSO.baseAttackDamage);
    }

    private void AttackDamageStatResolver_OnAttackDamageResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateStat();
    }
}
