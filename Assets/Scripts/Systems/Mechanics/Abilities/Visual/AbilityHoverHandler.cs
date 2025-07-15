using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class AbilityHoverHandler : UIHoverHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Runtime Filled")]
    [SerializeField] private Ability ability;
    [SerializeField] private bool isHovered;

    public bool IsHovered => isHovered;

    public static event EventHandler<OnAbilityHoverEventArgs> OnAbilityHoverEnter;
    public static event EventHandler<OnAbilityHoverEventArgs> OnAbilityHoverExit;

    public class OnAbilityHoverEventArgs : EventArgs
    {
        public Ability ability;
        public PivotQuadrant pivotQuadrant;
    }

    public void AssignAbility(Ability ability) => SetAbility(ability);

    #region Setters
    private void SetAbility(Ability ability) => this.ability = ability;
    private void ClearAbility() => ability = null;
    #endregion

    public void OnPointerEnter(PointerEventData eventData)
    {
        PivotQuadrant pivotQuadrant = GetPivotQuadrantByScreenQuadrant(GeneralUtilities.GetScreenQuadrant(rectTransformRefference));
        isHovered = true;
        OnAbilityHoverEnter?.Invoke(this, new OnAbilityHoverEventArgs { ability = ability, pivotQuadrant = pivotQuadrant });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        OnAbilityHoverExit?.Invoke(this, new OnAbilityHoverEventArgs { ability = ability });
    }
}
