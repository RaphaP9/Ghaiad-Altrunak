using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TemporalStatChangeActiveAbilitySO", menuName = "ScriptableObjects/Abilities/Active/TemporalStatChangeActiveAbility")]
public class TemporalStatChangeActiveAbilitySO : ActiveAbilitySO, IHasEmbeddedNumericStats
{
    [Header("Specific Settings")]
    public List<NumericEmbeddedStat> numericEmbeddedStats;
    [Range(3f,5f)] public float changeDuration;

    public List<NumericEmbeddedStat> GetNumericEmbeddedStats() => numericEmbeddedStats;
}
