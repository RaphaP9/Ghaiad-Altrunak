using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : EntityHealth
{
    #region Events
    public static event EventHandler<OnEntityInitializedEventArgs> OnAnyPlayerInitialized;
    public event EventHandler<OnEntityInitializedEventArgs> OnPlayerInitialized;

    public static event EventHandler<OnEntityDodgeEventArgs> OnAnyPlayerDodge;
    public event EventHandler<OnEntityDodgeEventArgs> OnPlayerDodge;

    public static event EventHandler<OnEntityHealthTakeDamageEventArgs> OnAnyPlayerHealthTakeDamage;
    public event EventHandler<OnEntityHealthTakeDamageEventArgs> OnPlayerHealthTakeDamage;

    public static event EventHandler<OnEntityShieldTakeDamageEventArgs> OnAnyPlayerShieldTakeDamage;
    public event EventHandler<OnEntityShieldTakeDamageEventArgs> OnPlayerShieldTakeDamage;

    public static event EventHandler<OnEntityHealEventArgs> OnAnyPlayerEntityHeal;
    public event EventHandler<OnEntityHealEventArgs> OnPlayerHeal;

    public static event EventHandler<OnEntityShieldRestoredEventArgs> OnAnyPlayerShieldRestored;
    public event EventHandler<OnEntityShieldRestoredEventArgs> OnPlayerShieldRestored;

    public static event EventHandler<OnEntityDeathEventArgs> OnAnyPlayerDeath;
    public event EventHandler<OnEntityDeathEventArgs> OnPlayerDeath;

    public static event EventHandler<OnEntityDeathEventArgs> OnAnyPlayerExecuted;
    public event EventHandler<OnEntityDeathEventArgs> OnPlayerExecuted;

    public static event EventHandler<OnEntityCurrentHealthClampedEventArgs> OnAnyPlayerCurrentHealthClamped;
    public event EventHandler<OnEntityCurrentHealthClampedEventArgs> OnPlayerCurrentHealthClamped;

    public static event EventHandler<OnEntityCurrentShieldClampedEventArgs> OnAnyPlayerCurrentShieldClamped;
    public event EventHandler<OnEntityCurrentShieldClampedEventArgs> OnPlayerCurrentShieldClamped;
    #endregion

    public void SetCurrentHealth(int setterHealth) => currentHealth = setterHealth;
    public void SetCurrentShield(int setterShield) => currentShield = setterShield;

    public override bool AvoidDamagePassThrough()
    {
        if (base.AvoidDamagePassThrough()) return true;
        if (GameManager.Instance.GameState != GameManager.State.Combat) return true; //Only Take Damage While On Combat

        return false;
    }

    #region Virtual Event Methods

    protected override void OnEntityInitializedMethod()
    {
        base.OnEntityInitializedMethod();

        OnPlayerInitialized?.Invoke(this, new OnEntityInitializedEventArgs { currentHealth = currentHealth, currentShield = currentShield });
        OnAnyPlayerInitialized?.Invoke(this, new OnEntityInitializedEventArgs { currentHealth = currentHealth, currentShield = currentShield });
    }

    protected override void OnEntityDodgeMethod(DamageData damageData)
    {
        base.OnEntityDodgeMethod(damageData);

        OnPlayerDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
        OnAnyPlayerDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
    }

    protected override void OnEntityHealthTakeDamageMethod(int damageTakenByHealth, int rawDamage, int previousHealth, bool isCrit, IDamageSource damageSource)
    {
        base.OnEntityHealthTakeDamageMethod(damageTakenByHealth, rawDamage, previousHealth, isCrit, damageSource);

        OnPlayerHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, rawDamage = rawDamage, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyPlayerHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth,rawDamage = rawDamage, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});
    }

    protected override void OnEntityShieldTakeDamageMethod(int damageTakenByShield,int rawDamage, int previousShield, bool isCrit, IDamageSource damageSource)
    {
        base.OnEntityShieldTakeDamageMethod(damageTakenByShield, rawDamage, previousShield, isCrit, damageSource);

        OnPlayerShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, rawDamage=rawDamage, previousShield = previousShield, 
        newShield = currentShield, maxShield = entityMaxShieldStatResolver.Value, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyPlayerShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, rawDamage=rawDamage, previousShield = previousShield, 
        newShield = currentShield, maxShield = entityMaxShieldStatResolver.Value, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

    }

    protected override void OnEntityHealMethod(int healAmount, int previousHealth, IHealSource healSource)
    {
        base.OnEntityHealMethod(healAmount, previousHealth, healSource);

        OnPlayerHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, healSource = healSource, healReceiver = this});
        OnAnyPlayerEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, healSource = healSource, healReceiver = this});
    }

    protected override void OnEntityShieldRestoredMethod(int shieldAmount, int previousShield, IShieldSource shieldSource)
    {
        base.OnEntityShieldRestoredMethod(shieldAmount, previousShield, shieldSource);

        OnPlayerShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = entityMaxShieldStatResolver.Value, shieldSource = shieldSource, shieldReceiver = this });
        OnAnyPlayerShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = entityMaxShieldStatResolver.Value, shieldSource = shieldSource, shieldReceiver = this });
    }

    protected override void OnEntityDeathMethod(EntitySO entitySO, IDamageSource damageSource)
    {
        base.OnEntityDeathMethod(entitySO, damageSource);

        OnPlayerDeath?.Invoke(this, new OnEntityDeathEventArgs { entitySO = entitySO as CharacterSO, damageSource = damageSource });
        OnAnyPlayerDeath?.Invoke(this, new OnEntityDeathEventArgs { entitySO = entitySO as CharacterSO, damageSource = damageSource });
    }

    protected override void OnEntityExecutedMethod(EntitySO entitySO, IDamageSource damageSource)
    {
        base.OnEntityExecutedMethod(entitySO, damageSource);

        OnPlayerExecuted?.Invoke(this, new OnEntityDeathEventArgs { entitySO = entitySO as AllySO, damageSource = damageSource });
        OnAnyPlayerExecuted?.Invoke(this, new OnEntityDeathEventArgs { entitySO = entitySO as AllySO, damageSource = damageSource });
    }

    protected override void OnEntityCurrentHealthClampedMethod()
    {
        base.OnEntityCurrentHealthClampedMethod();

        OnPlayerCurrentHealthClamped?.Invoke(this, new OnEntityCurrentHealthClampedEventArgs { currentHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value });
        OnAnyPlayerCurrentHealthClamped?.Invoke(this, new OnEntityCurrentHealthClampedEventArgs { currentHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value });
    }

    protected override void OnEntityCurrentShieldClampedMethod()
    {
        base.OnEntityCurrentShieldClampedMethod();

        OnPlayerCurrentShieldClamped?.Invoke(this, new OnEntityCurrentShieldClampedEventArgs { currentShield = currentShield, maxShield = entityMaxShieldStatResolver.Value });
        OnAnyPlayerCurrentShieldClamped?.Invoke(this, new OnEntityCurrentShieldClampedEventArgs { currentShield = currentShield, maxShield = entityMaxShieldStatResolver.Value });
    }
    #endregion
}