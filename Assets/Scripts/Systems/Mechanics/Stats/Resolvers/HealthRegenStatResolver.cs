using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegenStatResolver : NumericStatResolver
{
    public static HealthRegenStatResolver Instance { get; private set; }

    public static event EventHandler<OnNumericResolverEventArgs> OnHealthRegenResolverInitialized;
    public static event EventHandler<OnNumericResolverEventArgs> OnHealthRegenResolverUpdated;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one HealthRegenStatResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override NumericStatType GetNumericStatType() => NumericStatType.HealthRegen;

    #region Abstract Methods
    protected override void OnResolverInitializedMethod()
    {
        OnHealthRegenResolverInitialized?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }

    protected override void OnResolverUpdatedMethod()
    {
        OnHealthRegenResolverUpdated?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }
    #endregion
}

