using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class NumericStatResolver : StatResolver
{
    [Header("Lists")]
    [SerializeField] protected List<NumericStatModifierManager> numericStatModifierManagers;

    [Header("Resolved Values - Runtime Filled")]
    [SerializeField] protected float additiveValue;
    [SerializeField] protected float multiplierValue;
    [SerializeField] protected float replacementValue;

    public List<NumericStatModifierManager> NumericStatModifierManagers => numericStatModifierManagers;

    protected const float NON_EXISTENT_REPLACEMENT_VALUE = -1f;

    public class OnNumericResolverEventArgs : EventArgs
    {
        public float additiveValue;
        public float multiplierValue;
        public float replacementValue;
    }

    protected abstract NumericStatType GetNumericStatType();

    public virtual float ResolveStatFloat(float baseValue)
    {
        float additiveValue = ResolveAdditiveValue();
        float multiplierValue = ResolveMultiplierValue();
        float replacementValue = ResolveReplacementValue();

        if (replacementValue != NON_EXISTENT_REPLACEMENT_VALUE)
        {
            replacementValue = MechanicsUtilities.ClampNumericStat(replacementValue, GetNumericStatType());
            return replacementValue;
        }

        float resolvedStat = (baseValue + additiveValue) * multiplierValue;
        resolvedStat = MechanicsUtilities.ClampNumericStat(resolvedStat, GetNumericStatType());

        return resolvedStat;    
    }

    public virtual int ResolveStatInt(int baseValue)
    {
        float resolvedValue = ResolveStatFloat(baseValue);
        int resolvedValueInt = Mathf.CeilToInt(resolvedValue);
        return resolvedValueInt;
    }

    protected virtual float ResolveAdditiveValue()
    {
        float accumulatedAdditiveValue = 0f;

        foreach (NumericStatModifierManager numericStatModifierManager in numericStatModifierManagers)
        {
            foreach (NumericStatModifier numericStatModifier in numericStatModifierManager.NumericStatModifiers)
            {
                if (numericStatModifier.numericStatType != GetNumericStatType()) continue;

                if (numericStatModifier.numericStatModificationType == NumericStatModificationType.Value)
                {
                    accumulatedAdditiveValue += numericStatModifier.value;
                }
            }
        }

        return accumulatedAdditiveValue;
    }
    protected virtual float ResolveMultiplierValue()
    {
        float accumulatedMultiplierValue = 1f;

        foreach (NumericStatModifierManager numericStatModifierManager in numericStatModifierManagers)
        {
            foreach (NumericStatModifier numericStatModifier in numericStatModifierManager.NumericStatModifiers)
            {
                if (numericStatModifier.numericStatType != GetNumericStatType()) continue;

                if (numericStatModifier.numericStatModificationType == NumericStatModificationType.Percentage)
                {
                   accumulatedMultiplierValue += numericStatModifier.value;
                }
            }
        }

        return accumulatedMultiplierValue;
    }
    protected virtual float ResolveReplacementValue()
    {
        foreach (NumericStatModifierManager numericStatModifierManager in numericStatModifierManagers)
        {
            foreach (NumericStatModifier numericStatModifier in numericStatModifierManager.NumericStatModifiers)
            {
                if (numericStatModifier.numericStatType != GetNumericStatType()) continue;

                if (numericStatModifier.numericStatModificationType == NumericStatModificationType.Replacement)
                {
                    return numericStatModifier.value;
                }
            }
        }

        return NON_EXISTENT_REPLACEMENT_VALUE;
    }


    protected override void InitializeResolver()
    {
        CalculateNumericResolverParameters();
        OnResolverInitializedMethod();
    }

    protected override void UpdateResolver()
    {
        float previousAdditiveValue = additiveValue;
        float previousMultiplierValue = multiplierValue;
        float previousReplacementValue = replacementValue;

        CalculateNumericResolverParameters();

        if (previousAdditiveValue == additiveValue && previousMultiplierValue == multiplierValue && previousReplacementValue == replacementValue) return; //If none changed, do nothing
              
        OnResolverUpdatedMethod();
    }

    protected void CalculateNumericResolverParameters()
    {
        additiveValue = ResolveAdditiveValue();
        multiplierValue = ResolveMultiplierValue();
        replacementValue = ResolveReplacementValue();
    }

}
