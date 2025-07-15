using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpeedStatResolver : NumericStatResolver
{
    public static MovementSpeedStatResolver Instance { get; private set; }

    public static event EventHandler<OnNumericResolverEventArgs> OnMovementSpeedResolverInitialized;
    public static event EventHandler<OnNumericResolverEventArgs> OnMovementSpeedResolverUpdated;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one MovementSpeedStatResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override NumericStatType GetNumericStatType() => NumericStatType.MovementSpeed;

    #region Abstract Methods
    protected override void OnResolverInitializedMethod()
    {
        OnMovementSpeedResolverInitialized?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }

    protected override void OnResolverUpdatedMethod()
    {
        OnMovementSpeedResolverUpdated?.Invoke(this, new OnNumericResolverEventArgs { additiveValue = additiveValue, multiplierValue = multiplierValue, replacementValue = replacementValue });
    }
    #endregion

}
