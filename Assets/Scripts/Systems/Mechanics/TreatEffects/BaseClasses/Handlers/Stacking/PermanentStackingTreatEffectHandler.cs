using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PermanentStackingTreatEffectHandler : StackingTreatEffectHandler
{
    private void Start()
    {
        RecoverSavedStacks();
    }

    #region Saved Refference
    protected abstract string GetRefferencialGUID();
    protected abstract NumericEmbeddedStat GetRefferencialNumericEmbeddedStatPerStack();

    //The saved refferencial numeric stat type must coincide with the refferencial numeric embedded stat per stack
    protected NumericStatModifier FindSavedRefferencialNumericStatModifier() => RunNumericStatModifierManager.Instance.GetFirstNumericStatModifierByGUIDAndNumericStatType(GetRefferencialGUID(), GetRefferencialNumericEmbeddedStatPerStack().numericStatType);

    #endregion

    #region Stat Recovery
    private void RecoverSavedStacks()
    {
        NumericStatModifier savedRefferencialNumericStatModifier = FindSavedRefferencialNumericStatModifier();

        if(savedRefferencialNumericStatModifier == null) //If there is no saved numeric stat modifier
        {
            ResetStacks();
            return;
        }

        //Use a proportional relation to recover saved stacks. Ex. if saved stacks value is 6 and we have a referencial numeric embedded stack with value 0.5, we can conclude we have 12 stacks saved
        int savedStacks = Mathf.RoundToInt(savedRefferencialNumericStatModifier.value / GetRefferencialNumericEmbeddedStatPerStack().value); 
        SetStacks(savedStacks);
    }
    #endregion

    protected override void AddStacks(int quantity)
    {
        base.AddStacks(quantity);
        RunNumericStatModifierManager.Instance.RemoveStatModifiersByGUID(GetRefferencialGUID()); //Remove Stats from previous stacked value, in inherited classes, we Add the stat modifiers
    }

    protected override void ResetStacks()
    {
        base.ResetStacks();
        RunNumericStatModifierManager.Instance.RemoveStatModifiersByGUID(GetRefferencialGUID());
    }

    protected override void AddProportionalStatForStacks(NumericEmbeddedStat numericEmbeddedStatPerStack)
    {
        RunNumericStatModifierManager.Instance.AddSingleNumericStatModifier(GetRefferencialGUID(), MechanicsUtilities.GenerateProportionalNumericStatPerStack(stacks, numericEmbeddedStatPerStack));
    }

    protected override void OnTreatEffectDeactivatedByInventoryObjectsMethod() 
    {
        base.OnTreatEffectDeactivatedByInventoryObjectsMethod();
        RunNumericStatModifierManager.Instance.RemoveStatModifiersByGUID(GetRefferencialGUID()); //Remove Stacks from Run Numeric Stat Modifier List
    }
}
