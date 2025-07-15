using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoundHandler : MonoBehaviour
{
    public static event EventHandler<OnRoundEventArgs> OnRoundStart;
    public static event EventHandler<OnRoundEventArgs> OnRoundCompleted;

    public class OnRoundEventArgs : EventArgs
    {
        public RoundSO roundSO;
    }

    private void Awake()
    {
        SetSingleton();
    }

    protected abstract void SetSingleton();

    protected virtual void OnRoundStartMethod(RoundSO roundSO)
    {
        OnRoundStart?.Invoke(this, new OnRoundEventArgs { roundSO = roundSO });
    }

    protected virtual void OnRoundCompletedMethod(RoundSO roundSO)
    {
        OnRoundCompleted?.Invoke(this, new OnRoundEventArgs { roundSO = roundSO });
    }
}
