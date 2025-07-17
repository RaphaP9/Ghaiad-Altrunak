using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class WavesRoomIndicatorUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI wavesRoomIndicatorText;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private const string TICK_TRIGGER = "Tick";

    private int currentWave;
    private int totalWaves;

    public static event EventHandler OnWavesRoomIndicatorTick;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void ShowUI()
    {
        animator.ResetTrigger(HIDE_TRIGGER);
        animator.SetTrigger(SHOW_TRIGGER);
    }

    private void HideUI()
    {
        animator.ResetTrigger(SHOW_TRIGGER);
        animator.SetTrigger(HIDE_TRIGGER);
    }

    private void Tick()
    {
        animator.SetTrigger(TICK_TRIGGER);
        OnWavesRoomIndicatorTick?.Invoke(this, EventArgs.Empty);
    }

    private void SetWavesRoomIndicatorText(int currentWave, int totalWaves) => wavesRoomIndicatorText.text = $"{currentWave}/{totalWaves}";

    #region Subscriptions

    #endregion
}
