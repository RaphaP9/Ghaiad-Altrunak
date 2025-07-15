using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StackingTreatEffectHandler : TreatEffectHandler
{
    [Header("Stacking Runtime Filled")]
    [SerializeField] protected int stacks;

    public static event EventHandler<OnStackEventArgs> OnStacksGained;
    public static event EventHandler<OnStackEventArgs> OnStacksLost;
    public static event EventHandler<OnStackEventArgs> OnStacksSet;
    public static event EventHandler<OnStackEventArgs> OnStacksReset;

    public class OnStackEventArgs : EventArgs
    {
        public int stacks;
    }

    protected override void OnTreatEffectDeactivatedByInventoryObjectsMethod() //The default behavior is to reset the stacks as soon as you don't have any inventoryObject that activates the treat
    {
        base.OnTreatEffectDeactivatedByInventoryObjectsMethod();
        ResetStacks();
    }

    protected abstract void AddProportionalStatForStacks(NumericEmbeddedStat numericEmbeddedStatPerStack);

    #region Utility Stack Methods

    protected virtual void AddStacks(int quantity)
    {
        stacks += quantity;
        OnStacksGained?.Invoke(this, new OnStackEventArgs { stacks = stacks });
    }

    protected virtual void RemoveStacks(int quantity)
    {
        stacks = stacks - quantity <0? 0: stacks-quantity;
        OnStacksLost?.Invoke(this, new OnStackEventArgs { stacks = stacks });
    }

    protected virtual void SetStacks(int quantity)
    {
        stacks = quantity;
        OnStacksSet?.Invoke(this, new OnStackEventArgs { stacks = stacks });
    }

    protected virtual void ResetStacks()
    {
        stacks = 0;
        OnStacksReset?.Invoke(this, new OnStackEventArgs { stacks = stacks });
    }
    #endregion
}
