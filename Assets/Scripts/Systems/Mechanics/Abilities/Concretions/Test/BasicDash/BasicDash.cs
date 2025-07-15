using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BasicDash : ActiveAbility, IMovementInterruption, IDodger, IFacingInterruption
{
    [Header("Specific Components")]
    [SerializeField] private MouseDirectionHandler mouseDirectionHandler;
    [SerializeField] private MovementDirectionHandler movementDirectionHandler;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    [Header("Specific Settings")]
    [SerializeField] private DirectionMode directionMode;

    [Header("Specific Runtime Filled")]
    [SerializeField] private Vector2 currentDashDirection;

    private BasicDashSO BasicDashSO => AbilitySO as BasicDashSO;

    private enum DirectionMode { MousePosition, LastMovementDirection }

    private bool isDashing = false;
    private float dashPerformTimer = 0f;
    private bool shouldDash = false;

    #region Events

    public static event EventHandler<OnPlayerDashEventArgs> OnAnyPlayerDash;
    public static event EventHandler<OnPlayerDashEventArgs> OnAnyPlayerDashCompleted;
    public static event EventHandler<OnPlayerDashEventArgs> OnAnyPlayerDashInterrupted;

    public event EventHandler<OnPlayerDashEventArgs> OnPlayerDash;
    public event EventHandler<OnPlayerDashEventArgs> OnPlayerDashCompleted;
    public event EventHandler<OnPlayerDashEventArgs> OnPlayerDashInterrupted;


    #endregion

    #region EventArgs Classes
    public class OnPlayerDashEventArgs : EventArgs
    {
        public Vector2 dashDirection;
    }
    #endregion

    #region Interface Methods
    public bool IsInterruptingMovement() => isDashing;
    public bool StopMovementOnInterruption() => false;
    public bool IsDodging() => isDashing;
    public override bool IsInterruptingAttack() => isDashing;
    public bool IsInterruptingFacing() => isDashing;
    public bool OverrideFacingDirection() => true;
    public Vector2 GetFacingDirection() => currentDashDirection;
    #endregion

    #region Logic Methods

    protected override void HandleUpdateLogic()
    {
        HandleDashResistance();

        if (!playerHealth.IsAlive() && isDashing) InterruptDash();

        if (dashPerformTimer > 0) dashPerformTimer -= Time.deltaTime;
        else if (isDashing) CompleteDash();
    }

    protected override void HandleFixedUpdateLogic()
    {
        if (!IsActiveVariant) return;

        if (!shouldDash) return;

        Dash();
        shouldDash = false;
    }

    #endregion

    #region AbilitySpecifics
    public void Dash()
    {
        SetDashPerformTimer(BasicDashSO.dashTime);

        currentDashDirection = DefineDashDirection();

        float dashForce = BasicDashSO.dashDistance / BasicDashSO.dashTime;
        Vector2 dashVector = currentDashDirection * dashForce;

        Vector2 scaledDashVector = MechanicsUtilities.ScaleVector2ToPerspective(dashVector);

        _rigidbody2D.velocity = scaledDashVector;
        isDashing = true;

        OnAnyPlayerDash?.Invoke(this, new OnPlayerDashEventArgs { dashDirection = currentDashDirection });
        OnPlayerDash?.Invoke(this, new OnPlayerDashEventArgs { dashDirection = currentDashDirection });
    }

    private void CompleteDash()
    {
        if (!isDashing) return;

        StopDash();

        OnAnyPlayerDashCompleted?.Invoke(this, new OnPlayerDashEventArgs { dashDirection = currentDashDirection });
        OnPlayerDashCompleted?.Invoke(this, new OnPlayerDashEventArgs { dashDirection = currentDashDirection });
    }

    private void InterruptDash()
    {
        if (!isDashing) return;

        StopDash();

        OnAnyPlayerDashInterrupted?.Invoke(this, new OnPlayerDashEventArgs { dashDirection = currentDashDirection });
        OnPlayerDashInterrupted?.Invoke(this, new OnPlayerDashEventArgs { dashDirection = currentDashDirection });
    }

    private void StopDash()
    {
        isDashing = false;
        _rigidbody2D.velocity = Vector2.zero;

        currentDashDirection = Vector2.zero;
        ResetDashPerformTimer();
    }

    private Vector2 DefineDashDirection()
    {
        switch (directionMode)
        {
            case DirectionMode.MousePosition:
                return mouseDirectionHandler.NormalizedMouseDirection;
            case DirectionMode.LastMovementDirection:
            default:
                return movementDirectionHandler.LastMovementDirection;
        }
    }

    private void HandleDashResistance()
    {
        if (!isDashing) return;

        Vector2 dashResistanceForce = -currentDashDirection * BasicDashSO.dashResistance;
        _rigidbody2D.AddForce(dashResistanceForce, ForceMode2D.Force);
    }

    private void SetDashPerformTimer(float time) => dashPerformTimer = time;
    private void ResetDashPerformTimer() => dashPerformTimer = 0f;

    #endregion

    #region Abstract Methods
    protected override void OnAbilityCastMethod()
    {
        base.OnAbilityCastMethod();
        shouldDash = true;
    }

    protected override void OnAbilityVariantDeactivationMethod()
    {
        base.OnAbilityVariantDeactivationMethod();
        CompleteDash();
    }
    #endregion
}
