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

    public int CurrentHealth => currentHealth;

    public static event EventHandler<OnHittableObjectEventArgs> OnAnyHittableObjectInitialized;
    public event EventHandler<OnHittableObjectEventArgs> OnHittableObjectInitialized;

    public static event EventHandler<OnHittableObjectHealthTakeDamageEventArgs> OnAnyHittableObjectHealthTakeDamage;
    public event EventHandler<OnHittableObjectHealthTakeDamageEventArgs> OnHittableObjectHealthTakeDamage;

    public static event EventHandler<OnHittableObjectHealEventArgs> OnAnyHittableObjectHeal;
    public event EventHandler<OnHittableObjectHealEventArgs> OnHittableObjectHeal;

    public static event EventHandler OnAnyHittableObjectDeath;
    public event EventHandler OnHittableObjectDeath;


    #region EventArgs Classes
    public class OnHittableObjectEventArgs : EventArgs
    {
        public int maxHealth;
        public int currentHealth;
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
    #endregion

    protected void Start()
    {
        InitializeHittableObject();
    }

    protected virtual void InitializeHittableObject()
    {
        currentHealth = CalculateMaxHealth();

        OnHittableObjectInitializedMethod();
    }

    protected virtual int CalculateMaxHealth() => hittableObjectIdentifier.HittableObjectSO.health;

    #region Interface Methods
    public virtual bool AvoidDamageTakeHits() => false;
    public virtual bool AvoidDamagePassThrough() => !IsAlive();
    public virtual bool CanHeal() => true;

    public bool TakeDamage(DamageData damageData) //Any damage taken By a HittableObject is 1
    {
        //HittableObjects can't dodge
        if (!AvoidDamagePassThrough()) return false;
        if (!AvoidDamageTakeHits()) return true;

        int previousHealth = currentHealth;

        int damageTakenByHealth = 1;

        currentHealth = currentHealth < damageTakenByHealth ? 0 : currentHealth - damageTakenByHealth;

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

        int damageTakenByHealth = MechanicsUtilities.GetExecuteDamage();

        currentHealth = 0;

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

    public bool IsFullHealth() => currentHealth >= CalculateMaxHealth();
    public bool IsAlive() => currentHealth > 0;

    #endregion

    #region Virtual Methods

    protected virtual void OnHittableObjectInitializedMethod()
    {
        OnHittableObjectInitialized?.Invoke(this, new OnHittableObjectEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth});
        OnAnyHittableObjectInitialized?.Invoke(this, new OnHittableObjectEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth});
    }

    protected virtual void OnHittableObjectHealthTakeDamageMethod(int damageTakenByHealth, int previousHealth, bool isCrit, IDamageSource damageSource)
    {
        OnHittableObjectHealthTakeDamage?.Invoke(this, new OnHittableObjectHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = CalculateMaxHealth(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyHittableObjectHealthTakeDamage?.Invoke(this, new OnHittableObjectHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = CalculateMaxHealth(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});
    }

    protected virtual void OnHittableObjectHealMethod(int healAmount, int previousHealth, IHealSource healSource)
    {
        OnHittableObjectHeal?.Invoke(this, new OnHittableObjectHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = CalculateMaxHealth(), healSource = healSource, healReceiver = this});
        OnAnyHittableObjectHeal?.Invoke(this, new OnHittableObjectHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = CalculateMaxHealth(), healSource = healSource, healReceiver = this});
    }

    protected virtual void OnHittableObjectDeathMethod()
    {
        OnHittableObjectDeath?.Invoke(this, EventArgs.Empty);
        OnAnyHittableObjectDeath?.Invoke(this, EventArgs.Empty);
    }
    #endregion
}
