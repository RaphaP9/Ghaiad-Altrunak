using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RitardandoSO", menuName = "ScriptableObjects/Abilities/Delice/Ritardando")]
public class RitardandoSO : ActiveAbilitySO, IDamageSource, IPushSource
{
    [Header("Specific Settings")]
    [Range(1, 20)] public int damage;
    [ColorUsage(true, true)] public Color damageColor;
    [Space]
    public TemporalSlowStatusEffect tenporalSlowStatusEffect;
    [Space]
    [Range(0f, 1f)] public float performanceTime;
    [Space]
    [Range(1f, 100f)] public float pushForce;

    #region Damage Source Methods
    public DamageSourceClassification GetDamageSourceClassification() => DamageSourceClassification.Character;
    public Color GetDamageSourceColor() => damageColor;
    public string GetDamageSourceDescription() => description;
    public string GetDamageSourceName() => abilityName;
    public Sprite GetDamageSourceSprite() => sprite;
    #endregion

    #region Push Source Methods
    public string GetPushSourceName() => abilityName;
    public string GetPushSourceDescription() => description;
    public Sprite GetPushSourceSprite() => sprite;
    #endregion
}
