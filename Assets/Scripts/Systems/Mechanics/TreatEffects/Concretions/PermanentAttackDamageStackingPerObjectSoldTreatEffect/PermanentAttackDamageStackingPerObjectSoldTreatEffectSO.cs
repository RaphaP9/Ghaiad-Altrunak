using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PermanentAttackDamageStackingPerObjectSoldTreatEffectSO", menuName = "ScriptableObjects/TreatEffects/PermanentAttackDamageStackingPerObjectSoldTreatEffect")]
public class PermanentAttackDamageStackingPerObjectSoldTreatEffectSO : TreatEffectSO
{
    [Header("Specific Settings")]
    public string refferencialGUID;
    [Space]
    public NumericEmbeddedStat statPerStack;
}



