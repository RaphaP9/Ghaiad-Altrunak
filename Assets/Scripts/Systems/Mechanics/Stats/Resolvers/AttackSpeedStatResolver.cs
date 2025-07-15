using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedStatResolver : NumericStatResolver
{
    public static AttackSpeedStatResolver Instance { get; private set; }

    public static event EventHandler<OnNumericResolverEventArgs> OnAttackSpeedResolverInitialized;
    public static event EventHandler<OnNumericResolverEventArgs> OnAttackSpeedResolverUpdated;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AttackSpeedStatResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override NumericStatType GetNumericStatType() => NumericStatType.AttackSpeed;

    #region Abstract Methods
    protected override void OnResolverInitializedMethod()
    {
        OnAttackSpeedResolverInitialized?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }

    protected override void OnResolverUpdatedMethod()
    {
        OnAttackSpeedResolverUpdated?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }
    #endregion

}
