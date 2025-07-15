using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaxHealthStatResolver : EntityMaxHealthStatResolver
{
    private CharacterIdentifier CharacterIdentifier => entityIdentifier as CharacterIdentifier;

    protected virtual void OnEnable()
    {
        MaxHealthStatResolver.OnMaxHealthResolverUpdated += MaxHealthStatResolver_OnMaxHealthResolverUpdated;
    }

    protected virtual void OnDisable()
    {
        MaxHealthStatResolver.OnMaxHealthResolverUpdated -= MaxHealthStatResolver_OnMaxHealthResolverUpdated;
    }

    protected override int CalculateStat()
    {
        return MaxHealthStatResolver.Instance.ResolveStatInt(CharacterIdentifier.CharacterSO.baseHealth);
    }

    private void MaxHealthStatResolver_OnMaxHealthResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateStat();
    }
}
