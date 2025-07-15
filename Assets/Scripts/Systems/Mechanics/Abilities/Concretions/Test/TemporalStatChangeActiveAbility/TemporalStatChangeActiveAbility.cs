using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TemporalStatChangeActiveAbility : ActiveAbility
{
    private TemporalStatChangeActiveAbilitySO TemporalStatChangeActiveAbilitySO => AbilitySO as TemporalStatChangeActiveAbilitySO;

    [Header("Settings")]
    [SerializeField] private bool onlyOneActiveAtOnce;

    [Header("Specific Runtime Filled")]
    [SerializeField] private List<string> activeAbilityGUIDs;

    #region Logic Methods
    protected override void HandleFixedUpdateLogic() { }
    protected override void HandleUpdateLogic() { }
    #endregion

    #region Abstract Methods

    public override bool CanCastAbility()
    {
        if (!base.CanCastAbility()) return false;
        if (onlyOneActiveAtOnce && ActiveInstances() >= 1) return false;

        return true;
    }

    protected override void OnAbilityCastMethod()
    {
        base.OnAbilityCastMethod();

        StartCoroutine(TemporalStatsChangeCoroutine());
    }
    #endregion

    #region AbilitySpecifics

    private IEnumerator TemporalStatsChangeCoroutine()
    {
        string generatedGUID = GeneralUtilities.GenerateGUID();

        AddGUIDToActiveAbilityGUIDs(generatedGUID);

        TemporalNumericStatModifierManager.Instance.AddStatModifiers(generatedGUID, TemporalStatChangeActiveAbilitySO);

        yield return new WaitForSeconds(TemporalStatChangeActiveAbilitySO.changeDuration);

        TemporalNumericStatModifierManager.Instance.RemoveStatModifiersByGUID(generatedGUID);

        RemoveGUIDFromActiveAbilityGUIDs(generatedGUID);
    }

    private int ActiveInstances() => activeAbilityGUIDs.Count;
    private void AddGUIDToActiveAbilityGUIDs(string guid) => activeAbilityGUIDs.Add(guid);
    private void RemoveGUIDFromActiveAbilityGUIDs(string guid) => activeAbilityGUIDs.Remove(guid);

    #endregion
}
