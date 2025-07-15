using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatsHoverHandler : UIHoverHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Components")]
    [SerializeField] private StatInfoSO statInfoSO;

    [Header("Runtime Filled")]
    [SerializeField] private bool isHovered;

    public bool IsHovered => isHovered;

    public static event EventHandler<OnStatHoverEventArgs> OnStatHoverEnter;
    public static event EventHandler<OnStatHoverEventArgs> OnStatHoverExit;

    public class OnStatHoverEventArgs : EventArgs
    {
        public StatInfoSO statInfoSO;
        public PivotQuadrant pivotQuadrant;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PivotQuadrant pivotQuadrant = GetPivotQuadrantByScreenQuadrant(GeneralUtilities.GetScreenQuadrant(rectTransformRefference));
        isHovered = true;
        OnStatHoverEnter?.Invoke(this, new OnStatHoverEventArgs { statInfoSO = statInfoSO, pivotQuadrant = pivotQuadrant });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        OnStatHoverExit?.Invoke(this, new OnStatHoverEventArgs { statInfoSO = statInfoSO });
    }
}