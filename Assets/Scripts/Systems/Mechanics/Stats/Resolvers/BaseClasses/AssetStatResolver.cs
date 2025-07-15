using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public abstract class AssetStatResolver<T> : StatResolver where T : AssetStatSO
{
    [Header("Lists")]
    [SerializeField] private List<AssetStatModifierManager> assetStatModifierManagers;

    [Header("Resolved Values - Runtime Filled")]
    [SerializeField] protected List<T> unionValues;
    [SerializeField] protected T replacementValue;

    public List<AssetStatModifierManager> AssetStatModifierManagers => assetStatModifierManagers;

    public class OnAssetResolverEventArgs : EventArgs
    {
        public List<T> unionValue;
        public T replacementValue;
    }

    protected bool CheckAssetIsT(AssetStatSO assetStatSO) => assetStatSO is T;
    protected abstract AssetStatType GetAssetStatType();

    public virtual List<T> ResolveStat(List<T> baseList)
    {
        List<T> resolvedStatList = new List<T>();

        List<T> unionValues = ResolveUnionValues();
        T replacementValue = ResolveReplacementValue();

        if(replacementValue != null)
        {
            resolvedStatList.Add(replacementValue);
            return resolvedStatList;
        }

        resolvedStatList.AddRange(baseList);
        resolvedStatList.AddRange(unionValues);

        return resolvedStatList;
    }

    protected virtual List<T> ResolveUnionValues()
    {
        List<T> unionValues = new List<T>();

        foreach (AssetStatModifierManager assetStatModifierManager in assetStatModifierManagers)
        {
            foreach (AssetStatModifier assetStatModifier in assetStatModifierManager.AssetStatModifiers)
            {
                if (assetStatModifier.assetStatType != GetAssetStatType()) continue;
                if (assetStatModifier.asset == null) continue;
                if (!CheckAssetIsT(assetStatModifier.asset)) continue;

                if (assetStatModifier.assetStatModificationType == AssetStatModificationType.Union)
                {
                    unionValues.Add(assetStatModifier.asset as T);
                }
            }
        }

        return unionValues;
    }

    protected virtual T ResolveReplacementValue()
    {
        foreach (AssetStatModifierManager assetStatModifierManager in assetStatModifierManagers)
        {
            foreach (AssetStatModifier assetStatModifier in assetStatModifierManager.AssetStatModifiers)
            {
                if (assetStatModifier.assetStatType != GetAssetStatType()) continue;
                if (assetStatModifier.asset == null) continue;
                if (!CheckAssetIsT(assetStatModifier.asset)) continue;

                if (assetStatModifier.assetStatModificationType == AssetStatModificationType.Replacement)
                {
                    return assetStatModifier.asset as T;
                }
            }
        }

        return null;
    }

    protected override void InitializeResolver()
    {
        CalculateAssetResolverParameters();
        OnResolverInitializedMethod();
    }

    protected override void UpdateResolver()
    {
        List<T> previousUnionValues = unionValues;
        T previousReplacementValue = replacementValue;

        CalculateAssetResolverParameters();

        if (GeneralUtilities.ListsAreExactlyEqual(previousUnionValues, unionValues) && previousReplacementValue == replacementValue) return; //If none changed, do nothing

        OnResolverUpdatedMethod();
    }

    protected void CalculateAssetResolverParameters()
    {
        unionValues = ResolveUnionValues();
        replacementValue = ResolveReplacementValue();
    }
}
