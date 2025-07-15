using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthStatResolver : NumericStatResolver
{
    public static MaxHealthStatResolver Instance { get; private set; }

    public static event EventHandler<OnNumericResolverEventArgs> OnMaxHealtResolverInitialized;
    public static event EventHandler<OnNumericResolverEventArgs> OnMaxHealthResolverUpdated;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one MaxHealthStatResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override NumericStatType GetNumericStatType() => NumericStatType.MaxHealth;

    #region Abstract Methods
    protected override void OnResolverInitializedMethod()
    {
        OnMaxHealtResolverInitialized?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }

    protected override void OnResolverUpdatedMethod()
    {
        OnMaxHealthResolverUpdated?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }
    #endregion
}
