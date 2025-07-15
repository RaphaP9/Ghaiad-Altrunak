using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxShieldStatResolver : NumericStatResolver
{
    public static MaxShieldStatResolver Instance { get; private set; }

    public static event EventHandler<OnNumericResolverEventArgs> OnMaxShieldResolverInitialized;
    public static event EventHandler<OnNumericResolverEventArgs> OnMaxShieldResolverUpdated;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one MaxShieldStatResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override NumericStatType GetNumericStatType() => NumericStatType.MaxShield;

    #region Abstract Methods
    protected override void OnResolverInitializedMethod()
    {
        OnMaxShieldResolverInitialized?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }

    protected override void OnResolverUpdatedMethod()
    {
        OnMaxShieldResolverUpdated?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }
    #endregion

}

