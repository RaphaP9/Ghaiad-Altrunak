using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterSO", menuName = "ScriptableObjects/Entities/Characters/Character(Default)")]
public class CharacterSO : EntitySO, IHealSource, IShieldSource
{
    [Header("Attack Settings")]
    [Range(0.5f, 2f)] public float attackDamagePercentage;

    [Header("Character Health Settings")]
    [Range(0, 10)] public int baseHealthRegen;
    [Range(0, 10)] public int baseShieldRegen;

    [Header("Character Cooldown Settings")]
    [Range(0f, 1f)] public float baseCooldownReduction;

    [Header("Inventories")]
    [Range(0, 1000)] public int objectsInventorySize;
    [Range(0, 1000)] public int treatsInventorySize;

    public override DamageSourceClassification GetDamageSourceClassification() => DamageSourceClassification.Character;

    #region IHealSource Methods
    public string GetHealSourceName() => entityName;
    public string GetHealSourceDescription() => description;
    public Sprite GetHealSourceSprite() => sprite;
    #endregion

    #region IShieldSource Methods
    public string GetShieldSourceName() => entityName;
    public string GetShieldSourceDescription() => description;
    public Sprite GetShieldSourceSprite() => sprite;
    #endregion
}
