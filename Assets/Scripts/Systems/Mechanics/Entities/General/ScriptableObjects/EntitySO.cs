using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntitySO : ScriptableObject, IAttackable, IDamageSource //All entities Have Health & Movement Stats
{
    [Header("Entity Identifiers")]
    public int id;
    public string entityName;
    [TextArea(3, 10)] public string description;
    public Sprite sprite;
    [ColorUsage(true, true)] public Color color;
    [Space]
    public Transform prefab;

    [Header("Entity Health Settings")]
    [Range(0, 1000)] public int baseHealth;
    [Range(0, 20)] public int baseArmor;
    [Space]
    [Range(0, 1)] public float baseDodgeChance;

    [Header("Entity Damage Settings")]
    [Range(0, 30)] public int baseAttackDamage;
    [Range(0.1f, 3f)] public float baseAttackSpeed;
    [Space]
    [Range(0f, 1f)] public float baseAttackCritChance;
    [Range(0.5f, 2f)] public float baseAttackCritDamageMultiplier;
    [Space]
    [Range(0f, 1f)] public float baseLifesteal;
    [Space]
    [ColorUsage(true, true)] public Color damageColor;

    [Header("Entity Movement Settings")]
    [Range(0f, 10f)] public float baseMovementSpeed;
    [Range(0.5f, 10f)] public float pushResistanceFactor;

    #region IAttackableSO Methods
    public string GetAttackableName() => entityName;
    public string GetAttackableDescription() => description;
    public Sprite GetAttackableSprite() => sprite;
    #endregion

    #region IDamageSource Methods
    public string GetDamageSourceName() => entityName;
    public string GetDamageSourceDescription() => description;
    public Sprite GetDamageSourceSprite() => sprite;
    public Color GetDamageSourceColor() => damageColor;
    public abstract DamageSourceClassification GetDamageSourceClassification();
    #endregion
}
