using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Staccato : ActiveAbility, IFacingInterruption
{
    [Header("Specific Runtime Filled")]
    [SerializeField] private string activeAbilityGUID;

    private StaccatoSO StaccatoSO => AbilitySO as StaccatoSO;

    public event EventHandler OnStaccatoPerformanceStart;
    public event EventHandler OnStaccatoPerformanceEnd;

    public event EventHandler OnStaccatoBonificationStart;
    public event EventHandler OnStaccatoBonificationEnd;

    public static event EventHandler OnAnyStaccatoPerformanceStart;
    public static event EventHandler OnAnyStaccatoPerformanceEnd;

    public static event EventHandler OnAnyStaccatoBonificationStart;
    public static event EventHandler OnAnyStaccatoBonificationEnd;

    private bool isPerforming = false;

    public bool IsCurrentlyActive { get; private set; } = false;

    #region Interface Methods
    public bool IsInterruptingFacing() => isPerforming;
    public bool OverrideFacingDirection() => true;
    public Vector2 GetFacingDirection() => new Vector2(0f, -1f); //FacingDown
    /////////////////////////////////////////////////////////////////////
    public override bool IsInterruptingAttack() => isPerforming;
    /////////////////////////////////////////////////////////////////////
    public override bool IsInterruptingAbility() => isPerforming;
    #endregion

    #region Logic Methods
    protected override void HandleFixedUpdateLogic() { }
    protected override void HandleUpdateLogic() { }
    #endregion

    protected override void OnAbilityCastMethod()
    {
        base.OnAbilityCastMethod();
        HandleStaccatoTrigger();
    }

    private void HandleStaccatoTrigger()
    {
        StartCoroutine(StaccatoCoroutine());
    }

    public override bool CanCastAbility()
    {
       if(!base.CanCastAbility()) return false;
       if(IsCurrentlyActive) return false;

       return true;
    }

    private IEnumerator StaccatoCoroutine()
    {
        isPerforming = true;

        OnAnyStaccatoPerformanceStart?.Invoke(this, EventArgs.Empty);
        OnStaccatoPerformanceStart?.Invoke(this, EventArgs.Empty);

        yield return new WaitForSeconds(StaccatoSO.performanceTime);

        OnAnyStaccatoPerformanceEnd?.Invoke(this, EventArgs.Empty);
        OnStaccatoPerformanceEnd?.Invoke(this, EventArgs.Empty);

        isPerforming = false;

        OnAnyStaccatoBonificationStart?.Invoke(this, EventArgs.Empty);
        OnStaccatoBonificationStart?.Invoke(this, EventArgs.Empty);

        IsCurrentlyActive = true;

        string generatedGUID = GeneralUtilities.GenerateGUID();
        SetAbilityGUID(generatedGUID);
        TemporalNumericStatModifierManager.Instance.AddStatModifiers(generatedGUID, StaccatoSO);

        yield return new WaitForSeconds(StaccatoSO.duration);

        TemporalNumericStatModifierManager.Instance.RemoveStatModifiersByGUID(generatedGUID);
        ClearAbilityGUID();

        OnAnyStaccatoBonificationEnd?.Invoke(this, EventArgs.Empty);
        OnStaccatoBonificationEnd?.Invoke(this, EventArgs.Empty);

        IsCurrentlyActive = false;
    }

    private void SetAbilityGUID(string GUID) => activeAbilityGUID = GUID;
    private void ClearAbilityGUID() => activeAbilityGUID = "";

    public float GetBurstInterval() => StaccatoSO.burstInterval;
    public float GetBurstCount() => StaccatoSO.burstCount;

    public float GetSecondaryAttackDamagePercentage() => StaccatoSO.secondaryAttackDamagePercentage;
    public float GetSecondaryAttackInterval() => StaccatoSO.secondaryAttackInterval;
    public float GetSecondaryBurstAngleDeviation() => StaccatoSO.secondaryBurstAngleDeviation;
}
