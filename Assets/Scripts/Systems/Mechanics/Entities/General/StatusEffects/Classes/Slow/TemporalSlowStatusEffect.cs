using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TemporalSlowStatusEffect : SlowStatusEffect
{
    [Header("Temporal Settings")]
    [Range(0.5f,5f)] public float duration;
}
