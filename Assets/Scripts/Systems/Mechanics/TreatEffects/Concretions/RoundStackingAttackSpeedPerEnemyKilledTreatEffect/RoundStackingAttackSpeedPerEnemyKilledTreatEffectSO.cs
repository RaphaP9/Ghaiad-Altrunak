using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoundStackingAttackSpeedPerEnemyKilledTreatEffectSO", menuName = "ScriptableObjects/TreatEffects/RoundStackingAttackSpeedPerEnemyKilledTreatEffect")]
public class RoundStackingAttackSpeedPerEnemyKilledTreatEffectSO : TreatEffectSO
{
    [Header("Specific Settings")]
    public string refferencialGUID;
    [Space]
    public NumericEmbeddedStat statPerStack;
}


