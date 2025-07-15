using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldRegenStatResolver : EntityIntStatResolver
{
    private CharacterIdentifier CharacterIdentifier => entityIdentifier as CharacterIdentifier;

    protected virtual void OnEnable()
    {
        ShieldRegenStatResolver.OnShieldRegenResolverUpdated += ShieldRegenStatResolver_OnShieldRegenResolverUpdated;
    }

    protected virtual void OnDisable()
    {
        ShieldRegenStatResolver.OnShieldRegenResolverUpdated -= ShieldRegenStatResolver_OnShieldRegenResolverUpdated;
    }

    protected override int CalculateStat()
    {
        return ShieldRegenStatResolver.Instance.ResolveStatInt(CharacterIdentifier.CharacterSO.baseShieldRegen);
    }

    protected override int CalculateBaseValue()
    {
        return CharacterIdentifier.CharacterSO.baseShieldRegen;
    }

    private void ShieldRegenStatResolver_OnShieldRegenResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateStat();
    }
}
