using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKamikazeEnemySO", menuName = "ScriptableObjects/Entities/Enemies/KamikazeEnemy")]
public class KamikazeEnemySO : EnemySO
{
    [Header("Kamikaze Enemy Settings")]
    [Range(0.5f, 10f)] public int explosionDamage;
    [Range(0.5f, 5f)] public float explosionRadius;
    [Space]
    [Range(0.5f, 5f)] public float detectionRange;
    [Range(0.5f, 5f)] public float explosionTime;
}
