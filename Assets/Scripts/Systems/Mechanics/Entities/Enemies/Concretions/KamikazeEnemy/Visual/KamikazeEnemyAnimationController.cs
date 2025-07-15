using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemyAnimationController : EnemyAnimationController
{
    [Header("Melee Enemy Components")]
    [SerializeField] private KamikazeEnemyExplosion kamikazeEnemyExplosion;

    protected const string CHARGE_ANIMATION_NAME = "Charge";
    protected const string EXPLOSION_ANIMATION_NAME = "Explode";

    protected bool hasExploded = false;

    protected override void OnEnable()
    {
        base.OnEnable();

        kamikazeEnemyExplosion.OnEntityExplosionCompleted += KamikazeEnemyExplosion_OnEntityExplosionCompleted;
        kamikazeEnemyExplosion.OnKamikazeEnemyCharge += KamikazeEnemyExplosion_OnKamikazeEnemyCharge;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        kamikazeEnemyExplosion.OnEntityExplosionCompleted -= KamikazeEnemyExplosion_OnEntityExplosionCompleted;
        kamikazeEnemyExplosion.OnKamikazeEnemyCharge -= KamikazeEnemyExplosion_OnKamikazeEnemyCharge;
    }

    #region Subscriptions
    private void KamikazeEnemyExplosion_OnKamikazeEnemyCharge(object sender, KamikazeEnemyExplosion.OnKamikazeEnemyExplosionEventArgs e)
    {
        PlayAnimation(CHARGE_ANIMATION_NAME);
    }

    //On Regular Death, OnEnemyNormalKill, OnEnemyKillWhileExploding
    //1st, -, -
    private void KamikazeEnemyExplosion_OnEntityExplosionCompleted(object sender, EntityExplosion.OnEntityExplosionCompletedEventArgs e)
    {
        hasExploded = true;
        PlayAnimation(EXPLOSION_ANIMATION_NAME);
    }

    //2nd, 1st, 1st
    protected override void EnemyHealth_OnEnemyDeath(object sender, System.EventArgs e)
    {
        if (hasExploded) return;
        base.EnemyHealth_OnEnemyDeath(sender, e);
    }
    #endregion
}
