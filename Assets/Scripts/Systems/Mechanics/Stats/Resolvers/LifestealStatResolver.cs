using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifestealStatResolver : NumericStatResolver
{
    public static LifestealStatResolver Instance { get; private set; }

    public static event EventHandler<OnNumericResolverEventArgs> OnLifestealResolverInitialized;
    public static event EventHandler<OnNumericResolverEventArgs> OnLifestealResolverUpdated;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one LifestealStatResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override NumericStatType GetNumericStatType() => NumericStatType.Lifesteal;

    #region Abstract Methods
    protected override void OnResolverInitializedMethod()
    {
        OnLifestealResolverInitialized?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }

    protected override void OnResolverUpdatedMethod()
    {
        OnLifestealResolverUpdated?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }
    #endregion
}
