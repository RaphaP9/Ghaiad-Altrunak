using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AssetStatModifierManager : StatModifierManager
{
    [Header("Lists - Runtime Filled")]
    [SerializeField] protected List<AssetStatModifier> assetStatModifiers;

    public List<AssetStatModifier> AssetStatModifiers => assetStatModifiers;


    #region In-Line Methods
    public override bool HasStatModifiers() => assetStatModifiers.Count > 0;
    public override int GetStatModifiersQuantity() => assetStatModifiers.Count;

    protected override StatValueType GetStatValueType() => StatValueType.Asset;

    #endregion

    #region Add/Remove Stat Modifiers
    public void AddStatModifiers(string originGUID, IHasEmbeddedAssetStats assetEmbeddedStatsHolder)
    {
        if (originGUID == "")
        {
            if (debug) Debug.Log("GUID is empty. StatModifiers will not be added");
            return;
        }

        int statsAdded = 0;

        foreach (AssetEmbeddedStat assetEmbeddedStat in assetEmbeddedStatsHolder.GetAssetEmbeddedStats())
        {
            if (AddAssetStatModifier(originGUID, assetEmbeddedStat)) statsAdded++;
        }

        if (statsAdded > 0) UpdateStats();
    }

    protected bool AddAssetStatModifier(string originGUID, AssetEmbeddedStat assetEmbeddedStat)
    {
        if (assetEmbeddedStat == null)
        {
            if (debug) Debug.Log("AssetEmbeddedStat is null. StatModifier will not be added");
            return false;
        }

        if (assetEmbeddedStat.GetStatValueType() != GetStatValueType()) return false; //If not asset, return false

        AssetStatModifier assetStatModifier = new AssetStatModifier { originGUID = originGUID, assetStatType = assetEmbeddedStat.assetStatType, assetStatModificationType = assetEmbeddedStat.assetStatModificationType, asset = assetEmbeddedStat.asset};

        assetStatModifiers.Add(assetStatModifier);

        return true;
    }

    public override void RemoveStatModifiersByGUID(string originGUID)
    {
        if (originGUID == "")
        {
            if (debug) Debug.Log("GUID is empty. StatModifiers will not be removed");
            return;
        }

        int removedStats = assetStatModifiers.RemoveAll(statModifier => statModifier.originGUID == originGUID);

        if (removedStats > 0) UpdateStats();
    }
    #endregion

    public void SetStatList(List<AssetStatModifier> setterList) => assetStatModifiers.AddRange(setterList); //Add, NOT Replace!
}
