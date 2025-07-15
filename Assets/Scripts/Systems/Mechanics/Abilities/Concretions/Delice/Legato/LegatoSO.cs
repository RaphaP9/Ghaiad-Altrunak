using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LegatoSO", menuName = "ScriptableObjects/Abilities/Delice/Legato")]
public class LegatoSO : ActiveAbilitySO, IDamageSource, IPushSource, IHasEmbeddedNumericStats
{
    [Header("Specific Settings")]
    [Range(0f, 1f)] public float flyStartDuration;
    [Range(1f, 5f)] public float flyDuration;
    [Range(0f, 1f)] public float flyEndDuration;
    [Range(0f, 1f)] public float dodgeTimeAfterLand;
    [Space]
    [ColorUsage(true, true)] public Color damageColor;
    [Space]
    [Range(1f, 8f)] public float actionRadius;
    [Range(5, 10)] public int landDamage;
    [Range(1f, 100f)] public float pushForce;

    [Header("Stats")]
    public string refferencialGUID;
    public List<NumericEmbeddedStat> numericEmbeddedStats;

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

    public List<NumericEmbeddedStat> GetNumericEmbeddedStats() => numericEmbeddedStats;
}
