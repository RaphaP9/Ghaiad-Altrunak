using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : PlayerAttack
{
    [Header("Player Melee Attack Components")]
    [SerializeField] protected List<Transform> attackPoints;

    [Header("Player Melee Attack Settings")]
    [SerializeField, Range(0.1f, 3f)] protected float attackAreaRadius;


    protected override void Attack()
    {
        bool isCrit = MechanicsUtilities.EvaluateCritAttack(entityAttackCritChanceStatResolver.Value, IsOverridingCrit());
        int damage = isCrit ? MechanicsUtilities.CalculateCritDamage(entityAttackDamageStatResolver.Value, entityAttackCritDamageMultiplierStatResolver.Value) : entityAttackDamageStatResolver.Value;

        List<Vector2> positions = GeneralUtilities.TransformPositionVector2List(attackPoints);

        DamageData damageData = new DamageData(damage, isCrit, characterIdentifier.CharacterSO, true, true, true, true);
        MechanicsUtilities.DealDamageInAreas(positions, attackAreaRadius, damageData, attackLayermask, new List<Transform> {transform});

        OnEntityAttackMethod(isCrit, damage);
        OnEntityAttackCompletedMethod();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        foreach (Transform attackPoint in attackPoints)
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackAreaRadius);
        }
    }
}
