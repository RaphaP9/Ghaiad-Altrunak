using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeEnemySO", menuName = "ScriptableObjects/Entities/Enemies/MeleeEnemy")]
public class MeleeEnemySO : EnemySO
{
    [Header("Melee Enemy Settings")]
    [Range(0.5f, 5f)] public float attackDistance;
    [Space]
    [Range(0.5f, 3f)] public float attackArea;
    [Space]
    //RULE: chargingTimeMult + attackingTimeMult + postAttackMult = 1, Although is later normalized
    [Range(0.01f, 1f)] public float chargingTimeMult; //Charging Time = chargingTimeMult * 1/ AttackSpeed
    [Range(0.01f, 1f)] public float attackingTimeMult; //Attacking Time = attackingTimeMult * 1/ AttackSpeed
    [Range(0.01f, 1f)] public float recoverTimeMult; //PostAttack Time = postAttackTimeMult * 1/ AttackSpeed
    [Space]
    [Range(0f, 1f)] public float attackExecutionTimeMult; // Attack Execution on the middle of attack performance

    private float GetTotalMult() => chargingTimeMult + attackingTimeMult + recoverTimeMult;

    public float GetNormalizedChargingMult() => chargingTimeMult / GetTotalMult();
    public float GetNormalizedAttackingMult() => attackingTimeMult / GetTotalMult();
    public float GetNormalizedRecoverMult() => recoverTimeMult / GetTotalMult();
}
