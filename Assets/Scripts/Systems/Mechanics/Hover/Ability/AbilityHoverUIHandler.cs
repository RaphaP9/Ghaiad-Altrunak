using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHoverUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform rectTransformRefference;

    [Header("Runtime Filled")]
    [SerializeField] private Ability currentAbility;

    public Ability CurrentAbility => currentAbility;

    public event EventHandler<OnAbilityEventArgs> OnAbilitySet;

    public event EventHandler<OnAbilityEventArgs> OnHoverOpening;
    public event EventHandler<OnAbilityEventArgs> OnHoverClosing;

    public class OnAbilityEventArgs : EventArgs
    {
        public Ability ability;
    }

    private void OnEnable()
    {
        AbilityHoverHandler.OnAbilityHoverEnter += AbilityHoverHandler_OnAbilityHoverEnter;
        AbilityHoverHandler.OnAbilityHoverExit += AbilityHoverHandler_OnAbilityHoverExit;
    }

    private void OnDisable()
    {
        AbilityHoverHandler.OnAbilityHoverEnter -= AbilityHoverHandler_OnAbilityHoverEnter;
        AbilityHoverHandler.OnAbilityHoverExit -= AbilityHoverHandler_OnAbilityHoverExit;
    }

    #region Method Handlers
    private void HandleHoverEnter(Ability ability, UIHoverHandler.PivotQuadrant pivotQuadrant)
    {
        if (currentAbility != null)
        {
            if (ability.AbilitySlot == currentAbility.AbilitySlot) return;
        }

        GeneralUtilities.AdjustRectTransformPivotToScreenQuadrant(rectTransformRefference, pivotQuadrant.screenQuadrant, pivotQuadrant.rectTransformPoint);

        SetCurrentAbility(ability);
        OnAbilitySet?.Invoke(this, new OnAbilityEventArgs { ability = currentAbility });
        OnHoverOpening?.Invoke(this, new OnAbilityEventArgs { ability = currentAbility });
    }

    private void HandleHoverExit(Ability ability)
    {
        if (currentAbility != null)
        {
            if (ability.AbilitySlot != currentAbility.AbilitySlot) return;
        }

        OnHoverClosing?.Invoke(this, new OnAbilityEventArgs { ability = currentAbility });
        ClearCurrentAbility();
    }
    #endregion

    #region Get & Set
    private void SetCurrentAbility(Ability ability) => currentAbility = ability;
    private void ClearCurrentAbility() => currentAbility = null;
    #endregion

    private void AbilityHoverHandler_OnAbilityHoverEnter(object sender, AbilityHoverHandler.OnAbilityHoverEventArgs e)
    {
        HandleHoverEnter(e.ability, e.pivotQuadrant);
    }

    private void AbilityHoverHandler_OnAbilityHoverExit(object sender, AbilityHoverHandler.OnAbilityHoverEventArgs e)
    {
        HandleHoverExit(e.ability);
    }

}
