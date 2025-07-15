using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoundStackingMovementSpeedPerGoldTreatEffectSO", menuName = "ScriptableObjects/TreatEffects/RoundStackingMovementSpeedPerGoldTreatEffect")]
public class RoundStackingMovementSpeedPerGoldTreatEffectSO : TreatEffectSO
{
    [Header("Specific Settings")]
    public string refferencialGUID;
    [Space]
    public NumericEmbeddedStat statPerStack;
}

