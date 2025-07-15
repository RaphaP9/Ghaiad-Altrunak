using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownReductionStatResolver : NumericStatResolver
{
    public static CooldownReductionStatResolver Instance { get; private set; }

    public static event EventHandler<OnNumericResolverEventArgs> OnCooldownResolverInitialized;
    public static event EventHandler<OnNumericResolverEventArgs> OnCooldownResolverUpdated;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one CooldownStatResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override NumericStatType GetNumericStatType() => NumericStatType.CooldownReduction;

    #region Abstract Methods
    protected override void OnResolverInitializedMethod()
    {
        OnCooldownResolverInitialized?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }

    protected override void OnResolverUpdatedMethod()
    {
        OnCooldownResolverUpdated?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }
    #endregion
}
