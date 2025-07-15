using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour, IAbilityInterruption, IAttackInterruption//Some Abilities Can interrupt other ability castings or attacks
{
    [Header("Ability Components")]
    [SerializeField] protected AbilityIdentifier abilityIdentifier;
    [SerializeField] protected AbilityLevelHandler abilityLevelHandler;
    [SerializeField] protected AbilitySlotHandler abilitySlotHandler;
    [Space]
    [SerializeField] protected PlayerHealth playerHealth;
    [Space]
    [SerializeField] protected List<Component> abilityInterruptionComponents;

    private List<IAbilityInterruption> abilityInterruptions;

    public AbilityLevelHandler AbilityLevelHandler => abilityLevelHandler;
    public AbilitySO AbilitySO => abilityIdentifier.AbilitySO;
    public AbilitySlot AbilitySlot => abilitySlotHandler.AbilitySlot;
    public AbilityLevel AbilityLevel => abilityLevelHandler.AbilityLevel;
    public bool IsActiveVariant => abilitySlotHandler.ActiveAbilityVariant == this;

    #region Events

    //For specific Ability Casts, Sending the AbilitySO As Parammeter. Therefore check abilityID.
    public static event EventHandler<OnAbilityCastEventArgs> OnAnyAbilityCastDenied; 
    public static event EventHandler<OnAbilityCastEventArgs> OnAnyAbilityCast;

    public event EventHandler<OnAbilityCastEventArgs> OnAbilityCastDenied;
    public event EventHandler<OnAbilityCastEventArgs> OnAbilityCast;

    #endregion

    #region EventArgs Classes

    public class OnAbilityLevelIncreaseEventArgs : EventArgs
    {
        public AbilitySO abilitySO;
        public AbilityLevel newAbilityLevel;
    }

    public class OnAbilityCastEventArgs : EventArgs
    {
        public Ability ability;
    }

    #endregion

    protected virtual void OnEnable()
    {
        abilitySlotHandler.OnAbilityVariantInitialized += PlayerAbilitySlotsVariantsHandler_OnAbilityVariantInitialized;
        abilitySlotHandler.OnAbilityVariantSelected += AbilityVariantHandler_OnAbilityVariantSelected;

        abilityLevelHandler.OnAbilityLevelInitialized += AbilityLevelHandler_OnAbilityLevelInitialized;
        abilityLevelHandler.OnAbilityLevelSet += AbilityLevelHandler_OnAbilityLevelSet;
    }

    protected virtual void OnDisable()
    {
        abilitySlotHandler.OnAbilityVariantInitialized -= PlayerAbilitySlotsVariantsHandler_OnAbilityVariantInitialized;
        abilitySlotHandler.OnAbilityVariantSelected -= AbilityVariantHandler_OnAbilityVariantSelected;

        abilityLevelHandler.OnAbilityLevelInitialized -= AbilityLevelHandler_OnAbilityLevelInitialized;
        abilityLevelHandler.OnAbilityLevelSet -= AbilityLevelHandler_OnAbilityLevelSet;
    }

    protected virtual void Awake()
    {
        abilityInterruptions = GeneralUtilities.TryGetGenericsFromComponents<IAbilityInterruption>(abilityInterruptionComponents);
    }

    protected virtual void Update()
    {
        HandleUpdateLogic();
    }

    protected virtual void FixedUpdate()
    {
        HandleFixedUpdateLogic();
    }

    //All abilities can be cast. If only passive, cast is always denied

    public void TryCastAbility()
    {
        if (CanCastAbility())
        {
            OnAbilityCastMethod();
        }
        else
        {
            OnAbilityCastDeniedMethod();
        }
    }

    #region Interface Methods
    public virtual bool IsInterruptingAbility() => false;
    public virtual bool IsInterruptingAttack() => false;
    #endregion

    #region Abstract Methods
    protected abstract void HandleUpdateLogic();
    protected abstract void HandleFixedUpdateLogic();


    protected abstract void OnAbilityVariantActivationMethod();
    protected abstract void OnAbilityVariantDeactivationMethod();

    protected abstract void OnAbililityLevelInitializedMethod();
    protected abstract void OnAbililityLevelSetMethod();

    protected virtual void OnAbilityCastMethod()
    {
        OnAbilityCast?.Invoke(this, new OnAbilityCastEventArgs { ability = this });
        OnAnyAbilityCast?.Invoke(this, new OnAbilityCastEventArgs { ability = this });
    }

    protected virtual void OnAbilityCastDeniedMethod()
    {
        OnAbilityCastDenied?.Invoke(this, new OnAbilityCastEventArgs { ability = this });
        OnAnyAbilityCastDenied?.Invoke(this, new OnAbilityCastEventArgs { ability = this });
    }

    public virtual bool CanCastAbility()
    {
        if (!abilityLevelHandler.IsUnlocked()) return false;
        if (!playerHealth.IsAlive()) return false;
        if (AbilitySO.GetAbilityType() == AbilityType.Passive) return false; //Can not cast if only passive

        foreach(IAbilityInterruption abilityInterruption in abilityInterruptions)
        {
            if(abilityInterruption.IsInterruptingAbility()) return false;
        }

        return true;
    }
    #endregion

    #region Subscriptions
    private void PlayerAbilitySlotsVariantsHandler_OnAbilityVariantInitialized(object sender, AbilitySlotHandler.OnAbilityVariantInitializationEventArgs e)
    {
        if (e.abilityVariant == this)
        {
            OnAbilityVariantActivationMethod();
        }      
    }

    private void AbilityVariantHandler_OnAbilityVariantSelected(object sender, AbilitySlotHandler.OnAbilityVariantSelectionEventArgs e)
    {
        if (e.previousAbilityVariant == this)
        {
            OnAbilityVariantDeactivationMethod();
        }

        if (e.newAbilityVariant == this)
        {
            OnAbilityVariantActivationMethod();
        }
    }

    private void AbilityLevelHandler_OnAbilityLevelInitialized(object sender, AbilityLevelHandler.OnAbilityLevelEventArgs e)
    {
        OnAbililityLevelInitializedMethod();
    }

    private void AbilityLevelHandler_OnAbilityLevelSet(object sender, AbilityLevelHandler.OnAbilityLevelEventArgs e)
    {
        OnAbililityLevelSetMethod();
    }

    #endregion
}
