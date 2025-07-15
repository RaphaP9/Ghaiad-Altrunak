using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AssetEmbeddedStat : EmbeddedStat
{
    public AssetStatType assetStatType;
    public AssetStatModificationType assetStatModificationType;
    public AssetStatSO asset;

    public override StatValueType GetStatValueType() => StatValueType.Asset;
}
