using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoundAttackDamageBuffUntilAbilityCastTreatEffectSO", menuName = "ScriptableObjects/TreatEffects/RoundAttackDamageBuffUntilAbilityCastTreatEffect")]
public class RoundAttackDamageBuffUntilAbilityCastTreatEffectSO : TreatEffectSO
{
    [Header("Specific Settings")]
    public string refferencialGUID;
    [Space]
    public NumericEmbeddedStat buffStat;
}

