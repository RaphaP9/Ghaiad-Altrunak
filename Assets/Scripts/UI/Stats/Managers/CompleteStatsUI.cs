using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteStatsUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private CanvasGroup canvasGroup;

    private void OnEnable()
    {
        StatsUIOpeningManager.OnStatsUIOpen += StatsUIOpeningManager_OnStatsUIOpen;
        StatsUIOpeningManager.OnStatsUIClose += StatsUIOpeningManager_OnStatsUIClose;

        StatsUIOpeningManager.OnStatsUIOpenInmediately += StatsUIOpeningManager_OnStatsUIOpenInmediately;
        StatsUIOpeningManager.OnStatsUICloseInmediately += StatsUIOpeningManager_OnStatsUICloseInmediately;
    }

    private void OnDisable()
    {
        StatsUIOpeningManager.OnStatsUIOpen -= StatsUIOpeningManager_OnStatsUIOpen;
        StatsUIOpeningManager.OnStatsUIClose -= StatsUIOpeningManager_OnStatsUIClose;

        StatsUIOpeningManager.OnStatsUIOpenInmediately -= StatsUIOpeningManager_OnStatsUIOpenInmediately;
        StatsUIOpeningManager.OnStatsUICloseInmediately -= StatsUIOpeningManager_OnStatsUICloseInmediately;
    }


    private void OpenStatsInmediately()
    {
        UIUtilities.SetCanvasGroupAlpha(canvasGroup, 1f);
    }

    private void CloseStatsInmediately()
    {
        UIUtilities.SetCanvasGroupAlpha(canvasGroup, 0f);
    }

    private void OpenStats()
    {
        UIUtilities.SetCanvasGroupAlpha(canvasGroup, 1f);
    }

    private void CloseStats()
    {
        UIUtilities.SetCanvasGroupAlpha(canvasGroup, 0f);
    }


    #region Subscriptions
    private void StatsUIOpeningManager_OnStatsUIOpen(object sender, System.EventArgs e)
    {
        OpenStats();
    }

    private void StatsUIOpeningManager_OnStatsUIClose(object sender, System.EventArgs e)
    {
        CloseStats();
    }

    private void StatsUIOpeningManager_OnStatsUIOpenInmediately(object sender, System.EventArgs e)
    {
        OpenStatsInmediately();
    }

    private void StatsUIOpeningManager_OnStatsUICloseInmediately(object sender, System.EventArgs e)
    {
        CloseStatsInmediately();
    }
    #endregion
}
