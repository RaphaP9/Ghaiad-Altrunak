using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAnimationController : EnemyAnimationController
{
    [Header("Melee Enemy Components")]
    [SerializeField] private MeleeEnemyAttack meleeEnemyAttack;

    protected const string CHARGE_BLEND_TREE_NAME = "ChargeBlendTree";
    protected const string ATTACK_BLEND_TREE_NAME = "AttackBlendTree";
    protected const string RECOVER_BLEND_TREE_NAME = "RecoverBlendTree";

    protected override void OnEnable()
    {
        base.OnEnable();

        meleeEnemyAttack.OnMeleeEnemyCharge += MeleeEnemyAttack_OnMeleeEnemyCharge;
        meleeEnemyAttack.OnMeleeEnemyAttack += MeleeEnemyAttack_OnMeleeEnemyAttack;
        meleeEnemyAttack.OnMeleeEnemyRecover += MeleeEnemyAttack_OnMeleeEnemyRecover;

        meleeEnemyAttack.OnMeleeEnemyStopAttacking += MeleeEnemyAttack_OnMeleeEnemyStopAttacking;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        meleeEnemyAttack.OnMeleeEnemyCharge -= MeleeEnemyAttack_OnMeleeEnemyCharge;
        meleeEnemyAttack.OnMeleeEnemyAttack -= MeleeEnemyAttack_OnMeleeEnemyAttack;
        meleeEnemyAttack.OnMeleeEnemyRecover -= MeleeEnemyAttack_OnMeleeEnemyRecover;

        meleeEnemyAttack.OnMeleeEnemyStopAttacking -= MeleeEnemyAttack_OnMeleeEnemyStopAttacking;
    }

    #region Subscriptions
    private void MeleeEnemyAttack_OnMeleeEnemyCharge(object sender, MeleeEnemyAttack.OnMeleeEnemyAttackEventArgs e)
    {
        PlayAnimation(CHARGE_BLEND_TREE_NAME);
    }

    private void MeleeEnemyAttack_OnMeleeEnemyAttack(object sender, MeleeEnemyAttack.OnMeleeEnemyAttackEventArgs e)
    {
        PlayAnimation(ATTACK_BLEND_TREE_NAME);
    }

    private void MeleeEnemyAttack_OnMeleeEnemyRecover(object sender, MeleeEnemyAttack.OnMeleeEnemyAttackEventArgs e)
    {
        PlayAnimation(RECOVER_BLEND_TREE_NAME);
    }

    private void MeleeEnemyAttack_OnMeleeEnemyStopAttacking(object sender, MeleeEnemyAttack.OnMeleeEnemyAttackEventArgs e)
    {
        if (isDead) return;

        PlayAnimation(MOVEMENT_BLEND_TREE_NAME);
    }
    #endregion
}
