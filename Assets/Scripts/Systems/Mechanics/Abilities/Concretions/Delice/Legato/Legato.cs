using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legato : ActiveAbility, IDodger, IFacingInterruption, IMovementInterruption, ICritOverrider
{
    [Header("Specific Settings")]
    [SerializeField] private LayerMask effectLayerMask;

    private LegatoSO LegatoSO => AbilitySO as LegatoSO;

    public event EventHandler OnLegatoStarting;
    public event EventHandler OnLegatoStart;
    public event EventHandler OnLegatoEnding;
    public event EventHandler OnLegatoCompleted;

    public static event EventHandler OnAnyLegatoStarting;
    public static event EventHandler OnAnyLegatoStart;
    public static event EventHandler OnAnyLegatoEnding;
    public static event EventHandler OnAnyLegatoCompleted;

    public float LegatoTimer { get; private set; }
    public float Duration => GetDuration();

    public bool IsCurrentlyActive { get; private set; } = false;

    private bool isStarting = false;
    private bool isEnding = false;

    #region Interface Methods
    public bool IsDodging() => IsCurrentlyActive;
    /////////////////////////////////////////////////////////////////////
    public bool IsInterruptingFacing() => isStarting || isEnding;
    public bool OverrideFacingDirection() => true;
    public Vector2 GetFacingDirection() => new Vector2(0f, -1f); //FacingDown
    /////////////////////////////////////////////////////////////////////
    public bool IsInterruptingMovement() => false; // isStarting || isEnding;
    public bool StopMovementOnInterruption() => true;
    /////////////////////////////////////////////////////////////////////
    public bool IsOverridingCrit() => IsCurrentlyActive && AbilityLevel == AbilityLevel.Level3;
    ////////////////////////////////////////////////////////////////////
    public override bool IsInterruptingAttack() => isStarting || isEnding;
    /////////////////////////////////////////////////////////////////////
    public override bool IsInterruptingAbility() => isStarting || isEnding;
    #endregion

    #region Logic Methods
    protected override void HandleFixedUpdateLogic() { }
    protected override void HandleUpdateLogic() { }
    #endregion

    protected override void OnAbilityCastMethod()
    {
        base.OnAbilityCastMethod();
        HandleLegatoTrigger();
    }

    private void HandleLegatoTrigger()
    {
        StartCoroutine(LegatoCoroutine());
    }

    public override bool CanCastAbility()
    {
        if (!base.CanCastAbility()) return false;
        if (IsCurrentlyActive) return false;

        return true;
    }

    private IEnumerator LegatoCoroutine()
    {
        IsCurrentlyActive = true;
        ResetTimer();

        TemporalNumericStatModifierManager.Instance.AddStatModifiers(LegatoSO.refferencialGUID, LegatoSO);

        OnAnyLegatoStarting?.Invoke(this, EventArgs.Empty);
        OnLegatoStarting?.Invoke(this, EventArgs.Empty);

        isStarting = true;

        yield return new WaitForSeconds(LegatoSO.flyStartDuration);

        isStarting = false;

        OnAnyLegatoStart?.Invoke(this, EventArgs.Empty);
        OnLegatoStart?.Invoke(this, EventArgs.Empty);


        while (LegatoTimer < GetDuration())
        {
            LegatoTimer += Time.deltaTime;
            yield return null;
        }

        OnAnyLegatoEnding?.Invoke(this, EventArgs.Empty);
        OnLegatoEnding?.Invoke(this, EventArgs.Empty);

        isEnding = true;

        yield return new WaitForSeconds(LegatoSO.flyEndDuration);

        isEnding = false;

        if(AbilityLevel != AbilityLevel.Level1) //Level1 Does not Push or deal damage
        {
            PhysicPushData pushData = new PhysicPushData(LegatoSO.pushForce, LegatoSO);
            DamageData damageData = new DamageData(LegatoSO.landDamage, false, LegatoSO, false, true, true, true);

            MechanicsUtilities.PushAllEntitiesFromPoint(GeneralUtilities.TransformPositionVector2(transform), pushData, LegatoSO.actionRadius, effectLayerMask);
            MechanicsUtilities.DealDamageInArea(GeneralUtilities.TransformPositionVector2(transform), LegatoSO.actionRadius, damageData, effectLayerMask);
        }

        yield return new WaitForSeconds(LegatoSO.dodgeTimeAfterLand);

        TemporalNumericStatModifierManager.Instance.RemoveStatModifiersByGUID(LegatoSO.refferencialGUID);

        IsCurrentlyActive = false;

        OnAnyLegatoCompleted?.Invoke(this, EventArgs.Empty);
        OnLegatoCompleted?.Invoke(this, EventArgs.Empty);
    }

    private float GetDuration() //Maybe duration is not exactly FlyDuration (Longer duration due to Ability Level,etc)
    {
        return LegatoSO.flyDuration;
    }

    private void ResetTimer() => LegatoTimer = 0;
}
