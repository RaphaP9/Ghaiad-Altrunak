using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaxShieldStatResolver : EntityMaxShieldStatResolver
{
    private CharacterIdentifier CharacterIdentifier => entityIdentifier as CharacterIdentifier;

    protected virtual void OnEnable()
    {
        MaxShieldStatResolver.OnMaxShieldResolverUpdated += MaxShieldStatResolver_OnMaxShieldResolverUpdated;
    }

    protected virtual void OnDisable()
    {
        MaxShieldStatResolver.OnMaxShieldResolverUpdated -= MaxShieldStatResolver_OnMaxShieldResolverUpdated;
    }

    protected override int CalculateStat()
    {
        return MaxShieldStatResolver.Instance.ResolveStatInt(CharacterIdentifier.CharacterSO.baseShield);
    }

    private void MaxShieldStatResolver_OnMaxShieldResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateStat();
    }
}
