using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCooldownReductionStatResolver : EntityFloatStatResolver
{
    private CharacterIdentifier CharacterIdentifier => entityIdentifier as CharacterIdentifier;

    protected virtual void OnEnable()
    {
        CooldownReductionStatResolver.OnCooldownResolverUpdated += CooldownReductionResolver_OnCooldownReductionResolverUpdated;
    }

    protected virtual void OnDisable()
    {
        CooldownReductionStatResolver.OnCooldownResolverUpdated -= CooldownReductionResolver_OnCooldownReductionResolverUpdated;
    }

    protected override float CalculateStat()
    {
        return CooldownReductionStatResolver.Instance.ResolveStatFloat(CharacterIdentifier.CharacterSO.baseCooldownReduction);
    }

    protected override float CalculateBaseValue()
    {
        return CharacterIdentifier.CharacterSO.baseCooldownReduction;
    }

    public float GetAbilityCooldown(float abilityOriginalCooldown)
    {
        float newCooldown = MechanicsUtilities.ProcessAbilityCooldown(abilityOriginalCooldown, value);
        return newCooldown;
    }

    private void CooldownReductionResolver_OnCooldownReductionResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateStat();
    }
}