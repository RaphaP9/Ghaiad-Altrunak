using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCooldownHandler : MonoBehaviour
{
    [Header("Runtime Filled")]
    [SerializeField] private float cooldownTimer;

    public float CooldownTimer => cooldownTimer;

    public event EventHandler<OnCooldownTimerSetEventArgs> OnCooldownTimerSet;
    public event EventHandler OnCooldownTimerReset;

    public event EventHandler<OnCooldownTimerReducedEventArgs> OnCooldownTimerReduced;
    public event EventHandler<OnCooldownTimerIncreasedEventArgs> OnCooldownTimerIncreased;

    public class OnCooldownTimerSetEventArgs: EventArgs
    {
        public float cooldownTimer;
    }

    public class OnCooldownTimerReducedEventArgs : EventArgs
    {
        public float previousCooldownTimer;
        public float newCooldownTimer;
        public float reductionTime;
    }

    public class OnCooldownTimerIncreasedEventArgs : EventArgs
    {
        public float previousCooldownTimer;
        public float newCooldownTimer;
        public float increaseTime;
    }

    private void Update()
    {
        HandleCooldownTimer();
    }

    private void HandleCooldownTimer()
    {
        if (cooldownTimer <= 0f) return;

        cooldownTimer -= Time.deltaTime;

        if(cooldownTimer <= 0f) ResetCooldownTimer();
    }

    public bool IsOnCooldown() => cooldownTimer > 0;

    public void SetCooldownTimer(float cooldownTime)
    {
        cooldownTimer = cooldownTime;
        OnCooldownTimerSet?.Invoke(this, new OnCooldownTimerSetEventArgs { cooldownTimer = cooldownTimer });
    }

    public void ResetCooldownTimer()
    {
        cooldownTimer = 0f;

        OnCooldownTimerReset?.Invoke(this, EventArgs.Empty);
    }

    public void ReduceCooldown(float reductionTime)
    {
        if(cooldownTimer <= 0f) return;

        float previousCooldownTimer = cooldownTimer;
        cooldownTimer = cooldownTimer - reductionTime < 0f? 0f: cooldownTimer - reductionTime;

        OnCooldownTimerReduced?.Invoke(this, new OnCooldownTimerReducedEventArgs { previousCooldownTimer = previousCooldownTimer, newCooldownTimer = cooldownTimer, reductionTime = reductionTime });
    }

    public void IncreaseCooldown(float increaseTime)
    {
        float previousCooldownTimer = cooldownTimer;
        cooldownTimer += increaseTime;

        OnCooldownTimerIncreased?.Invoke(this, new OnCooldownTimerIncreasedEventArgs { previousCooldownTimer = previousCooldownTimer, newCooldownTimer = cooldownTimer, increaseTime = increaseTime });
    }
}
