using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyExplosionTreatEffectSO", menuName = "ScriptableObjects/TreatEffects/EnemyExplosionTreatEffect")]
public class EnemyExplosionTreatEffectSO : TreatEffectSO, IDamageSource
{
    [Header("Specific Settings")]
    [Range(1, 10)] public int explosionDamage;
    [Range(0.5f, 2f)] public float explosionRadius;
    [ColorUsage(true, true)] public Color damageColor;
    [Space]
    [Range(0f,1f)] public float explosionProbability;

    #region IDamageSource Methods
    public string GetDamageSourceName() => treatName;
    public string GetDamageSourceDescription() => description;
    public Sprite GetDamageSourceSprite() => sprite;
    public Color GetDamageSourceColor() => damageColor;
    public DamageSourceClassification GetDamageSourceClassification() => DamageSourceClassification.Treat;
    #endregion
}
