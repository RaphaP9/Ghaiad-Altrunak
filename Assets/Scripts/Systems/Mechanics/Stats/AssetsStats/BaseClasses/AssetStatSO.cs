using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AssetStatSO : ScriptableObject
{
    [Header("Identifiers")]
    public int id;

    public abstract AssetStatType GetAssetStatType();
}
