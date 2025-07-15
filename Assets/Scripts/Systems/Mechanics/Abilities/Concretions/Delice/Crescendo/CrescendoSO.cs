using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrescendoSO", menuName = "ScriptableObjects/Abilities/Delice/Crescendo")]
public class CrescendoSO : PassiveAbilitySO, IHasEmbeddedNumericStats
{
    [Header("Specific Settings")]
    public List<NumericEmbeddedStat> numericEmbeddedStats;
    [Range(0.5f, 5f)] public float bonificationDuration;

    public List<NumericEmbeddedStat> GetNumericEmbeddedStats() => numericEmbeddedStats;
}
