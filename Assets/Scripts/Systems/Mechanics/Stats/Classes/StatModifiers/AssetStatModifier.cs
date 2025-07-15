using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AssetStatModifier : StatModifier
{
    public AssetStatType assetStatType;
    public AssetStatModificationType assetStatModificationType;
    public AssetStatSO asset;

    public override StatValueType GetStatValueType() => StatValueType.Asset;
}
