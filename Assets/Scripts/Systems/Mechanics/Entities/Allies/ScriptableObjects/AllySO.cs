using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAllySO", menuName = "ScriptableObjects/Entities/Allies/Ally(Default)")]
public class AllySO : EntitySO
{
    [Header("Ally Extra Settings")]
    [Range(1f, 5f)] public float spawnDuration;
    [Range(1f, 10f)] public float cleanupTime;

    public override DamageSourceClassification GetDamageSourceClassification() => DamageSourceClassification.Ally;
}
