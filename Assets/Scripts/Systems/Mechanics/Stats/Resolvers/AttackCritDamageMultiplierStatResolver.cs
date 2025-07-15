using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCritDamageMultiplierStatResolver : NumericStatResolver
{
    public static AttackCritDamageMultiplierStatResolver Instance { get; private set; }

    public static event EventHandler<OnNumericResolverEventArgs> OnAttackCritDamageMultiplierResolverInitialized;
    public static event EventHandler<OnNumericResolverEventArgs> OnAttackCritDamageMultiplierResolverUpdated;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AttackCritDamageMultiplierStatResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override NumericStatType GetNumericStatType() => NumericStatType.AttackCritDamageMultiplier;

    #region Abstract Methods
    protected override void OnResolverInitializedMethod()
    {
        OnAttackCritDamageMultiplierResolverInitialized?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }

    protected override void OnResolverUpdatedMethod()
    {
        OnAttackCritDamageMultiplierResolverUpdated?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }
    #endregion
}
