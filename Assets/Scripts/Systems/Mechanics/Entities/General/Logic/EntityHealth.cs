using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityHealth : MonoBehaviour, IHasHealth
{
    [Header("Entity Health Components")]
    [SerializeField] protected EntityIdentifier entityIdentifier;
    [Space]
    [SerializeField] protected EntityMaxHealthStatResolver entityMaxHealthStatResolver;
    [SerializeField] protected EntityArmorStatResolver entityArmorStatResolver;
    [SerializeField] protected EntityDodgeChanceStatResolver entityDodgeChanceStatResolver;
    [Space]
    [SerializeField] protected List<Component> dodgerComponents;
    [SerializeField] protected List<Component> immunerComponents;

    [Header("Entity Health Settings")]
    [SerializeField, Range(0f,3f)] protected float invulnerableTimeAfterTakingDamage;

    [Header("Runtime Filled")]
    [SerializeField] protected int currentHealth;

    protected List<IDodger> dodgers;
    protected List<IImmuner> immuners;

    protected bool invulnerableAfterTakingDamage = false;

    #region Properties
    public int CurrentHealth => currentHealth;
    #endregion

    #region Events
    public static event EventHandler<OnEntityInitializedEventArgs> OnAnyEntityInitialized;
    public event EventHandler<OnEntityInitializedEventArgs> OnEntityInitialized;

    public static event EventHandler<OnEntityDodgeEventArgs> OnAnyEntityDodge;
    public event EventHandler<OnEntityDodgeEventArgs> OnEntityDodge;

    public static event EventHandler<OnEntityImmuneEventArgs> OnAnyEntityImmune;
    public event EventHandler<OnEntityImmuneEventArgs> OnEntityImmune;

    public static event EventHandler<OnEntityHealthTakeDamageEventArgs> OnAnyEntityHealthTakeDamage;
    public event EventHandler<OnEntityHealthTakeDamageEventArgs> OnEntityHealthTakeDamage;

    public static event EventHandler<OnEntityHealEventArgs> OnAnyEntityHeal;
    public event EventHandler<OnEntityHealEventArgs> OnEntityHeal;

    public static event EventHandler<OnEntityDeathEventArgs> OnAnyEntityDeath;
    public event EventHandler<OnEntityDeathEventArgs> OnEntityDeath;

    public static event EventHandler<OnEntityDeathEventArgs> OnAnyEntityExecuted;
    public event EventHandler<OnEntityDeathEventArgs> OnEntityExecuted;

    public static event EventHandler<OnEntityCurrentHealthClampedEventArgs> OnAnyEntityCurrentHealthClamped;
    public event EventHandler<OnEntityCurrentHealthClampedEventArgs> OnEntityCurrentHealthClamped;
    #endregion

    #region EventArgs Classes
    public class OnEntityInitializedEventArgs : EventArgs
    {
        public int currentHealth;
    }

    public class OnEntityDodgeEventArgs : EventArgs
    {
        public int damageDodged;
        public bool isCrit;

        public IDamageSource damageSource;
    }

    public class OnEntityImmuneEventArgs : EventArgs
    {
        public int damageImmuned;
        public bool isCrit;

        public IDamageSource damageSource;
    }

    public class OnEntityHealthTakeDamageEventArgs : EventArgs
    {
        public int damageTakenByHealth;
        public int rawDamage;

        public int previousHealth;
        public int newHealth;
        public int maxHealth;

        public bool isCrit;

        public IDamageSource damageSource;
        public IHasHealth damageReceiver;
    }

    public class OnEntityHealEventArgs : EventArgs
    {
        public int healDone;

        public int previousHealth;
        public int newHealth;
        public int maxHealth;

        public IHealSource healSource;
        public IHasHealth healReceiver;
    }

    public class OnEntityDeathEventArgs : EventArgs
    {
        public EntitySO entitySO;
        public IDamageSource damageSource;
    }

    public class OnEntityCurrentHealthClampedEventArgs: EventArgs
    {
        public int currentHealth;
        public int maxHealth;
    }
    #endregion

    protected virtual void OnEnable()
    {
        entityMaxHealthStatResolver.OnEntityStatInitialized += EntityMaxHealthStatResolver_OnEntityStatInitialized;
        entityMaxHealthStatResolver.OnEntityStatUpdated += EntityMaxHealthStatResolver_OnEntityStatUpdated; //Only to check clampings
    }

    protected virtual void OnDisable()
    {
        entityMaxHealthStatResolver.OnEntityStatInitialized -= EntityMaxHealthStatResolver_OnEntityStatInitialized;
        entityMaxHealthStatResolver.OnEntityStatUpdated -= EntityMaxHealthStatResolver_OnEntityStatUpdated; //Only to check clampings
    }

    protected virtual void Awake()
    {
        dodgers = GeneralUtilities.TryGetGenericsFromComponents<IDodger>(dodgerComponents);
        immuners = GeneralUtilities.TryGetGenericsFromComponents<IImmuner>(immunerComponents);
    }

    protected virtual void InitializeEntity()
    {
        currentHealth = currentHealth > entityMaxHealthStatResolver.Value ? entityMaxHealthStatResolver.Value : currentHealth; //Clamp to Maximums

        if(currentHealth <= 0) //Default Values if currentHealth == 0, if so, max both stats
        {
            currentHealth = entityMaxHealthStatResolver.Value;
        }

        OnEntityInitializedMethod();
    }

 
    #region Stats Clamping
    protected virtual void CheckCurrentHealthClamped()
    {
        if (currentHealth > entityMaxHealthStatResolver.Value)
        {
            currentHealth = entityMaxHealthStatResolver.Value;
            OnEntityCurrentHealthClampedMethod();
        }
    }
    #endregion

    #region Interface Methods
    public virtual bool AvoidDamageTakeHits() => false;
    public virtual bool AvoidDamagePassThrough() => !IsAlive(); //If it is not alive, pass though attacks
    public virtual bool CanHeal() => IsAlive();

    public bool TakeDamage(DamageData damageData) 
    {
        //First Check Damage PassThorugh (Dodged, etc), then damage Hit (immune, invulnerableAfterTakingDamage, etc)
        if(AvoidDamagePassThrough()) return false;

        bool dodged = MechanicsUtilities.EvaluateDodgeChance(entityDodgeChanceStatResolver.Value);

        if ((dodged||IsDodgingByAbility()) && damageData.canBeDodged)
        {
            OnEntityDodgeMethod(damageData);
            return false;
        }

        if (AvoidDamageTakeHits()) return true;

        if (invulnerableAfterTakingDamage && damageData.canBeInvulnerabled) return true;

        if (IsImmuneByAbility() && damageData.canBeImmuned)
        {
            OnEntityImmuneMethod(damageData);
            return true;
        }

        int armorMitigatedDamage = MechanicsUtilities.MitigateDamageByArmor(damageData.damage, entityArmorStatResolver.Value);

        if(armorMitigatedDamage <= 0) return true;

        int previousHealth = currentHealth;
        int damageTakenByHealth = currentHealth < armorMitigatedDamage ? currentHealth : armorMitigatedDamage;

        currentHealth = currentHealth < damageTakenByHealth ? 0 : currentHealth - damageTakenByHealth;

        if(damageTakenByHealth > 0)
        {
            OnEntityHealthTakeDamageMethod(damageTakenByHealth, armorMitigatedDamage, previousHealth, damageData.isCrit, damageData.damageSource);
        }

        if(damageData.triggerInvulnerability) TriggerInvulnerabilityAfterTakingDamage();

        if (!IsAlive()) OnEntityDeathMethod(entityIdentifier.EntitySO, damageData.damageSource);

        return true;
    }

    public void SelfTakeDamage(SelfDamageData selfDamageData)
    {
        DamageData damageData = new DamageData(selfDamageData.damage , selfDamageData.isCrit, entityIdentifier.EntitySO, selfDamageData.canBeDodged, selfDamageData.canBeImmuned, selfDamageData.canBeInvulnerabled, selfDamageData.triggerInulnerability);
        TakeDamage(damageData);
    }

    public void Execute(ExecuteDamageData executeDamageData)
    {
        if (!IsAlive()) return;

        int previousHealth = currentHealth;

        int damageTakenByHealth = MechanicsUtilities.GetExecuteDamage();

        currentHealth = 0;      

        if(damageTakenByHealth > 0)
        {
            if (executeDamageData.triggerHealthTakeDamageEvents) OnEntityHealthTakeDamageMethod(damageTakenByHealth, executeDamageData.executeDamage, previousHealth, executeDamageData.isCrit, executeDamageData.damageSource);
        }

        OnEntityExecutedMethod(entityIdentifier.EntitySO, executeDamageData.damageSource);
        OnEntityDeathMethod(entityIdentifier.EntitySO, executeDamageData.damageSource);
    }

    public void SelfExecute(SelfExecuteDamageData selfExecuteDamageData)
    {
        ExecuteDamageData executeDamageData = new ExecuteDamageData(selfExecuteDamageData.isCrit,entityIdentifier.EntitySO, selfExecuteDamageData.triggerHealthTakeDamageEvents);
        Execute(executeDamageData);
    }

    public void Heal(HealData healData)
    {
        if (!CanHeal()) return;
        if(!IsAlive()) return;

        int previousHealth = currentHealth;

        int effectiveHealAmount = currentHealth + healData.healAmount > entityMaxHealthStatResolver.Value ? entityMaxHealthStatResolver.Value - currentHealth : healData.healAmount;
        currentHealth = currentHealth + effectiveHealAmount > entityMaxHealthStatResolver.Value ? entityMaxHealthStatResolver.Value : currentHealth + effectiveHealAmount;

        if (currentHealth == previousHealth) return; //Did not heal at all

        OnEntityHealMethod(effectiveHealAmount, previousHealth, healData.healSource);
    }
    public void HealCompletely(IHealSource healSource)
    {
        if (!CanHeal()) return;
        if (!IsAlive()) return;

        int previousHealth = currentHealth;

        int healAmount = entityMaxHealthStatResolver.Value - currentHealth;
        currentHealth = entityMaxHealthStatResolver.Value;

        OnEntityHealMethod(healAmount, previousHealth, healSource);
    }

    public bool IsFullHealth() => currentHealth >= entityMaxHealthStatResolver.Value;
    public bool IsAlive() => currentHealth > 0;
    #endregion

    #region Invulnerability After Taking Damage
    protected void TriggerInvulnerabilityAfterTakingDamage()
    {
        if (invulnerableTimeAfterTakingDamage <= 0) return;
        StartCoroutine(TriggerInvulnerabilityAfterTakingDamageCoroutine());
    }

    protected IEnumerator TriggerInvulnerabilityAfterTakingDamageCoroutine()
    {
        invulnerableAfterTakingDamage = true;

        yield return new WaitForSeconds(invulnerableTimeAfterTakingDamage);

        invulnerableAfterTakingDamage = false;
    }
    #endregion

    #region Virtual Event Methods

    protected virtual void OnEntityInitializedMethod()
    {
        OnEntityInitialized?.Invoke(this, new OnEntityInitializedEventArgs { currentHealth = currentHealth});
        OnAnyEntityInitialized?.Invoke(this, new OnEntityInitializedEventArgs { currentHealth = currentHealth});
    }

    protected virtual void OnEntityDodgeMethod(DamageData damageData)
    {
        OnEntityDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
        OnAnyEntityDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
    }

    protected virtual void OnEntityImmuneMethod(DamageData damageData)
    {
        OnEntityImmune?.Invoke(this, new OnEntityImmuneEventArgs { damageImmuned = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
        OnAnyEntityImmune?.Invoke(this, new OnEntityImmuneEventArgs { damageImmuned = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
    }

    protected virtual void OnEntityHealthTakeDamageMethod(int damageTakenByHealth, int rawDamage, int previousHealth, bool isCrit, IDamageSource damageSource)
    {
        OnEntityHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, rawDamage = rawDamage, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyEntityHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, rawDamage= rawDamage, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});
    }

    protected virtual void OnEntityHealMethod(int healAmount, int previousHealth, IHealSource healSource)
    {
        OnEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, healSource = healSource, healReceiver = this});
        OnAnyEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, healSource = healSource, healReceiver = this});
    }

    protected virtual void OnEntityDeathMethod(EntitySO entitySO, IDamageSource damageSource)
    {
        OnEntityDeath?.Invoke(this, new OnEntityDeathEventArgs {entitySO = entitySO, damageSource = damageSource});
        OnAnyEntityDeath?.Invoke(this, new OnEntityDeathEventArgs { entitySO = entitySO, damageSource = damageSource });
    }

    protected virtual void OnEntityExecutedMethod(EntitySO entitySO, IDamageSource damageSource)
    {
        OnEntityExecuted?.Invoke(this, new OnEntityDeathEventArgs { entitySO = entitySO, damageSource = damageSource });
        OnAnyEntityExecuted?.Invoke(this, new OnEntityDeathEventArgs { entitySO = entitySO, damageSource = damageSource });
    }

    //

    protected virtual void OnEntityCurrentHealthClampedMethod()
    {
        OnEntityCurrentHealthClamped?.Invoke(this, new OnEntityCurrentHealthClampedEventArgs { currentHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value });
        OnAnyEntityCurrentHealthClamped?.Invoke(this, new OnEntityCurrentHealthClampedEventArgs { currentHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value });
    }

    #endregion

    #region Abilities Methods

    private bool IsDodgingByAbility()
    {
        if (!IsAlive()) return false;

        foreach (IDodger dodgeAbility in dodgers)
        {
            if (dodgeAbility.IsDodging()) return true;
        }

        return false;
    }

    private bool IsImmuneByAbility()
    {
        if (!IsAlive()) return false;

        foreach (IImmuner immuneAbility in immuners)
        {
            if (immuneAbility.IsImmune()) return true;
        }

        return false;
    }
    #endregion

    #region Subscriptions
    private void EntityMaxHealthStatResolver_OnEntityStatInitialized(object sender, EntityIntStatResolver.OnStatEventArgs e)
    {
        InitializeEntity();
    }

    private void EntityMaxHealthStatResolver_OnEntityStatUpdated(object sender, EntityIntStatResolver.OnStatEventArgs e)
    {
        CheckCurrentHealthClamped();
    }
    #endregion
}
