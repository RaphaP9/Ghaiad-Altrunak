using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasEmbeddedAssetStats 
{
    public List<AssetEmbeddedStat> GetAssetEmbeddedStats();
}
