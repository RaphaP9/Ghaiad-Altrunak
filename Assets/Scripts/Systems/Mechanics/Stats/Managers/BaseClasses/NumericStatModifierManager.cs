using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NumericStatModifierManager : StatModifierManager
{
    [Header("Lists - Runtime Filled")]
    [SerializeField] protected List<NumericStatModifier> numericStatModifiers;

    public List<NumericStatModifier> NumericStatModifiers => numericStatModifiers;


    #region In-Line Methods
    public override bool HasStatModifiers() => numericStatModifiers.Count > 0;
    public override int GetStatModifiersQuantity() => numericStatModifiers.Count;

    protected override StatValueType GetStatValueType() => StatValueType.Numeric;

    #endregion

    #region Add/Remove Stat Modifiers
    public void AddStatModifiers(string originGUID, IHasEmbeddedNumericStats numericEmbeddedStatsHolder)
    {
        if (originGUID == "")
        {
            if (debug) Debug.Log("GUID is empty. StatModifiers will not be added");
            return;
        }

        int statsAdded = 0;

        foreach (NumericEmbeddedStat numericEmbeddedStat in numericEmbeddedStatsHolder.GetNumericEmbeddedStats())
        {
            if (AddNumericStatModifier(originGUID, numericEmbeddedStat)) statsAdded++;
        }

        if (statsAdded > 0) UpdateStats();
    }

    protected bool AddNumericStatModifier(string originGUID, NumericEmbeddedStat numericEmbeddedStat)
    {
        if (numericEmbeddedStat == null)
        {
            if (debug) Debug.Log("NumericEmbeddedStat is null. StatModifier will not be added");
            return false;
        }

        if (numericEmbeddedStat.GetStatValueType() != GetStatValueType()) return false; //If not numeric, return false

        NumericStatModifier numericStatModifier = new NumericStatModifier {originGUID = originGUID, numericStatType = numericEmbeddedStat.numericStatType, numericStatModificationType = numericEmbeddedStat.numericStatModificationType, value = numericEmbeddedStat.value};

        numericStatModifiers.Add(numericStatModifier);

        return true;
    }

    public void AddSingleNumericStatModifier(string originGUID, NumericEmbeddedStat numericEmbeddedStat)
    {
        if (numericEmbeddedStat == null)
        {
            if (debug) Debug.Log("NumericEmbeddedStat is null. StatModifier will not be added");
            return;
        }

        if (numericEmbeddedStat.GetStatValueType() != GetStatValueType()) return; //If not numeric, return 

        NumericStatModifier numericStatModifier = new NumericStatModifier { originGUID = originGUID, numericStatType = numericEmbeddedStat.numericStatType, numericStatModificationType = numericEmbeddedStat.numericStatModificationType, value = numericEmbeddedStat.value };
        numericStatModifiers.Add(numericStatModifier);
        UpdateStats();
    }

    public override void RemoveStatModifiersByGUID(string originGUID)
    {
        if (originGUID == "")
        {
            if (debug) Debug.Log("GUID is empty. StatModifiers will not be removed");
            return;
        }

        int removedStats = numericStatModifiers.RemoveAll(statModifier => statModifier.originGUID == originGUID);

        if (removedStats > 0) UpdateStats();
    }
    #endregion

    public NumericStatModifier GetFirstNumericStatModifierByGUID(string GUID)
    {
        foreach(NumericStatModifier numericStatModifier in numericStatModifiers)
        {
            if(numericStatModifier.originGUID == GUID) return numericStatModifier;
        }
        
        return null;
    }

    public NumericStatModifier GetFirstNumericStatModifierByGUIDAndNumericStatType(string GUID, NumericStatType numericStatType)
    {
        foreach (NumericStatModifier numericStatModifier in numericStatModifiers)
        {
            if (numericStatModifier.originGUID != GUID) continue;
            if (numericStatModifier.numericStatType != numericStatType) continue;
            return numericStatModifier;
        }

        return null;
    }

    public void SetStatList(List<NumericStatModifier> setterList) => numericStatModifiers.AddRange(setterList); //Add, NOT Replace!
}