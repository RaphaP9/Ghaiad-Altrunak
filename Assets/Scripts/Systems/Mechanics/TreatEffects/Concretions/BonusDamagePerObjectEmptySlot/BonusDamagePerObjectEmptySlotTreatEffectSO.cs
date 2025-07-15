using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BonusDamagePerObjectEmptySlotTreatEffectSO", menuName = "ScriptableObjects/TreatEffects/BonusDamagePerObjectEmptySlotTreatEffect")]
public class BonusDamagePerObjectEmptySlotTreatEffectSO : TreatEffectSO
{
    [Header("Specific Settings")]
    public string refferencialGUID;
    [Space]
    public NumericEmbeddedStat statPerStack;
}

