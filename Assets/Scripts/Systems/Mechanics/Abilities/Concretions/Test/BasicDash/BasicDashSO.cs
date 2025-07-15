using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicDashSO", menuName = "ScriptableObjects/Abilities/Active/BasicDash")]
public class BasicDashSO : ActiveAbilitySO
{
    [Header("Specific Settings")]
    [Range(1f, 12f)] public float dashDistance;
    [Range(0.1f, 1f)] public float dashTime;
    [Range(0f, 50f)] public float dashResistance;
}
