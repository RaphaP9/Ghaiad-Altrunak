using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoundStackingDamagePerGoldTreatEffectSO", menuName = "ScriptableObjects/TreatEffects/RoundStackingDamagePerGoldTreatEffect")]
public class RoundStackingDamagePerGoldTreatEffectSO : TreatEffectSO
{
    [Header("Specific Settings")]
    public string refferencialGUID;
    [Space]
    [Range(0f, 1f)] public float stackProbability;
    public NumericEmbeddedStat statPerStack;
}

