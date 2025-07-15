using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCritChanceStatResolver : EntityAttackCritChanceStatResolver
{
    private CharacterIdentifier CharacterIdentifier => entityIdentifier as CharacterIdentifier;

    protected virtual void OnEnable()
    {
        AttackCritChanceStatResolver.OnAttackCritChanceResolverUpdated += AttackCritChanceStatResolver_OnAttackCritChanceResolverUpdated;
    }

    protected virtual void OnDisable()
    {
        AttackCritChanceStatResolver.OnAttackCritChanceResolverUpdated -= AttackCritChanceStatResolver_OnAttackCritChanceResolverUpdated;
    }

    protected override float CalculateStat()
    {
        return AttackCritChanceStatResolver.Instance.ResolveStatFloat(CharacterIdentifier.CharacterSO.baseAttackCritChance);
    }

    private void AttackCritChanceStatResolver_OnAttackCritChanceResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateStat();
    }
}
