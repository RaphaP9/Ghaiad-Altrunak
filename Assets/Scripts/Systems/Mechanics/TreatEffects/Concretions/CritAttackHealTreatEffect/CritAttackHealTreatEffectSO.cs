using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CritAttackHealTreatEffectSO", menuName = "ScriptableObjects/TreatEffects/CritAttackHealTreatEffect")]
public class CritAttackHealTreatEffectSO : TreatEffectSO, IHealSource
{
    [Header("Specific Settings")]
    [Range(1, 3)] public int healPerCrit;
    [Range(0f, 1f)] public float healProbability;

    #region IHealSource Methods
    public string GetHealSourceName() => treatName;
    public string GetHealSourceDescription() => description;
    public Sprite GetHealSourceSprite() => sprite;
    #endregion
}