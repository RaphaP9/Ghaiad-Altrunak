using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCritChanceStatResolver : NumericStatResolver
{
    public static AttackCritChanceStatResolver Instance { get; private set; }

    public static event EventHandler<OnNumericResolverEventArgs> OnAttackCritChanceResolverInitialized;
    public static event EventHandler<OnNumericResolverEventArgs> OnAttackCritChanceResolverUpdated;
    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AttackCritChanceStatResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override NumericStatType GetNumericStatType() => NumericStatType.AttackCritChance;

    #region Abstract Methods
    protected override void OnResolverInitializedMethod()
    {
        OnAttackCritChanceResolverInitialized?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }

    protected override void OnResolverUpdatedMethod()
    {
        OnAttackCritChanceResolverUpdated?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }
    #endregion

}
