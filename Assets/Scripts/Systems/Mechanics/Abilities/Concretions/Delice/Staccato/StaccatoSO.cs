using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaccatoSO", menuName = "ScriptableObjects/Abilities/Delice/Staccato")]
public class StaccatoSO : ActiveAbilitySO, IHasEmbeddedNumericStats
{
    [Header("Specific Settings")]
    public List<NumericEmbeddedStat> numericEmbeddedStats;
    [Space]
    [Range(0.5f, 5f)] public float duration;
    [Range(1, 3)] public int burstCount;
    [Range(0.5f, 0.8f)] public float secondaryAttackDamagePercentage;

    [Header("Technical Settings")]
    [Range(0f, 1f)] public float performanceTime;
    [Range(0.05f, 0.2f)] public float burstInterval;
    [Space]
    [Range(0.05f, 0.2f)] public float secondaryAttackInterval;
    [Range(5f, 10f)] public float secondaryBurstAngleDeviation;

    public List<NumericEmbeddedStat> GetNumericEmbeddedStats() => numericEmbeddedStats;
}
