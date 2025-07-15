using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeChanceStatResolver : EntityDodgeChanceStatResolver
{
    private CharacterIdentifier CharacterIdentifier => entityIdentifier as CharacterIdentifier;

    protected virtual void OnEnable()
    {
        DodgeChanceStatResolver.OnDodgeChanceResolverUpdated += DodgeChanceStatResolver_OnDodgeChanceResolverUpdated;
    }

    protected virtual void OnDisable()
    {
        DodgeChanceStatResolver.OnDodgeChanceResolverUpdated -= DodgeChanceStatResolver_OnDodgeChanceResolverUpdated;
    }

    protected override float CalculateStat()
    {
        return DodgeChanceStatResolver.Instance.ResolveStatFloat(CharacterIdentifier.CharacterSO.baseDodgeChance);
    }

    private void DodgeChanceStatResolver_OnDodgeChanceResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateStat();
    }
}
