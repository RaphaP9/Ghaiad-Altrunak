using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ExecuteTreatEffectSO", menuName = "ScriptableObjects/TreatEffects/ExecuteTreatEffect")]
public class ExecuteTreatEffectSO : TreatEffectSO, IDamageSource
{
    [Header("Specific Settings")]
    [Range(1,10)] public int healthExecuteThreshold;
    [ColorUsage(true, true)] public Color damageColor;

    #region IDamageSource Methods
    public string GetDamageSourceName() => treatName;
    public string GetDamageSourceDescription() => description;
    public Sprite GetDamageSourceSprite() => sprite;
    public Color GetDamageSourceColor() => damageColor;
    public DamageSourceClassification GetDamageSourceClassification() => DamageSourceClassification.Treat;
    #endregion
}
