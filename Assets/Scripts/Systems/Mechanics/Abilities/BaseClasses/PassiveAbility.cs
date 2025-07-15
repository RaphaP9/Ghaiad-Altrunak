using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAbility : Ability, IPassiveAbility
{
    private PassiveAbilitySO PassiveAbilitySO => AbilitySO as PassiveAbilitySO;

    #region Abstract Methods - Variants
    protected override void OnAbililityLevelInitializedMethod()
    {
        //
    }

    protected override void OnAbililityLevelSetMethod()
    {
        //
    }
    #endregion

    #region Abstract Methods - Levels

    protected override void OnAbilityVariantActivationMethod()
    {
        //
    }

    protected override void OnAbilityVariantDeactivationMethod()
    {
        //
    }
    #endregion
}
