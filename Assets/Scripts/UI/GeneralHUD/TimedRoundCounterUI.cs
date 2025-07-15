using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimedRoundCounterUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI timedRoundCounterText;

    [Header("Settings")]
    [SerializeField, Range (0, 15)] private int thresholdToTick;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private const string TICK_TRIGGER = "Tick";

    private int previousCounter;
    private bool enableCounterUpdate;

    public static event EventHandler OnTimedRoundCounterTick;

    private void OnEnable()
    {
        TimedRoundHandler.OnTimedRoundStart += TimedRoundHandler_OnTimedRoundStart;
        TimedRoundHandler.OnTimedRoundCompleted += TimedRoundHandler_OnTimedRoundCompleted;
    }

    private void OnDisable()
    {
        TimedRoundHandler.OnTimedRoundStart -= TimedRoundHandler_OnTimedRoundStart;
        TimedRoundHandler.OnTimedRoundCompleted -= TimedRoundHandler_OnTimedRoundCompleted;
    }

    private void Start()
    {
        ResetPreviousCounter();
    }

    private void Update()
    {
        HandleCounter();
    }

    private void HandleCounter()
    {
        if (!enableCounterUpdate) return;
        int currentCounter = Mathf.CeilToInt(TimedRoundHandler.Instance.CurrentRoundCountdown);

        if (currentCounter == previousCounter) return;
        if (currentCounter <= 0) return; //Don't update counter to 0, keep countdown on 1 at minimum

        SetCounterText(currentCounter);

        if (currentCounter <= thresholdToTick) Tick();

        previousCounter = currentCounter;
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
        OnTimedRoundCounterTick?.Invoke(this, EventArgs.Empty);
    }

    private void ResetPreviousCounter() => previousCounter = 0;
    private void SetCounterText(int counter) => timedRoundCounterText.text = counter.ToString();


    private void TimedRoundHandler_OnTimedRoundStart(object sender, TimedRoundHandler.OnTimedRoundEventArgs e)
    {
        ShowUI();
        enableCounterUpdate = true;
    }

    private void TimedRoundHandler_OnTimedRoundCompleted(object sender, TimedRoundHandler.OnTimedRoundEventArgs e)
    {
        HideUI();
        enableCounterUpdate = false;
    }
}
