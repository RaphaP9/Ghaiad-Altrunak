using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyExplosion : EntityExplosion
{
    [Header("Enemy Explosion Components")]
    [SerializeField] private EnemyIdentifier enemyIdentifier;

    protected EnemySO EnemySO => enemyIdentifier.EnemySO;

    protected float timer;
    protected bool shouldExplode = false;
    protected bool shouldStopExplosion = false;


    #region Events
    public event EventHandler<OnEnemyExplosionEventArgs> OnEnemyExplosion;
    public static event EventHandler<OnEnemyExplosionEventArgs> OnAnyEnemyExplosion;
    #endregion

    #region EventArgs Classes
    public class OnEnemyExplosionEventArgs : OnEntityExplosionEventArgs
    {
        public EnemySO enemySO;
    }
    #endregion

    public void TriggerExplosion() => shouldExplode = true;
    public void TriggerExplosionStop() => shouldStopExplosion = true;

    protected void ResetTimer() => timer = 0f;

    #region Virtual Event Methods
    protected override void OnEntityExplosionMethod(int explosionDamage, float explosionRadius, Vector2 position)
    {
        base.OnEntityExplosionMethod(explosionDamage, explosionRadius, position);

        OnEnemyExplosion?.Invoke(this, new OnEnemyExplosionEventArgs { explosionPoints = explosionPoints, explosionDamage = explosionDamage, explosionRadius = explosionRadius, position = position, enemySO = EnemySO });
        OnAnyEnemyExplosion?.Invoke(this, new OnEnemyExplosionEventArgs { explosionPoints = explosionPoints, explosionDamage = explosionDamage, explosionRadius = explosionRadius, position = position, enemySO = EnemySO });
    }
    #endregion
}
