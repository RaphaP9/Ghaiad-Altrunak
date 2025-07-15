using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActivePassiveAbilitySO : AbilitySO, IActiveAbilitySO, IPassiveAbilitySO
{
    [Header("Active Ability Settings")]
    [Range(0.5f, 100f)] public float baseCooldown;

    public float GetBaseCooldown() => baseCooldown;
    public override AbilityType GetAbilityType() => AbilityType.ActivePassive;
}
