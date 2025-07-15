using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralEntityHealth : EntityHealth
{
    #region Events
    public static event EventHandler<OnEntityInitializedEventArgs> OnAnyNeutralEntityInitialized;
    public event EventHandler<OnEntityInitializedEventArgs> OnNeutralEntityInitialized;

    public static event EventHandler<OnEntityDodgeEventArgs> OnAnyNeutralEntityDodge;
    public event EventHandler<OnEntityDodgeEventArgs> OnNeutralEntityDodge;

    public static event EventHandler<OnEntityHealthTakeDamageEventArgs> OnAnyNeutralEntityHealthTakeDamage;
    public event EventHandler<OnEntityHealthTakeDamageEventArgs> OnNeutralEntityHealthTakeDamage;

    public static event EventHandler<OnEntityShieldTakeDamageEventArgs> OnAnyNeutralEntityShieldTakeDamage;
    public event EventHandler<OnEntityShieldTakeDamageEventArgs> OnNeutralEntityShieldTakeDamage;

    public static event EventHandler<OnEntityHealEventArgs> OnAnyNeutralEntityHeal;
    public event EventHandler<OnEntityHealEventArgs> OnNeutralEntityHeal;

    public static event EventHandler<OnEntityShieldRestoredEventArgs> OnAnyNeutralEntityShieldRestored;
    public event EventHandler<OnEntityShieldRestoredEventArgs> OnNeutralEntityShieldRestored;

    public static event EventHandler<OnEntityDeathEventArgs> OnAnyNeutralEntityDeath;
    public event EventHandler<OnEntityDeathEventArgs> OnNeutralEntityDeath;

    public static event EventHandler<OnEntityDeathEventArgs> OnAnyNeutralEntityExecuted;
    public event EventHandler<OnEntityDeathEventArgs> OnNeutralEntityExecuted;

    public static event EventHandler<OnEntityCurrentHealthClampedEventArgs> OnAnyNeutralEntityCurrentHealthClamped;
    public event EventHandler<OnEntityCurrentHealthClampedEventArgs> OnNeutralEntityCurrentHealthClamped;

    public static event EventHandler<OnEntityCurrentShieldClampedEventArgs> OnAnyNeutralEntityCurrentShieldClamped;
    public event EventHandler<OnEntityCurrentShieldClampedEventArgs> OnNeutralEntityCurrentShieldClamped;

    #endregion

    #region Virtual Event Methods
    protected override void OnEntityInitializedMethod()
    {
        base.OnEntityInitializedMethod();

        OnNeutralEntityInitialized?.Invoke(this, new OnEntityInitializedEventArgs { currentHealth = currentHealth, currentShield = currentShield });
        OnAnyNeutralEntityInitialized?.Invoke(this, new OnEntityInitializedEventArgs { currentHealth = currentHealth, currentShield = currentShield });
    }

    protected override void OnEntityDodgeMethod(DamageData damageData)
    {
        base.OnEntityDodgeMethod(damageData);

        OnNeutralEntityDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
        OnAnyNeutralEntityDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
    }

    protected override void OnEntityHealthTakeDamageMethod(int damageTakenByHealth, int rawDamage, int previousHealth, bool isCrit, IDamageSource damageSource)
    {
        base.OnEntityHealthTakeDamageMethod(damageTakenByHealth, rawDamage, previousHealth, isCrit, damageSource);

        OnNeutralEntityHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, rawDamage = rawDamage, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyNeutralEntityHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, rawDamage = rawDamage, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});
    }

    protected override void OnEntityShieldTakeDamageMethod(int damageTakenByShield, int rawDamage, int previousShield, bool isCrit, IDamageSource damageSource)
    {
        base.OnEntityShieldTakeDamageMethod(damageTakenByShield, rawDamage, previousShield, isCrit, damageSource);

        OnNeutralEntityShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, rawDamage = rawDamage, previousShield = previousShield, 
        newShield = currentShield, maxShield = entityMaxShieldStatResolver.Value, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyNeutralEntityShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, rawDamage = rawDamage, previousShield = previousShield, 
        newShield = currentShield, maxShield = entityMaxShieldStatResolver.Value, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

    }

    protected override void OnEntityHealMethod(int healAmount, int previousHealth, IHealSource healSource)
    {
        base.OnEntityHealMethod(healAmount, previousHealth, healSource);

        OnNeutralEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, healSource = healSource, healReceiver = this});
        OnAnyNeutralEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, healSource = healSource, healReceiver = this});
    }

    protected override void OnEntityShieldRestoredMethod(int shieldAmount, int previousShield, IShieldSource shieldSource)
    {
        base.OnEntityShieldRestoredMethod(shieldAmount, previousShield, shieldSource);

        OnNeutralEntityShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = entityMaxShieldStatResolver.Value, shieldSource = shieldSource, shieldReceiver = this });
        OnAnyNeutralEntityShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = entityMaxShieldStatResolver.Value, shieldSource = shieldSource, shieldReceiver = this });
    }

    protected override void OnEntityDeathMethod(EntitySO entitySO, IDamageSource damageSource)
    {
        base.OnEntityDeathMethod(entitySO, damageSource);

        OnNeutralEntityDeath?.Invoke(this, new OnEntityDeathEventArgs { entitySO = entitySO as NeutralEntitySO, damageSource = damageSource });
        OnAnyNeutralEntityDeath?.Invoke(this, new OnEntityDeathEventArgs { entitySO = entitySO as NeutralEntitySO, damageSource = damageSource });
    }
    protected override void OnEntityExecutedMethod(EntitySO entitySO, IDamageSource damageSource)
    {
        base.OnEntityExecutedMethod(entitySO, damageSource);

        OnNeutralEntityExecuted?.Invoke(this, new OnEntityDeathEventArgs { entitySO = entitySO as AllySO, damageSource = damageSource });
        OnAnyNeutralEntityExecuted?.Invoke(this, new OnEntityDeathEventArgs { entitySO = entitySO as AllySO, damageSource = damageSource });
    }

    protected override void OnEntityCurrentHealthClampedMethod()
    {
        base.OnEntityCurrentHealthClampedMethod();

        OnNeutralEntityCurrentHealthClamped?.Invoke(this, new OnEntityCurrentHealthClampedEventArgs { currentHealth = currentHealth , maxHealth = entityMaxHealthStatResolver.Value });
        OnAnyNeutralEntityCurrentHealthClamped?.Invoke(this, new OnEntityCurrentHealthClampedEventArgs { currentHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value });
    }

    protected override void OnEntityCurrentShieldClampedMethod()
    {
        base.OnEntityCurrentShieldClampedMethod();

        OnNeutralEntityCurrentShieldClamped?.Invoke(this, new OnEntityCurrentShieldClampedEventArgs { currentShield = currentShield, maxShield = entityMaxShieldStatResolver.Value });
        OnAnyNeutralEntityCurrentShieldClamped?.Invoke(this, new OnEntityCurrentShieldClampedEventArgs { currentShield = currentShield, maxShield = entityMaxShieldStatResolver.Value });
    }
    #endregion
}