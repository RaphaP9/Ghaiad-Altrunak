using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : EntityHealth
{
    [Header("Enemy Health Components")]
    [SerializeField] protected EnemySpawnHandler enemySpawnHandler;

    #region Events
    public static event EventHandler<OnEntityInitializedEventArgs> OnAnyEnemyInitialized;
    public event EventHandler<OnEntityInitializedEventArgs> OnEnemyInitialized;

    public static event EventHandler<OnEntityDodgeEventArgs> OnAnyEnemyDodge;
    public event EventHandler<OnEntityDodgeEventArgs> OnEnemyDodge;

    public static event EventHandler<OnEntityHealthTakeDamageEventArgs> OnAnyEnemyHealthTakeDamage;
    public event EventHandler<OnEntityHealthTakeDamageEventArgs> OnEnemyHealthTakeDamage;

    public static event EventHandler<OnEntityHealEventArgs> OnAnyEnemyHeal;
    public event EventHandler<OnEntityHealEventArgs> OnEnemyHeal;

    public static event EventHandler<OnEntityDeathEventArgs> OnAnyEnemyDeath;
    public event EventHandler<OnEntityDeathEventArgs> OnEnemyDeath;

    public static event EventHandler<OnEntityDeathEventArgs> OnAnyEnemyExecuted;
    public event EventHandler<OnEntityDeathEventArgs> OnEnemyExecuted;

    public static event EventHandler<OnEntityCurrentHealthClampedEventArgs> OnAnyEnemyCurrentHealthClamped;
    public event EventHandler<OnEntityCurrentHealthClampedEventArgs> OnEnemyCurrentHealthClamped;
    #endregion

    public override bool AvoidDamagePassThrough()
    {
        if (base.AvoidDamagePassThrough()) return true;
        if (enemySpawnHandler.IsSpawning) return true;

        return false;
    }

    #region Virtual Event Methods
    protected override void OnEntityInitializedMethod()
    {
        base.OnEntityInitializedMethod();

        OnEnemyInitialized?.Invoke(this, new OnEntityInitializedEventArgs { currentHealth = currentHealth});
        OnAnyEnemyInitialized?.Invoke(this, new OnEntityInitializedEventArgs { currentHealth = currentHealth});
    }

    protected override void OnEntityDodgeMethod(DamageData damageData)
    {
        base.OnEntityDodgeMethod(damageData);

        OnEnemyDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
        OnAnyEnemyDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
    }

    protected override void OnEntityHealthTakeDamageMethod(int damageTakenByHealth, int rawDamage, int previousHealth, bool isCrit, IDamageSource damageSource)
    {
        base.OnEntityHealthTakeDamageMethod(damageTakenByHealth, rawDamage, previousHealth, isCrit, damageSource);

        OnEnemyHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, rawDamage = rawDamage, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyEnemyHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, rawDamage = rawDamage, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});
    }

    protected override void OnEntityHealMethod(int healAmount, int previousHealth, IHealSource healSource)
    {
        base.OnEntityHealMethod(healAmount, previousHealth, healSource);

        OnEnemyHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, healSource = healSource, healReceiver = this});
        OnAnyEnemyHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, healSource = healSource, healReceiver = this});
    }

    protected override void OnEntityDeathMethod(EntitySO entitySO, IDamageSource damageSource)
    {
        base.OnEntityDeathMethod(entitySO, damageSource);

        OnEnemyDeath?.Invoke(this, new OnEntityDeathEventArgs { entitySO = entitySO as EnemySO, damageSource = damageSource });
        OnAnyEnemyDeath?.Invoke(this, new OnEntityDeathEventArgs { entitySO = entitySO as EnemySO, damageSource = damageSource });
    }

    protected override void OnEntityExecutedMethod(EntitySO entitySO, IDamageSource damageSource)
    {
        base.OnEntityExecutedMethod(entitySO, damageSource);

        OnEnemyExecuted?.Invoke(this, new OnEntityDeathEventArgs { entitySO = entitySO as AllySO, damageSource = damageSource });
        OnAnyEnemyExecuted?.Invoke(this, new OnEntityDeathEventArgs { entitySO = entitySO as AllySO, damageSource = damageSource });
    }

    protected override void OnEntityCurrentHealthClampedMethod()
    {
        base.OnEntityCurrentHealthClampedMethod();

        OnEnemyCurrentHealthClamped?.Invoke(this, new OnEntityCurrentHealthClampedEventArgs { currentHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value });
        OnAnyEnemyCurrentHealthClamped?.Invoke(this, new OnEntityCurrentHealthClampedEventArgs { currentHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value });
    }
    #endregion
}
