using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ritardando : ActiveAbility, IFacingInterruption
{
    [Header("Specific Settings")]
    [SerializeField] private LayerMask effectLayerMask; //For both damage & slow
    [Space]
    [SerializeField] private List<Transform> coneAreaTransforms;
    [SerializeField, Range(0.5f, 1f)] private float coneAreaRadius;
    [Space]
    [SerializeField] private Transform circleAreaTransform;
    [SerializeField, Range(0.5f, 5f)] private float circleAreaRadius;

    private RitardandoSO RitardandoSO => AbilitySO as RitardandoSO;

    public event EventHandler<OnRitardandoEventArgs> OnRitardandoPerformanceStart;
    public event EventHandler<OnRitardandoEventArgs> OnRitardandoPerformanceEnd;

    public static event EventHandler<OnRitardandoEventArgs> OnAnyRitardandoPerformanceStart;
    public static event EventHandler<OnRitardandoEventArgs> OnAnyRitardandoPerformanceEnd;

    private bool isPerforming = false;

    public class OnRitardandoEventArgs: EventArgs
    {
        public AbilityLevel abilityLevel;
    }

    #region Interface Methods
    public bool IsInterruptingFacing() => false; // isPerforming;
    public bool OverrideFacingDirection() => false;
    public Vector2 GetFacingDirection() => new Vector2(0f, -1f); //Override Facing Direction is False, so this does not matter
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
        HandleRitardandoTrigger();
    }

    private void HandleRitardandoTrigger()
    {
        switch (AbilityLevel)
        {
            case AbilityLevel.NotLearned:
                break;
            case AbilityLevel.Level1:
            case AbilityLevel.Level2:
                StartCoroutine(RitardandoConeCoroutine());
                break;
            case AbilityLevel.Level3:
                StartCoroutine(RitardandoCircleCoroutine());
                break;
        }

    }

    private IEnumerator RitardandoConeCoroutine()
    {
        isPerforming = true;

        OnAnyRitardandoPerformanceStart?.Invoke(this, new OnRitardandoEventArgs { abilityLevel = AbilityLevel});
        OnRitardandoPerformanceStart?.Invoke(this, new OnRitardandoEventArgs { abilityLevel = AbilityLevel });

        yield return new WaitForSeconds(RitardandoSO.performanceTime);

        DamageData damageData = new DamageData(RitardandoSO.damage, false, RitardandoSO, false, true, true, true);

        MechanicsUtilities.DealDamageInAreas(GeneralUtilities.TransformPositionVector2List(coneAreaTransforms), coneAreaRadius, damageData, effectLayerMask); //Damage
        MechanicsUtilities.TemporalSlowInAreas(GeneralUtilities.TransformPositionVector2List(coneAreaTransforms), coneAreaRadius, RitardandoSO.tenporalSlowStatusEffect, effectLayerMask);

        if (AbilityLevel == AbilityLevel.Level2) //Level 2 Pushes
        {
            PhysicPushData pushData = new PhysicPushData(RitardandoSO.pushForce, RitardandoSO);
            MechanicsUtilities.PushEntitiesInAreasFromPoint(transform.position, pushData, GeneralUtilities.TransformPositionVector2List(coneAreaTransforms), coneAreaRadius, effectLayerMask);
        }

        isPerforming = false;

        OnAnyRitardandoPerformanceEnd?.Invoke(this, new OnRitardandoEventArgs { abilityLevel = AbilityLevel });
        OnRitardandoPerformanceEnd?.Invoke(this, new OnRitardandoEventArgs { abilityLevel = AbilityLevel });
    }

    private IEnumerator RitardandoCircleCoroutine()
    {
        isPerforming = true;

        OnAnyRitardandoPerformanceStart?.Invoke(this, new OnRitardandoEventArgs { abilityLevel = AbilityLevel });
        OnRitardandoPerformanceStart?.Invoke(this, new OnRitardandoEventArgs { abilityLevel = AbilityLevel });

        yield return new WaitForSeconds(RitardandoSO.performanceTime);

        DamageData damageData = new DamageData(RitardandoSO.damage, false, RitardandoSO, false, true, true, true);

        MechanicsUtilities.DealDamageInArea(GeneralUtilities.TransformPositionVector2(circleAreaTransform), circleAreaRadius, damageData, effectLayerMask);
        MechanicsUtilities.TemporalSlowInArea(GeneralUtilities.TransformPositionVector2(circleAreaTransform), circleAreaRadius, RitardandoSO.tenporalSlowStatusEffect, effectLayerMask);

        PhysicPushData pushData = new PhysicPushData(RitardandoSO.pushForce, RitardandoSO);
        MechanicsUtilities.PushAllEntitiesFromPoint(circleAreaTransform.position, pushData, circleAreaRadius, effectLayerMask);

        isPerforming = false;

        OnAnyRitardandoPerformanceEnd?.Invoke(this, new OnRitardandoEventArgs { abilityLevel = AbilityLevel });
        OnRitardandoPerformanceEnd?.Invoke(this, new OnRitardandoEventArgs { abilityLevel = AbilityLevel });
    }
}
