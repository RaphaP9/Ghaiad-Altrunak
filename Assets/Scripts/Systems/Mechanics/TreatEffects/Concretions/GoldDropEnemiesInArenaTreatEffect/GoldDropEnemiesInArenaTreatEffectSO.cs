using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoldDropEnemiesInArenaTreatEffectSO", menuName = "ScriptableObjects/TreatEffects/GoldDropEnemiesInArenaTreatEffect")]
public class GoldDropEnemiesInArenaTreatEffectSO : TreatEffectSO, IHasEmbeddedNumericStats
{
    [Header("Specific Settings")]
    public string refferencialGUID;
    [Space]
    [Range(1, 10)] public int enemiesInArenaThreshold;
    public List<NumericEmbeddedStat> numericEmbeddedStats;

    public List<NumericEmbeddedStat> GetNumericEmbeddedStats() => numericEmbeddedStats;
}
