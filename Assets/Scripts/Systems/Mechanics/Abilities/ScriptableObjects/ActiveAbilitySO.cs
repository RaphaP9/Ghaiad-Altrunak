using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveAbilitySO : AbilitySO, IActiveAbilitySO
{
    [Header("Active Ability Settings")]
    [Range(0.5f, 100f)] public float baseCooldown;

    public float GetBaseCooldown() => baseCooldown;
    public override AbilityType GetAbilityType() => AbilityType.Active;
}
