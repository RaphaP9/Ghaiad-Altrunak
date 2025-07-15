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
    [SerializeField] protected EntityMaxShieldStatResolver entityMaxShieldStatResolver;
    [SerializeField] protected EntityArmorStatResolver entityArmorStatResolver;
    [SerializeField] protected EntityDodgeChanceStatResolver entityDodgeChanceStatResolver;
    [Space]
    [SerializeField] protected List<Component> dodgerComponents;
    [SerializeField] protected List<Component> immunerComponents;

    [Header("Entity Health Settings")]
    [SerializeField, Range(0f,3f)] protected float invulnerableTimeAfterTakingDamage;

    [Header("Runtime Filled")]
    [SerializeField] protected int currentHealth;
    [SerializeField] protected int currentShield;

    protected List<IDodger> dodgers;
    protected List<IImmuner> immuners;

    protected bool healthReady = false;
    protected bool shieldReady = false;
    protected bool invulnerableAfterTakingDamage = false;

    #region Properties
    public int CurrentHealth => currentHealth;
    public int CurrentShield => currentShield;
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

    public static event EventHandler<OnEntityShieldTakeDamageEventArgs> OnAnyEntityShieldTakeDamage;
    public event EventHandler<OnEntityShieldTakeDamageEventArgs> OnEntityShieldTakeDamage;

    public static event EventHandler<OnEntityHealEventArgs> OnAnyEntityHeal;
    public event EventHandler<OnEntityHealEventArgs> OnEntityHeal;

    public static event EventHandler<OnEntityShieldRestoredEventArgs> OnAnyEntityShieldRestored;
    public event EventHandler<OnEntityShieldRestoredEventArgs> OnEntityShieldRestored;

    public static event EventHandler<OnEntityDeathEventArgs> OnAnyEntityDeath;
    public event EventHandler<OnEntityDeathEventArgs> OnEntityDeath;

    public static event EventHandler<OnEntityDeathEventArgs> OnAnyEntityExecuted;
    public event EventHandler<OnEntityDeathEventArgs> OnEntityExecuted;

    public static event EventHandler<OnEntityCurrentHealthClampedEventArgs> OnAnyEntityCurrentHealthClamped;
    public event EventHandler<OnEntityCurrentHealthClampedEventArgs> OnEntityCurrentHealthClamped;

    public static event EventHandler<OnEntityCurrentShieldClampedEventArgs> OnAnyEntityCurrentShieldClamped;
    public event EventHandler<OnEntityCurrentShieldClampedEventArgs> OnEntityCurrentShieldClamped;

    #endregion

    #region EventArgs Classes
    public class OnEntityInitializedEventArgs : EventArgs
    {
        public int currentHealth;
        public int currentShield;
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

    public class OnEntityShieldTakeDamageEventArgs : EventArgs
    {
        public int damageTakenByShield;
        public int rawDamage;

        public int previousShield;
        public int newShield;
        public int maxShield;

        public bool isCrit;

        public IDamageSource damageSource;
        public IHasHealth damageReceiver;
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

    public class OnEntityShieldRestoredEventArgs : EventArgs
    {
        public int shieldRestored;

        public int previousShield;
        public int newShield;
        public int maxShield;

        public IShieldSource shieldSource;
        public IHasHealth shieldReceiver;
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

    public class OnEntityCurrentShieldClampedEventArgs : EventArgs
    {
        public int currentShield;
        public int maxShield;
    }
    #endregion

    protected virtual void OnEnable()
    {
        entityMaxHealthStatResolver.OnEntityStatInitialized += EntityMaxHealthStatResolver_OnEntityStatInitialized;
        entityMaxShieldStatResolver.OnEntityStatInitialized += EntityMaxShieldStatResolver_OnEntityStatInitialized;

        entityMaxHealthStatResolver.OnEntityStatUpdated += EntityMaxHealthStatResolver_OnEntityStatUpdated; //Only to check clampings
        entityMaxShieldStatResolver.OnEntityStatUpdated += EntityMaxShieldStatResolver_OnEntityStatUpdated;
    }

    protected virtual void OnDisable()
    {
        entityMaxHealthStatResolver.OnEntityStatInitialized -= EntityMaxHealthStatResolver_OnEntityStatInitialized;
        entityMaxShieldStatResolver.OnEntityStatInitialized -= EntityMaxShieldStatResolver_OnEntityStatInitialized;

        entityMaxHealthStatResolver.OnEntityStatUpdated -= EntityMaxHealthStatResolver_OnEntityStatUpdated; //Only to check clampings
        entityMaxShieldStatResolver.OnEntityStatUpdated -= EntityMaxShieldStatResolver_OnEntityStatUpdated;
    }

    protected virtual void Awake()
    {
        dodgers = GeneralUtilities.TryGetGenericsFromComponents<IDodger>(dodgerComponents);
        immuners = GeneralUtilities.TryGetGenericsFromComponents<IImmuner>(immunerComponents);
    }

    protected virtual void InitializeHealth()
    {
        healthReady = true;
        if (shieldReady) CompleteInitialization();
    }

    protected virtual void InitializeShield()
    {
        shieldReady = true;
        if (healthReady) CompleteInitialization();
    }

    protected virtual void CompleteInitialization()
    {
        currentHealth = currentHealth > entityMaxHealthStatResolver.Value ? entityMaxHealthStatResolver.Value : currentHealth; //Clamp to Maximums
        currentShield = currentShield > entityMaxShieldStatResolver.Value ? entityMaxShieldStatResolver.Value : currentShield;

        if(currentHealth <= 0) //Default Values if currentHealth == 0, if so, max both stats
        {
            currentHealth = entityMaxHealthStatResolver.Value;
            currentShield = entityMaxShieldStatResolver.Value;
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

    protected virtual void CheckCurrentShieldClamped()
    {
        if(currentShield > entityMaxShieldStatResolver.Value)
        {
            currentShield = entityMaxShieldStatResolver.Value;
            OnEntityCurrentShieldClampedMethod();
        }
    }
    #endregion

    #region Interface Methods
    public virtual bool AvoidDamageTakeHits() => false;
    public virtual bool AvoidDamagePassThrough() => !IsAlive(); //If it is not alive, pass though attacks
    public virtual bool CanHeal() => IsAlive();
    public virtual bool CanRestoreShield() => IsAlive();

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
        int previousShield = currentShield;

        int damageTakenByShield, damageTakenByHealth;

        if (HasShield())
        {
            damageTakenByShield = currentShield < armorMitigatedDamage ? currentShield : armorMitigatedDamage; //Shield Absorbs all Damage, Ex: if an entity has 3 Shield and would take 10 damage, it destroys all shield and health does not receive damage at all
            damageTakenByHealth = 0;
        }
        else
        {
            damageTakenByShield = 0;
            damageTakenByHealth = currentHealth < armorMitigatedDamage ? currentHealth : armorMitigatedDamage;
        }

        currentShield = currentShield < damageTakenByShield ? 0 : currentShield - damageTakenByShield;
        currentHealth = currentHealth < damageTakenByHealth ? 0 : currentHealth - damageTakenByHealth;

        if(damageTakenByShield > 0)
        {
            OnEntityShieldTakeDamageMethod(damageTakenByShield, armorMitigatedDamage, previousShield, damageData.isCrit, damageData.damageSource);
        }

        if(damageTakenByHealth > 0)
        {
            OnEntityHealthTakeDamageMethod(damageTakenByHealth, armorMitigatedDamage, previousHealth, damageData.isCrit, damageData.damageSource);
        }

        if(damageData.triggerInvulnerability) HandleInvulnerabilityAfterTakingDamage();

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
        int previousShield = currentShield;

        int damageTakenByShield = HasShield()? MechanicsUtilities.GetExecuteDamage() : 0;
        int damageTakenByHealth = MechanicsUtilities.GetExecuteDamage();

        currentShield = 0;
        currentHealth = 0;
         
        if(damageTakenByShield > 0)
        {
            if(executeDamageData.triggerShieldTakeDamageEvents) OnEntityShieldTakeDamageMethod(damageTakenByShield, executeDamageData.executeDamage, previousShield, executeDamageData.isCrit, executeDamageData.damageSource);
        }

        if(damageTakenByHealth > 0)
        {
            if (executeDamageData.triggerHealthTakeDamageEvents) OnEntityHealthTakeDamageMethod(damageTakenByHealth, executeDamageData.executeDamage, previousHealth, executeDamageData.isCrit, executeDamageData.damageSource);
        }

        OnEntityExecutedMethod(entityIdentifier.EntitySO, executeDamageData.damageSource);
        OnEntityDeathMethod(entityIdentifier.EntitySO, executeDamageData.damageSource);
    }

    public void SelfExecute(SelfExecuteDamageData selfExecuteDamageData)
    {
        ExecuteDamageData executeDamageData = new ExecuteDamageData(selfExecuteDamageData.isCrit,entityIdentifier.EntitySO, selfExecuteDamageData.triggerHealthTakeDamageEvents, selfExecuteDamageData.triggerHealthTakeDamageEvents);
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

    public void RestoreShield(ShieldData shieldData)
    {
        if (!CanRestoreShield()) return;
        if (!IsAlive()) return;

        int previousShield = currentShield;

        int effectiveShieldRestored = currentShield + shieldData.shieldAmount > entityMaxShieldStatResolver.Value ? entityMaxShieldStatResolver.Value - currentShield : shieldData.shieldAmount;
        currentShield = currentShield + effectiveShieldRestored > entityMaxShieldStatResolver.Value ? entityMaxShieldStatResolver.Value : currentShield + effectiveShieldRestored;

        if (currentShield == previousShield) return; //Did not restore shield at all

        OnEntityShieldRestoredMethod(effectiveShieldRestored, previousShield, shieldData.shieldSource);
    }

    public void RestoreShieldCompletely(IShieldSource shieldSource)
    {
        if (!CanRestoreShield()) return;
        if (!IsAlive()) return;

        int previousShield = currentShield;

        int shieldAmount = entityMaxShieldStatResolver.Value - currentShield;
        currentShield = entityMaxShieldStatResolver.Value;

        OnEntityShieldRestoredMethod(shieldAmount, previousShield, shieldSource);
    }

    public bool IsFullHealth() => currentHealth >= entityMaxHealthStatResolver.Value;
    public bool IsFullShield() => currentShield >= entityMaxShieldStatResolver.Value;
    public bool IsAlive() => currentHealth > 0;
    public bool HasShield() => currentShield > 0;

    #endregion

    #region Invulnerability After Taking Damage
    protected void HandleInvulnerabilityAfterTakingDamage()
    {
        if (invulnerableTimeAfterTakingDamage <= 0) return;
        StartCoroutine(InvulnerabilityAfterTakingDamageCoroutine());
    }

    protected IEnumerator InvulnerabilityAfterTakingDamageCoroutine()
    {
        invulnerableAfterTakingDamage = true;

        yield return new WaitForSeconds(invulnerableTimeAfterTakingDamage);

        invulnerableAfterTakingDamage = false;
    }
    #endregion

    #region Virtual Event Methods

    protected virtual void OnEntityInitializedMethod()
    {
        OnEntityInitialized?.Invoke(this, new OnEntityInitializedEventArgs { currentHealth = currentHealth, currentShield = currentShield });
        OnAnyEntityInitialized?.Invoke(this, new OnEntityInitializedEventArgs { currentHealth = currentHealth, currentShield = currentShield });
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

    protected virtual void OnEntityShieldTakeDamageMethod(int damageTakenByShield, int rawDamage, int previousShield, bool isCrit, IDamageSource damageSource)
    {
        OnEntityShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, rawDamage = rawDamage, previousShield = previousShield, 
        newShield = currentShield, maxShield = entityMaxShieldStatResolver.Value, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyEntityShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, rawDamage= rawDamage, previousShield = previousShield, 
        newShield = currentShield, maxShield = entityMaxShieldStatResolver.Value, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

    }

    protected virtual void OnEntityHealMethod(int healAmount, int previousHealth, IHealSource healSource)
    {
        OnEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, healSource = healSource, healReceiver = this});
        OnAnyEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, healSource = healSource, healReceiver = this});
    }

    protected virtual void OnEntityShieldRestoredMethod(int shieldAmount, int previousShield, IShieldSource shieldSource)
    {
        OnEntityShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = entityMaxShieldStatResolver.Value, shieldSource = shieldSource, shieldReceiver = this });
        OnAnyEntityShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = entityMaxShieldStatResolver.Value, shieldSource = shieldSource, shieldReceiver = this });
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

    protected virtual void OnEntityCurrentShieldClampedMethod()
    {
        OnEntityCurrentShieldClamped?.Invoke(this, new OnEntityCurrentShieldClampedEventArgs { currentShield = currentShield, maxShield = entityMaxShieldStatResolver.Value });
        OnAnyEntityCurrentShieldClamped?.Invoke(this, new OnEntityCurrentShieldClampedEventArgs { currentShield = currentShield, maxShield = entityMaxShieldStatResolver.Value });
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
        InitializeHealth();
    }

    private void EntityMaxShieldStatResolver_OnEntityStatInitialized(object sender, EntityIntStatResolver.OnStatEventArgs e)
    {
        InitializeShield();
    }

    private void EntityMaxHealthStatResolver_OnEntityStatUpdated(object sender, EntityIntStatResolver.OnStatEventArgs e)
    {
        CheckCurrentHealthClamped();
    }

    private void EntityMaxShieldStatResolver_OnEntityStatUpdated(object sender, EntityIntStatResolver.OnStatEventArgs e)
    {
        CheckCurrentShieldClamped();
    }
    #endregion
}
