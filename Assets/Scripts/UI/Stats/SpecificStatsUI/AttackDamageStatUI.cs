using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamageStatUI : PlayerNumericStatUI<PlayerAttackDamageStatResolver>
{
    protected override void SubscribeToEvents()
    {
        resolver.OnEntityStatInitialized += Resolver_OnEntityStatInitialized;
        resolver.OnEntityStatUpdated += Resolver_OnEntityStatUpdated;
    }
    protected override void UnSubscribeToEvents()
    {
        if (resolver == null) return;

        resolver.OnEntityStatInitialized -= Resolver_OnEntityStatInitialized;
        resolver.OnEntityStatUpdated -= Resolver_OnEntityStatUpdated;
    }

    protected override float GetBaseValue() => resolver.BaseValue;
    protected override float GetCurrentValue() => resolver.Value;
    protected override NumericStatType GetNumericStatType() => NumericStatType.AttackDamage;


    #region Subscriptions
    private void Resolver_OnEntityStatInitialized(object sender, EntityIntStatResolver.OnStatEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }

    private void Resolver_OnEntityStatUpdated(object sender, EntityIntStatResolver.OnStatEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }
    #endregion
}