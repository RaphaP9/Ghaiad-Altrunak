using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BonusDamageLowHealthTreatEffectSO", menuName = "ScriptableObjects/TreatEffects/BonusDamageLowHealthTreatEffect")]
public class BonusDamageLowHealthTreatEffectSO : TreatEffectSO, IHasEmbeddedNumericStats
{
    [Header("Specific Settings")]
    public string refferencialGUID;
    [Space]
    [Range(1, 10)] public int healthThreshold;
    public List<NumericEmbeddedStat> numericEmbeddedStats;

    public List<NumericEmbeddedStat> GetNumericEmbeddedStats() => numericEmbeddedStats;
}
