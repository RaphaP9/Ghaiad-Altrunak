using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeChanceStatResolver : NumericStatResolver
{
    public static DodgeChanceStatResolver Instance { get; private set; }

    public static event EventHandler<OnNumericResolverEventArgs> OnDodgeChanceResolverInitialized;
    public static event EventHandler<OnNumericResolverEventArgs> OnDodgeChanceResolverUpdated;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one DodgeChanceStatResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override NumericStatType GetNumericStatType() => NumericStatType.DodgeChance;

    #region Abstract Methods
    protected override void OnResolverInitializedMethod()
    {
        OnDodgeChanceResolverInitialized?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }

    protected override void OnResolverUpdatedMethod()
    {
        OnDodgeChanceResolverUpdated?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }
    #endregion
}