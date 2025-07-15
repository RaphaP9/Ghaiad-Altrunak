using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class RoundBuffUntilActionTreatEffectHandler : TreatEffectHandler
{
    protected bool buffActive = false;

    public static event EventHandler OnTreatBuffAdd;
    public static event EventHandler OnTreatBuffRemoved;

    protected virtual void OnEnable()
    {
        GameManager.OnStateChanged += GameManager_OnStateChanged;
    }

    protected virtual void OnDisable()
    {
        GameManager.OnStateChanged -= GameManager_OnStateChanged;
    }

    protected abstract string GetRefferencialGUID();

    protected virtual void AddBuff()
    {
        buffActive = true;
        OnTreatBuffAdd?.Invoke(this, EventArgs.Empty);
        TemporalNumericStatModifierManager.Instance.RemoveStatModifiersByGUID(GetRefferencialGUID()); //First Remove Buff
    }

    protected void RemoveBuff()
    {
        buffActive = false;
        OnTreatBuffRemoved?.Invoke(this, EventArgs.Empty);
        TemporalNumericStatModifierManager.Instance.RemoveStatModifiersByGUID(GetRefferencialGUID());
    }

    #region Subscriptions
    private void GameManager_OnStateChanged(object sender, GameManager.OnStateChangeEventArgs e)
    {
        if (!isCurrentlyActiveByInventoryObjects) return;
        if (!isMeetingCondition) return;

        if (e.newState == GameManager.State.Combat)
        {
            if(!buffActive) AddBuff();
            return;
        }

        if (e.previousState == GameManager.State.Combat)
        {
            if(buffActive) RemoveBuff(); 
            return;
        }
    }
    #endregion
}
