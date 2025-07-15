using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusStackingTreatEffectHandler : StackingTreatEffectHandler
{
    protected abstract string GetRefferencialGUID();
    protected abstract int GetStacksByStatus();

    protected override void AddStacks(int quantity)
    {
        base.AddStacks(quantity);
        TemporalNumericStatModifierManager.Instance.RemoveStatModifiersByGUID(GetRefferencialGUID()); //Remove Stats from previous stacked value, in inherited classes, we Add the stat modifiers
    }

    protected override void ResetStacks()
    {
        base.ResetStacks();
        TemporalNumericStatModifierManager.Instance.RemoveStatModifiersByGUID(GetRefferencialGUID());
    }

    protected override void AddProportionalStatForStacks(NumericEmbeddedStat numericEmbeddedStatPerStack)
    {
        TemporalNumericStatModifierManager.Instance.AddSingleNumericStatModifier(GetRefferencialGUID(), MechanicsUtilities.GenerateProportionalNumericStatPerStack(stacks, numericEmbeddedStatPerStack));
    }

    protected override void OnTreatEffectActivatedByInventoryObjectsMethod()
    {
        base.OnTreatEffectActivatedByInventoryObjectsMethod();
        SetProportionalStatForStacksByStatus();
    }

    protected override void OnTreatEffectDeactivatedByInventoryObjectsMethod()
    {
        base.OnTreatEffectDeactivatedByInventoryObjectsMethod();
        TemporalNumericStatModifierManager.Instance.RemoveStatModifiersByGUID(GetRefferencialGUID()); 
    }

    protected virtual void SetProportionalStatForStacksByStatus()
    {
        TemporalNumericStatModifierManager.Instance.RemoveStatModifiersByGUID(GetRefferencialGUID());

        int stacks = GetStacksByStatus();
        if (stacks <= 0) return;

        SetStacks(stacks); //Adding to Temporal Numeric... will be done in inheritances
    }
}
