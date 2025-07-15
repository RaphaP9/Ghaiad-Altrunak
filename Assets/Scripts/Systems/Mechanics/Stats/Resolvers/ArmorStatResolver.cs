using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorStatResolver : NumericStatResolver
{
    public static ArmorStatResolver Instance { get; private set; }

    public static event EventHandler<OnNumericResolverEventArgs> OnArmorResolverInitialized;
    public static event EventHandler<OnNumericResolverEventArgs> OnArmorResolverUpdated;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one ArmorStatResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override NumericStatType GetNumericStatType() => NumericStatType.Armor;

    #region Abstract Methods
    protected override void OnResolverInitializedMethod()
    {
        OnArmorResolverInitialized?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }

    protected override void OnResolverUpdatedMethod()
    {
        OnArmorResolverUpdated?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }
    #endregion

}