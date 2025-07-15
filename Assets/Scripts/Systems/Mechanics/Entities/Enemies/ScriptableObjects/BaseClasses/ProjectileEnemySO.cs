using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectileEnemySO", menuName = "ScriptableObjects/Entities/Enemies/ProjectileEnemy")]
public class ProjectileEnemySO : EnemySO
{
    [Header("Projectile Enemy Settings")]
    [Range(3f, 20f)] public float tooCloseDistance;
    [Range(3f, 20f)] public float preferredDistance;
    [Range(3f, 20f)] public float tooFarDistance;
    [Space]
    //RULE: chargingTimeMult + attackingTimeMult + postAttackMult = 1, Although is later normalized
    [Range(0.01f, 1f)] public float aimingTimeMult; //Charging Time = chargingTimeMult * 1/ AttackSpeed
    [Range(0.01f, 1f)] public float shootingTimeMult; //Attacking Time = attackingTimeMult * 1/ AttackSpeed
    [Range(0.01f, 1f)] public float reloadTimeMult; //PostAttack Time = postAttackTimeMult * 1/ AttackSpeed
    [Space]
    [Range(0f, 1f)] public float attackExecutionTimeMult; // Attack Execution on the middle of attack performance

    private float GetTotalMult() => aimingTimeMult + shootingTimeMult + reloadTimeMult;

    public float GetNormalizedAimingMult() => aimingTimeMult / GetTotalMult();
    public float GetNormalizedShootingMult() => shootingTimeMult / GetTotalMult();
    public float GetNormalizedReloadMult() => reloadTimeMult / GetTotalMult();
}
