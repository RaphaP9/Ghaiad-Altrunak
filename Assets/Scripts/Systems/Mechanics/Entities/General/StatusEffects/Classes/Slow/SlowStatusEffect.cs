using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlowStatusEffect : EntityStatusEffect
{
    [Header("Slow Settings")]
    [Range(0.1f, 0.9f)] public float slowPercentage;

    public override EntityStatusEffectType GetEntityStatusEffectType() => EntityStatusEffectType.Slow;
}
