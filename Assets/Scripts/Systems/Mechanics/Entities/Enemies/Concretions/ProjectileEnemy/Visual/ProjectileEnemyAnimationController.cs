using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyAnimationController : EnemyAnimationController
{
    [Header("Melee Enemy Components")]
    [SerializeField] private ProjectileEnemyAttack projectileEnemyAttack;

    protected const string AIM_BLEND_TREE_NAME = "AimBlendTree";
    protected const string SHOOT_BLEND_TREE_NAME = "ShootBlendTree";
    protected const string RELOAD_BLEND_TREE_NAME = "ReloadBlendTree";

    protected override void OnEnable()
    {
        base.OnEnable();

        projectileEnemyAttack.OnProjectileEnemyAim += ProjectileEnemyAttack_OnProjectileEnemyAim;
        projectileEnemyAttack.OnProjectileEnemyShoot += ProjectileEnemyAttack_OnProjectileEnemyShoot;
        projectileEnemyAttack.OnProjectileEnemyReload += ProjectileEnemyAttack_OnProjectileEnemyReload;

        projectileEnemyAttack.OnProjectileEnemyStopShooting += ProjectileEnemyAttack_OnProjectileEnemyStopShooting;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        projectileEnemyAttack.OnProjectileEnemyAim -= ProjectileEnemyAttack_OnProjectileEnemyAim;
        projectileEnemyAttack.OnProjectileEnemyShoot -= ProjectileEnemyAttack_OnProjectileEnemyShoot;
        projectileEnemyAttack.OnProjectileEnemyReload -= ProjectileEnemyAttack_OnProjectileEnemyReload;

        projectileEnemyAttack.OnProjectileEnemyStopShooting -= ProjectileEnemyAttack_OnProjectileEnemyStopShooting;
    }

    #region Subscriptions
    private void ProjectileEnemyAttack_OnProjectileEnemyAim(object sender, ProjectileEnemyAttack.OnProjectileEnemyAttackEventArgs e)
    {
        PlayAnimation(AIM_BLEND_TREE_NAME);
    }
    private void ProjectileEnemyAttack_OnProjectileEnemyShoot(object sender, ProjectileEnemyAttack.OnProjectileEnemyAttackEventArgs e)
    {
        PlayAnimation(SHOOT_BLEND_TREE_NAME);
    }
    private void ProjectileEnemyAttack_OnProjectileEnemyReload(object sender, ProjectileEnemyAttack.OnProjectileEnemyAttackEventArgs e)
    {
        PlayAnimation(RELOAD_BLEND_TREE_NAME);
    }

    private void ProjectileEnemyAttack_OnProjectileEnemyStopShooting(object sender, ProjectileEnemyAttack.OnProjectileEnemyAttackEventArgs e)
    {
        if (isDead) return;

        PlayAnimation(MOVEMENT_BLEND_TREE_NAME);
    }
    #endregion
}
