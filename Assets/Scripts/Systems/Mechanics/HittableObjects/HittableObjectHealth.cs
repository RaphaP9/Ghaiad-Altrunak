using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableObjectHealth : MonoBehaviour, IHasHealth
{
    [Header("Components")]
    [SerializeField] private HittableObjectIdentifier hittableObjectIdentifier;

    [Header("Runtime Filled")]
    [SerializeField] protected int currentHealth;
    [SerializeField] protected int currentShield;

    public int CurrentHealth => currentHealth;
    public int CurrentShield => currentShield;

    public static event EventHandler<OnHittableObjectStatsEventArgs> OnAnyHittableObjectStatsInitialized;
    public event EventHandler<OnHittableObjectStatsEventArgs> OnHittableObjectStatsInitialized;

    public static event EventHandler<OnHittableObjectHealthTakeDamageEventArgs> OnAnyHittableObjectHealthTakeDamage;
    public event EventHandler<OnHittableObjectHealthTakeDamageEventArgs> OnHittableObjectHealthTakeDamage;

    public static event EventHandler<OnHittableObjectShieldTakeDamageEventArgs> OnAnyHittableObjectShieldTakeDamage;
    public event EventHandler<OnHittableObjectShieldTakeDamageEventArgs> OnHittableObjectShieldTakeDamage;

    public static event EventHandler<OnHittableObjectHealEventArgs> OnAnyHittableObjectHeal;
    public event EventHandler<OnHittableObjectHealEventArgs> OnHittableObjectHeal;

    public static event EventHandler<OnHittableObjectShieldRestoredEventArgs> OnAnyHittableObjectShieldRestored;
    public event EventHandler<OnHittableObjectShieldRestoredEventArgs> OnHittableObjectShieldRestored;

    public static event EventHandler OnAnyHittableObjectDeath;
    public event EventHandler OnHittableObjectDeath;


    #region EventArgs Classes
    public class OnHittableObjectStatsEventArgs : EventArgs
    {
        public int maxHealth;
        public int currentHealth;

        public int maxShield;
        public int currentShield;
    }
    public class OnHittableObjectShieldTakeDamageEventArgs : EventArgs
    {
        public int damageTakenByShield;

        public int previousShield;
        public int newShield;
        public int maxShield;

        public bool isCrit;

        public IDamageSource damageSource;
        public IHasHealth damageReceiver;
    }

    public class OnHittableObjectHealthTakeDamageEventArgs : EventArgs
    {
        public int damageTakenByHealth;

        public int previousHealth;
        public int newHealth;
        public int maxHealth;

        public bool isCrit;

        public IDamageSource damageSource;
        public IHasHealth damageReceiver;
    }

    public class OnHittableObjectHealEventArgs : EventArgs
    {
        public int healDone;

        public int previousHealth;
        public int newHealth;
        public int maxHealth;

        public IHealSource healSource;
        public IHasHealth healReceiver;
    }

    public class OnHittableObjectShieldRestoredEventArgs : EventArgs
    {
        public int shieldRestored;

        public int previousShield;
        public int newShield;
        public int maxShield;

        public IShieldSource shieldSource;
        public IHasHealth shieldReceiver;
    }
    #endregion

    protected void Start()
    {
        InitializeStats();
    }

    protected virtual void InitializeStats()
    {
        currentHealth = CalculateMaxHealth();
        currentShield = CalculateMaxShield();

        OnHittableObjectStatsInitialized?.Invoke(this, new OnHittableObjectStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxShield(), currentShield = currentShield });
        OnAnyHittableObjectStatsInitialized?.Invoke(this, new OnHittableObjectStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxShield(), currentShield = currentShield });
    }

    protected virtual int CalculateMaxHealth() => hittableObjectIdentifier.HittableObjectSO.health;
    protected virtual int CalculateMaxShield() => hittableObjectIdentifier.HittableObjectSO.shield;

    #region Interface Methods
    public virtual bool AvoidDamageTakeHits() => false;
    public virtual bool AvoidDamagePassThrough() => !IsAlive();
    public virtual bool CanHeal() => true;
    public virtual bool CanRestoreShield() => true;

    public bool TakeDamage(DamageData damageData) //Any damage taken By a HittableObject is 1
    {
        //HittableObjects can't dodge
        if (!AvoidDamagePassThrough()) return false;
        if (!AvoidDamageTakeHits()) return true;

        int previousHealth = currentHealth;
        int previousShield = currentShield;

        int damageTakenByShield, damageTakenByHealth;

        damageTakenByShield = HasShield() ? 1 : 0;
        damageTakenByHealth = HasShield() ? 0 : 1;

        currentShield = currentShield < damageTakenByShield ? 0 : currentShield - damageTakenByShield;
        currentHealth = currentHealth < damageTakenByHealth ? 0 : currentHealth - damageTakenByHealth;

        if (damageTakenByShield > 0)
        {
            OnHittableObjectShieldTakeDamageMethod(damageTakenByShield, previousShield, damageData.isCrit, damageData.damageSource);
        }

        if (damageTakenByHealth > 0)
        {
            OnHittableObjectHealthTakeDamageMethod(damageTakenByHealth, previousHealth, damageData.isCrit, damageData.damageSource);
        }

        if (!IsAlive()) OnHittableObjectDeathMethod();

        return true;
    }

    public void Execute(ExecuteDamageData executeDamageData)
    {
        if (!AvoidDamageTakeHits()) return;
        if (!IsAlive()) return;

        int previousHealth = currentHealth;
        int previousShield = currentShield;

        int damageTakenByShield = HasShield() ? MechanicsUtilities.GetExecuteDamage() : 0;
        int damageTakenByHealth = MechanicsUtilities.GetExecuteDamage();

        currentShield = 0;
        currentHealth = 0;

        if (damageTakenByShield > 0)
        {
            if (executeDamageData.triggerShieldTakeDamageEvents) OnHittableObjectShieldTakeDamageMethod(damageTakenByShield, previousShield, executeDamageData.isCrit, executeDamageData.damageSource);
        }

        if (damageTakenByHealth > 0)
        {
            if (executeDamageData.triggerHealthTakeDamageEvents) OnHittableObjectHealthTakeDamageMethod(damageTakenByHealth, previousHealth, executeDamageData.isCrit, executeDamageData.damageSource);
        }

        OnHittableObjectDeathMethod();
    }

    public void Heal(HealData healData)
    {
        if (!CanHeal()) return;
        if (!IsAlive()) return;

        int previousHealth = currentHealth;

        int effectiveHealAmount = currentHealth + healData.healAmount > CalculateMaxHealth() ? CalculateMaxHealth() - currentHealth : healData.healAmount;
        currentHealth = currentHealth + effectiveHealAmount > CalculateMaxHealth() ? CalculateMaxHealth() : currentHealth + effectiveHealAmount;

        OnHittableObjectHealMethod(effectiveHealAmount, previousHealth, healData.healSource);
    }
    public void HealCompletely(IHealSource healSource)
    {
        if (!CanHeal()) return;
        if (!IsAlive()) return;

        int previousHealth = currentHealth;

        int healAmount = CalculateMaxHealth() - currentHealth;
        currentHealth = CalculateMaxHealth();

        OnHittableObjectHealMethod(healAmount, previousHealth, healSource);
    }

    public void RestoreShield(ShieldData shieldData)
    {
        if (!CanRestoreShield()) return;
        if (!IsAlive()) return;

        int previousShield = currentShield;

        int effectiveShieldRestored = currentShield + shieldData.shieldAmount > CalculateMaxShield() ? CalculateMaxShield() - currentShield : shieldData.shieldAmount;
        currentShield = currentShield + effectiveShieldRestored > CalculateMaxShield() ? CalculateMaxShield() : currentShield + effectiveShieldRestored;

        OnHittableObjectShieldRestoredMethod(effectiveShieldRestored, previousShield, shieldData.shieldSource);
    }

    public void RestoreShieldCompletely(IShieldSource shieldSource)
    {
        if (!CanRestoreShield()) return;
        if (!IsAlive()) return;

        int previousShield = currentShield;

        int shieldAmount = CalculateMaxShield() - currentShield;
        currentShield = CalculateMaxShield();

        OnHittableObjectShieldRestoredMethod(shieldAmount, previousShield, shieldSource);
    }

    public bool IsFullHealth() => currentHealth >= CalculateMaxHealth();
    public bool IsFullShield() => currentShield >= CalculateMaxHealth();
    public bool IsAlive() => currentHealth > 0;
    public bool HasShield() => currentShield > 0;

    #endregion

    #region Virtual Methods

    protected virtual void OnHittableObjectStatsInitializedMethod()
    {
        OnHittableObjectStatsInitialized?.Invoke(this, new OnHittableObjectStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield});
        OnAnyHittableObjectStatsInitialized?.Invoke(this, new OnHittableObjectStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield });
    }

    protected virtual void OnHittableObjectHealthTakeDamageMethod(int damageTakenByHealth, int previousHealth, bool isCrit, IDamageSource damageSource)
    {
        OnHittableObjectHealthTakeDamage?.Invoke(this, new OnHittableObjectHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = CalculateMaxHealth(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyHittableObjectHealthTakeDamage?.Invoke(this, new OnHittableObjectHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = CalculateMaxHealth(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});
    }

    protected virtual void OnHittableObjectShieldTakeDamageMethod(int damageTakenByShield, int previousShield, bool isCrit, IDamageSource damageSource)
    {
        OnHittableObjectShieldTakeDamage?.Invoke(this, new OnHittableObjectShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = CalculateMaxShield(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyHittableObjectShieldTakeDamage?.Invoke(this, new OnHittableObjectShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = CalculateMaxShield(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});
    }

    protected virtual void OnHittableObjectHealMethod(int healAmount, int previousHealth, IHealSource healSource)
    {
        OnHittableObjectHeal?.Invoke(this, new OnHittableObjectHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = CalculateMaxHealth(), healSource = healSource, healReceiver = this});
        OnAnyHittableObjectHeal?.Invoke(this, new OnHittableObjectHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = CalculateMaxHealth(), healSource = healSource, healReceiver = this});
    }

    protected virtual void OnHittableObjectShieldRestoredMethod(int shieldAmount, int previousShield, IShieldSource shieldSource)
    {
        OnHittableObjectShieldRestored?.Invoke(this, new OnHittableObjectShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = CalculateMaxShield(), shieldSource = shieldSource, shieldReceiver = this });
        OnAnyHittableObjectShieldRestored?.Invoke(this, new OnHittableObjectShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = CalculateMaxShield(), shieldSource = shieldSource, shieldReceiver = this });
    }

    protected virtual void OnHittableObjectDeathMethod()
    {
        OnHittableObjectDeath?.Invoke(this, EventArgs.Empty);
        OnAnyHittableObjectDeath?.Invoke(this, EventArgs.Empty);
    }
    #endregion
}
