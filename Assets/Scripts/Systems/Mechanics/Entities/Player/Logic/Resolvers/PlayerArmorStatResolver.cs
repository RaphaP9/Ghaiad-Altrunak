using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmorStatResolver : EntityArmorStatResolver
{
    private CharacterIdentifier CharacterIdentifier => entityIdentifier as CharacterIdentifier;

    protected virtual void OnEnable()
    {
        ArmorStatResolver.OnArmorResolverUpdated += ArmorStatResolver_OnArmorResolverUpdated;
    }

    protected virtual void OnDisable()
    {
        ArmorStatResolver.OnArmorResolverUpdated -= ArmorStatResolver_OnArmorResolverUpdated;
    }

    protected override int CalculateStat()
    {
        return ArmorStatResolver.Instance.ResolveStatInt(CharacterIdentifier.CharacterSO.baseArmor);
    }

    private void ArmorStatResolver_OnArmorResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateStat();
    }
}
