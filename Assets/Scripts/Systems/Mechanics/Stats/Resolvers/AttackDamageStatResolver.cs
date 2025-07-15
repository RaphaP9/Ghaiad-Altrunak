using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamageStatResolver : NumericStatResolver
{
    public static AttackDamageStatResolver Instance { get; private set; }

    public static event EventHandler<OnNumericResolverEventArgs> OnAttackDamageResolverInitialized;
    public static event EventHandler<OnNumericResolverEventArgs> OnAttackDamageResolverUpdated;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AttackDamageStatResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override NumericStatType GetNumericStatType() => NumericStatType.AttackDamage;

    #region Abstract Methods
    protected override void OnResolverInitializedMethod()
    {
        OnAttackDamageResolverInitialized?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }

    protected override void OnResolverUpdatedMethod()
    {
        OnAttackDamageResolverUpdated?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }
    #endregion

}

