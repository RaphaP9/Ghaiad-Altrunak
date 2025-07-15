using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsHoverUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform rectTransformRefference;

    [Header("Runtime Filled")]
    [SerializeField] private StatInfoSO currentStatInfo;

    public event EventHandler<OnStatInfoEventArgs> OnStatInfoSet;

    public event EventHandler<OnStatInfoEventArgs> OnHoverOpening;
    public event EventHandler<OnStatInfoEventArgs> OnHoverClosing;

    public class OnStatInfoEventArgs : EventArgs
    {
        public StatInfoSO statInfoSO;
    }

    private void OnEnable()
    {
        StatsHoverHandler.OnStatHoverEnter += StatsHoverHandler_OnStatHoverEnter;
        StatsHoverHandler.OnStatHoverExit += StatsHoverHandler_OnStatHoverExit;
    }

    private void OnDisable()
    {
        StatsHoverHandler.OnStatHoverEnter -= StatsHoverHandler_OnStatHoverEnter;
        StatsHoverHandler.OnStatHoverExit -= StatsHoverHandler_OnStatHoverExit;
    }

    #region Method Handlers
    private void HandleHoverEnter(StatInfoSO statInfoSO, UIHoverHandler.PivotQuadrant pivotQuadrant)
    {
        if (currentStatInfo == statInfoSO) return;

        GeneralUtilities.AdjustRectTransformPivotToScreenQuadrant(rectTransformRefference, pivotQuadrant.screenQuadrant, pivotQuadrant.rectTransformPoint);

        SetCurrentStatInfo(statInfoSO);
        OnStatInfoSet?.Invoke(this, new OnStatInfoEventArgs { statInfoSO = currentStatInfo });
        OnHoverOpening?.Invoke(this, new OnStatInfoEventArgs { statInfoSO = currentStatInfo });
    }

    private void HandleHoverExit(StatInfoSO statInfoSO)
    {
        if (currentStatInfo != statInfoSO) return;

        OnHoverClosing?.Invoke(this, new OnStatInfoEventArgs { statInfoSO = currentStatInfo });
        ClearCurrentStatInfo();
    }
    #endregion

    #region Get & Set
    private void SetCurrentStatInfo(StatInfoSO statInfoSO) => currentStatInfo = statInfoSO;
    private void ClearCurrentStatInfo() => currentStatInfo = null;
    #endregion


    private void StatsHoverHandler_OnStatHoverEnter(object sender, StatsHoverHandler.OnStatHoverEventArgs e)
    {
        HandleHoverEnter(e.statInfoSO, e.pivotQuadrant);
    }
    private void StatsHoverHandler_OnStatHoverExit(object sender, StatsHoverHandler.OnStatHoverEventArgs e)
    {
        HandleHoverExit(e.statInfoSO);
    }
}
