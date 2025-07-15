using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : EntityMovement
{
    [Header("Enabler")]
    [SerializeField] private bool movementEnabled;

    [Header("Components")]
    [SerializeField] private CharacterIdentifier characterIdentifier;
    [SerializeField] private PlayerHealth playerHealth;
    [Space]
    [SerializeField] private CheckWall checkWall;

    #region Properties
    public Vector2 DirectionInput => MovementInput.Instance.GetMovementInputNormalized();

    public float DesiredSpeed { get; private set; }
    public float SmoothCurrentSpeed { get; private set; }

    public Vector2 SmoothDirectionInput { get; private set; }
    public Vector2 FinalMoveValue { get; private set; }
    public Vector2 ScaledMovementVector { get; private set; }
    public bool MovementEnabled => movementEnabled;
    #endregion

    #region Logic
    public void HandleMovement() //Called By PlayerStateHandler
    {
        if (!movementEnabled) return;

        CalculateDesiredSpeed();
        SmoothSpeed();

        SmoothDirection();

        CalculateFinalMovement();
        ScaleFinalMovement();
    }

    public void ApplyMovement() //Called By PlayerStateHandler
    {
        if (!CanApplyMovement()) return;

        _rigidbody2D.velocity = new Vector2(ScaledMovementVector.x, ScaledMovementVector.y);
    }

    public override void Stop() //Called By PlayerStateHandler
    {
        FinalMoveValue = Vector2.zero;
        _rigidbody2D.velocity = Vector2.zero;
    }

    private void CalculateDesiredSpeed()
    {
        DesiredSpeed = CanMove() ? GetMovementSpeedValue() : 0f;
    }

    private bool CanMove()
    {
        if (DirectionInput == Vector2.zero) return false;
        if (checkWall.HitWall) return false;
        if (!playerHealth.IsAlive()) return false;

        return true;
    }

    private void SmoothSpeed()
    {
        SmoothCurrentSpeed = Mathf.Lerp(SmoothCurrentSpeed, DesiredSpeed, Time.deltaTime * smoothVelocityFactor);
    }

    private void SmoothDirection() => SmoothDirectionInput = Vector2.Lerp(SmoothDirectionInput, DirectionInput, Time.deltaTime * smoothDirectionFactor);

    private void CalculateFinalMovement()
    {
        Vector2 finalInput = SmoothDirectionInput * SmoothCurrentSpeed;

        Vector2 roundedFinalInput;
        roundedFinalInput.x = Mathf.Abs(finalInput.x) < 0.01f ? 0f : finalInput.x;
        roundedFinalInput.y = Mathf.Abs(finalInput.y) < 0.01f ? 0f : finalInput.y;

        FinalMoveValue = roundedFinalInput;
    }

    private void ScaleFinalMovement()
    {
        ScaledMovementVector = MechanicsUtilities.ScaleVector2ToPerspective(FinalMoveValue);
    }

    #endregion

    

}
