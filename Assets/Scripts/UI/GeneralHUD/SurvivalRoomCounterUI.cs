using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SurvivalRoomCounterUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI survivalRoomCounterText;

    [Header("Settings")]
    [SerializeField, Range (0, 15)] private int thresholdToTick;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private const string TICK_TRIGGER = "Tick";

    private int previousCounter;
    private bool enableCounterUpdate;

    public static event EventHandler OnSurvivalRoomCounterTick;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

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
        int currentCounter = Mathf.CeilToInt(0f);

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
        OnSurvivalRoomCounterTick?.Invoke(this, EventArgs.Empty);
    }

    private void ResetPreviousCounter() => previousCounter = 0;
    private void SetCounterText(int counter) => survivalRoomCounterText.text = counter.ToString();

    #region Subscriptions

    #endregion
}
