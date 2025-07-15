using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldRegenStatResolver : NumericStatResolver
{
    public static ShieldRegenStatResolver Instance { get; private set; }

    public static event EventHandler<OnNumericResolverEventArgs> OnShieldRegenResolverInitialized;
    public static event EventHandler<OnNumericResolverEventArgs> OnShieldRegenResolverUpdated;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one ShieldRegenResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override NumericStatType GetNumericStatType() => NumericStatType.ShieldRegen;

    #region Abstract Methods
    protected override void OnResolverInitializedMethod()
    {
        OnShieldRegenResolverInitialized?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }

    protected override void OnResolverUpdatedMethod()
    {
        OnShieldRegenResolverUpdated?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }
    #endregion

}

