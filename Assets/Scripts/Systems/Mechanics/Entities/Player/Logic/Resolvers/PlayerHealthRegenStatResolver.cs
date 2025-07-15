using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthRegenStatResolver : EntityIntStatResolver
{
    private CharacterIdentifier CharacterIdentifier => entityIdentifier as CharacterIdentifier;

    protected virtual void OnEnable()
    {
        HealthRegenStatResolver.OnHealthRegenResolverUpdated += HealthRegenStatResolver_OnHealthRegenResolverUpdated;
    }

    protected virtual void OnDisable()
    {
        HealthRegenStatResolver.OnHealthRegenResolverUpdated -= HealthRegenStatResolver_OnHealthRegenResolverUpdated;
    }

    protected override int CalculateStat()
    {
        return HealthRegenStatResolver.Instance.ResolveStatInt(CharacterIdentifier.CharacterSO.baseHealthRegen);
    }

    protected override int CalculateBaseValue()
    {
        return CharacterIdentifier.CharacterSO.baseHealthRegen;
    }

    private void HealthRegenStatResolver_OnHealthRegenResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateStat();
    }
}
