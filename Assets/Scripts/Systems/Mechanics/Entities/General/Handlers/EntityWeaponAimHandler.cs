using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class EntityWeaponAimHandler : MonoBehaviour, IDirectionHandler
{
    [Header("Entity Components")]
    [SerializeField] private EntityFacingDirectionHandler facingDirectionHandler;
    [SerializeField] private EntityHealth entityHealth;
    [Space]
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private Transform refferenceAimPoint;

    [Header("Runtime Filled")]
    [SerializeField] private float pivotAimRefferenceAngle;
    [SerializeField] private float pivotAimRefferenceDistance;
    [Space]
    [SerializeField] private float pivotTargetAngle;
    [SerializeField] private float pivotTargetDistance;
    [Space]
    [SerializeField] private float pivotAngle;
    [SerializeField] private float weaponAimAngle;
    [SerializeField] private Vector2 weaponAimDirection;

    public float PivotAngle => pivotAngle;
    public float WeaponAimAngle => weaponAimAngle;
    public Vector2 WeaponAimDirection => weaponAimDirection;

    private void Start()
    {
        InitializePivotAimRefferenceValues();
    }

    private void InitializePivotAimRefferenceValues() //Values when pivot has rotation 0° (Start())
    {
        pivotAimRefferenceAngle = CalculatePivotAimRefferenceAngle();
        pivotAimRefferenceDistance = CalculatePivotAimRefferenceDistance();
    }

    protected abstract Vector2 GetTargetPosition();

    private float CalculatePivotAimRefferenceAngle() => GeneralUtilities.GetVector2AngleDegrees(GeneralUtilities.SupressZComponent(refferenceAimPoint.position - weaponPivot.position));
    private float CalculatePivotAimRefferenceDistance() => GeneralUtilities.SupressZComponent(refferenceAimPoint.position - weaponPivot.position).magnitude;
    private float CalculatePivotTargetAngle() => GeneralUtilities.GetVector2AngleDegrees(GetTargetPosition() - GeneralUtilities.SupressZComponent(weaponPivot.position));
    private float CalculatePivotTargetDistance() => (GetTargetPosition() - GeneralUtilities.SupressZComponent(weaponPivot.position)).magnitude;

    public void HandlePivotRotation() //Called By the corresponding entity StateHandler: PlayerStateHandler, MeleeEnemyStateHandler, etc
    {
        if (!entityHealth.IsAlive()) return;

        //Update Values

        pivotTargetAngle = CalculatePivotTargetAngle();
        pivotTargetDistance = CalculatePivotTargetDistance();

        //Components of the equation

        float phi = pivotAimRefferenceAngle;
        float AB = pivotAimRefferenceDistance;

        float alpha = pivotTargetAngle;
        float AC = pivotTargetDistance;

        float beta;

        #region Pivot Angle Logic

        //if AC > AB, aiming outside weapon

        if (facingDirectionHandler.IsFacingRight()) //Take in count Scale Flip when Facing Right
        {
            if (AC > AB) beta = alpha - Mathf.Asin((AB / AC) * Mathf.Sin(phi * Mathf.Deg2Rad)) * Mathf.Rad2Deg;
            else beta = alpha - phi;
        }
        else
        {
            if (AC > AB) beta = -180 + alpha + Mathf.Asin((AB / AC) * Mathf.Sin(phi * Mathf.Deg2Rad)) * Mathf.Rad2Deg;
            else beta = alpha -180 + phi;
        }

        pivotAngle = beta;

        UpdatePivotRotation();
        #endregion

        #region Aim Angle Logic

        weaponAimAngle = facingDirectionHandler.IsFacingRight() ? beta : 180 + beta;
        weaponAimDirection = GeneralUtilities.GetAngleDegreesVector2(weaponAimAngle);

        #endregion
    }

    private void UpdatePivotRotation()
    {
        weaponPivot.rotation = Quaternion.Euler(0f, 0f, pivotAngle);
    }

    #region Interface Methods
    public Vector2 GetDirection() => weaponAimDirection;
    public float GetAngle() => weaponAimAngle;
    #endregion
}
