using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoldHealTreatEffectSO", menuName = "ScriptableObjects/TreatEffects/GoldHealTreatEffect")]
public class GoldHealTreatEffectSO : TreatEffectSO, IHealSource
{
    [Header("Specific Settings")]
    [Range(1, 3)] public int healPerGold;
    [Range(0f, 1f)] public float healProbability;

    #region IHealSource Methods
    public string GetHealSourceName() => treatName;
    public string GetHealSourceDescription() => description;
    public Sprite GetHealSourceSprite() => sprite;
    #endregion
}
