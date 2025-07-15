using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttack : EntityAttack
{
    [Header("Enemy Attack Components")]
    [SerializeField] private EnemyIdentifier enemyIdentifier;

    protected EnemySO EnemySO => enemyIdentifier.EnemySO;

    protected float timer;
    protected bool shouldAttack = false;
    protected bool shouldStopAttack = false;

    protected bool hasExecutedAttack = false;

    #region Events
    public event EventHandler<OnEnemyAttackEventArgs> OnEnemyAttack;
    public static event EventHandler<OnEnemyAttackEventArgs> OnAnyEnemyAttack;

    public event EventHandler<OnEnemyAttackCompletedEventArgs> OnEnemyAttackCompleted;
    public static event EventHandler<OnEnemyAttackCompletedEventArgs> OnAnyEnemyAttackCompleted;
    #endregion

    #region EventArgs Classes
    public class OnEnemyAttackEventArgs : OnEntityAttackEventArgs
    {
        public EnemySO enemySO;
    }

    public class OnEnemyAttackCompletedEventArgs : OnEntityAttackCompletedEventArgs
    {
        public EnemySO enemySO;
    }
    #endregion

    public void TriggerAttack() => shouldAttack = true;
    public void TriggerAttackStop() => shouldStopAttack = true;

    protected void ResetTimer() => timer = 0f;
    public abstract bool OnAttackExecution();

    #region Virtual Event Methods
    protected override void OnEntityAttackMethod(bool isCrit, int attackDamage)
    {
        base.OnEntityAttackMethod(isCrit, attackDamage);

        OnEnemyAttack?.Invoke(this, new OnEnemyAttackEventArgs { enemySO = enemyIdentifier.EnemySO, isCrit = isCrit, attackDamage = attackDamage, attackSpeed = entityAttackSpeedStatResolver.Value, attackCritChance = entityAttackCritChanceStatResolver.Value, attackCritDamageMultiplier = entityAttackCritDamageMultiplierStatResolver.Value });
        OnAnyEnemyAttack?.Invoke(this, new OnEnemyAttackEventArgs { enemySO = enemyIdentifier.EnemySO, attackDamage = attackDamage, attackSpeed = entityAttackSpeedStatResolver.Value, attackCritChance = entityAttackCritChanceStatResolver.Value, attackCritDamageMultiplier = entityAttackCritDamageMultiplierStatResolver.Value });
    }

    protected override void OnEntityAttackCompletedMethod()
    {
        base.OnEntityAttackCompletedMethod();

        OnEnemyAttackCompleted?.Invoke(this, new OnEnemyAttackCompletedEventArgs { enemySO = enemyIdentifier.EnemySO });
        OnAnyEnemyAttackCompleted?.Invoke(this, new OnEnemyAttackCompletedEventArgs { enemySO = enemyIdentifier.EnemySO });
    }
    #endregion
}
