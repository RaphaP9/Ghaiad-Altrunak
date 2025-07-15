using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveAbility : Ability, IActiveAbility
{
    [Header("Active Ability Components")]
    [SerializeField] protected PlayerCooldownReductionStatResolver cooldownReductionStatResolver;
    [SerializeField] protected AbilityCooldownHandler abilityCooldownHandler;

    public AbilityCooldownHandler AbilityCooldownHandler => abilityCooldownHandler;
    private ActiveAbilitySO ActiveAbilitySO => AbilitySO as ActiveAbilitySO;
    public float ProcessedAbilityCooldown => cooldownReductionStatResolver.GetAbilityCooldown(ActiveAbilitySO.baseCooldown);

    #region InterfaceMethods
    public float CalculateAbilityCooldown() => cooldownReductionStatResolver.GetAbilityCooldown(ActiveAbilitySO.baseCooldown);
    public bool AbilityCastInput() => abilitySlotHandler.GetAssociatedDownInput();
    public override bool CanCastAbility()
    {
        if (!base.CanCastAbility()) return false;
        if (abilityCooldownHandler.IsOnCooldown()) return false;

        return true;
    }
    #endregion

    #region Abstract Methods - Casting
    protected override void OnAbilityCastMethod()
    {
        base.OnAbilityCastMethod();
        abilityCooldownHandler.SetCooldownTimer(ProcessedAbilityCooldown); 
    }

    protected override void OnAbilityCastDeniedMethod()
    {
        base.OnAbilityCastDeniedMethod();
    }

    #endregion

    #region Abstract Methods - Variants
    protected override void OnAbilityVariantActivationMethod()
    {
        abilityCooldownHandler.ResetCooldownTimer(); //Reset Cooldown On Activation
    }

    protected override void OnAbilityVariantDeactivationMethod()
    {
        //
    }
    #endregion

    #region Abstract Methods - Levels
    protected override void OnAbililityLevelInitializedMethod()
    {
        abilityCooldownHandler.ResetCooldownTimer(); //Reset Cooldown On Initialization
    }

    protected override void OnAbililityLevelSetMethod()
    {
        abilityCooldownHandler.ResetCooldownTimer(); //Reset Cooldown On LevelSet
    }
    #endregion
}
