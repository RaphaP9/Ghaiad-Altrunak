using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifestealStatResolver : EntityLifestealStatResolver
{
    private CharacterIdentifier CharacterIdentifier => entityIdentifier as CharacterIdentifier;

    protected virtual void OnEnable()
    {
        LifestealStatResolver.OnLifestealResolverUpdated += LifestealStatResolver_OnLifestealResolverUpdated;
    }

    protected virtual void OnDisable()
    {
        LifestealStatResolver.OnLifestealResolverUpdated -= LifestealStatResolver_OnLifestealResolverUpdated;
    }

    protected override float CalculateStat()
    {
        return LifestealStatResolver.Instance.ResolveStatFloat(CharacterIdentifier.CharacterSO.baseLifesteal);
    }

    private void LifestealStatResolver_OnLifestealResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateStat();
    }
}