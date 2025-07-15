using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyHealth : EntityHealth
{
    #region Events
    public static event EventHandler<OnEntityInitializedEventArgs> OnAnyAllyInitialized;
    public event EventHandler<OnEntityInitializedEventArgs> OnAllyInitialized;

    public static event EventHandler<OnEntityDodgeEventArgs> OnAnyAllyDodge;
    public event EventHandler<OnEntityDodgeEventArgs> OnAllyDodge;

    public static event EventHandler<OnEntityHealthTakeDamageEventArgs> OnAnyAllyHealthTakeDamage;
    public event EventHandler<OnEntityHealthTakeDamageEventArgs> OnAllyHealthTakeDamage;

    public static event EventHandler<OnEntityShieldTakeDamageEventArgs> OnAnyAllyShieldTakeDamage;
    public event EventHandler<OnEntityShieldTakeDamageEventArgs> OnAllyShieldTakeDamage;

    public static event EventHandler<OnEntityHealEventArgs> OnAnyAllyHeal;
    public event EventHandler<OnEntityHealEventArgs> OnAllyHeal;

    public static event EventHandler<OnEntityShieldRestoredEventArgs> OnAnyAllyShieldRestored;
    public event EventHandler<OnEntityShieldRestoredEventArgs> OnAllyShieldRestored;

    public static event EventHandler<OnEntityDeathEventArgs> OnAnyAllyDeath;
    public event EventHandler<OnEntityDeathEventArgs> OnAllyDeath;

    public static event EventHandler<OnEntityDeathEventArgs> OnAnyAllyExecuted;
    public event EventHandler<OnEntityDeathEventArgs> OnAllyExecuted;

    public static event EventHandler<OnEntityCurrentHealthClampedEventArgs> OnAnyAllyCurrentHealthClamped;
    public event EventHandler<OnEntityCurrentHealthClampedEventArgs> OnAllyCurrentHealthClamped;

    public static event EventHandler<OnEntityCurrentShieldClampedEventArgs> OnAnyAllyCurrentShieldClamped;
    public event EventHandler<OnEntityCurrentShieldClampedEventArgs> OnAllyCurrentShieldClamped;

    #endregion

    #region Virtual Event Methods
    protected override void OnEntityInitializedMethod()
    {
        base.OnEntityInitializedMethod();

        OnAllyInitialized?.Invoke(this, new OnEntityInitializedEventArgs { currentHealth = currentHealth, currentShield = currentShield });
        OnAnyAllyInitialized?.Invoke(this, new OnEntityInitializedEventArgs { currentHealth = currentHealth, currentShield = currentShield });
    }

    protected override void OnEntityDodgeMethod(DamageData damageData)
    {
        base.OnEntityDodgeMethod(damageData);

        OnAllyDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
        OnAnyAllyDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
    }

    protected override void OnEntityHealthTakeDamageMethod(int damageTakenByHealth, int rawDamage, int previousHealth, bool isCrit, IDamageSource damageSource)
    {
        base.OnEntityHealthTakeDamageMethod(damageTakenByHealth, rawDamage, previousHealth, isCrit, damageSource);

        OnAllyHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs
        {
            damageTakenByHealth = damageTakenByHealth,
            rawDamage = rawDamage,
            previousHealth = previousHealth,
            newHealth = currentHealth,
            maxHealth = entityMaxHealthStatResolver.Value,
            isCrit = isCrit,
            damageSource = damageSource,
            damageReceiver = this
        });

        OnAnyAllyHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs
        {
            damageTakenByHealth = damageTakenByHealth,
            rawDamage = rawDamage,
            previousHealth = previousHealth,
            newHealth = currentHealth,
            maxHealth = entityMaxHealthStatResolver.Value,
            isCrit = isCrit,
            damageSource = damageSource,
            damageReceiver = this
        });
    }

    protected override void OnEntityShieldTakeDamageMethod(int damageTakenByShield, int rawDamage, int previousShield, bool isCrit, IDamageSource damageSource)
    {
        base.OnEntityShieldTakeDamageMethod(damageTakenByShield, rawDamage, previousShield, isCrit, damageSource);

        OnAllyShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs
        {
            damageTakenByShield = damageTakenByShield,
            rawDamage = rawDamage,
            previousShield = previousShield,
            newShield = currentShield,
            maxShield = entityMaxShieldStatResolver.Value,
            isCrit = isCrit,
            damageSource = damageSource,
            damageReceiver = this
        });

        OnAnyAllyShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs
        {
            damageTakenByShield = damageTakenByShield,
            rawDamage = rawDamage,
            previousShield = previousShield,
            newShield = currentShield,
            maxShield = entityMaxShieldStatResolver.Value,
            isCrit = isCrit,
            damageSource = damageSource,
            damageReceiver = this
        });

    }

    protected override void OnEntityHealMethod(int healAmount, int previousHealth, IHealSource healSource)
    {
        base.OnEntityHealMethod(healAmount, previousHealth, healSource);

        OnAllyHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, healSource = healSource, healReceiver = this });
        OnAnyAllyHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, healSource = healSource, healReceiver = this });
    }

    protected override void OnEntityShieldRestoredMethod(int shieldAmount, int previousShield, IShieldSource shieldSource)
    {
        base.OnEntityShieldRestoredMethod(shieldAmount, previousShield, shieldSource);

        OnAllyShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = entityMaxShieldStatResolver.Value, shieldSource = shieldSource, shieldReceiver = this });
        OnAnyAllyShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = entityMaxShieldStatResolver.Value, shieldSource = shieldSource, shieldReceiver = this });
    }

    protected override void OnEntityDeathMethod(EntitySO entitySO, IDamageSource damageSource)
    {
        base.OnEntityDeathMethod(entitySO, damageSource);

        OnAllyDeath?.Invoke(this, new OnEntityDeathEventArgs { entitySO = entitySO as AllySO, damageSource = damageSource });
        OnAnyAllyDeath?.Invoke(this, new OnEntityDeathEventArgs { entitySO = entitySO as AllySO, damageSource = damageSource });
    }

    protected override void OnEntityExecutedMethod(EntitySO entitySO, IDamageSource damageSource)
    {
        base.OnEntityExecutedMethod(entitySO, damageSource);

        OnAllyExecuted?.Invoke(this, new OnEntityDeathEventArgs { entitySO = entitySO as AllySO, damageSource = damageSource });
        OnAnyAllyExecuted?.Invoke(this, new OnEntityDeathEventArgs { entitySO = entitySO as AllySO, damageSource = damageSource });
    }

    protected override void OnEntityCurrentHealthClampedMethod()
    {
        base.OnEntityCurrentHealthClampedMethod();

        OnAllyCurrentHealthClamped?.Invoke(this, new OnEntityCurrentHealthClampedEventArgs { currentHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value });
        OnAnyAllyCurrentHealthClamped?.Invoke(this, new OnEntityCurrentHealthClampedEventArgs { currentHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value });
    }

    protected override void OnEntityCurrentShieldClampedMethod()
    {
        base.OnEntityCurrentShieldClampedMethod();

        OnAllyCurrentShieldClamped?.Invoke(this, new OnEntityCurrentShieldClampedEventArgs { currentShield = currentShield, maxShield = entityMaxShieldStatResolver.Value });
        OnAnyAllyCurrentShieldClamped?.Invoke(this, new OnEntityCurrentShieldClampedEventArgs { currentShield = currentShield, maxShield = entityMaxShieldStatResolver.Value });
    }
    #endregion
}
