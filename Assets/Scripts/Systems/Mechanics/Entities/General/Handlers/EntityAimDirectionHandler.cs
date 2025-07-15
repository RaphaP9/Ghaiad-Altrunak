using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityAimDirectionHandler : MonoBehaviour, IDirectionHandler
{
    [Header("Components")]
    [SerializeField] protected EntityHealth entityHealth;

    [Header("RuntimeFilled")]
    [SerializeField] protected Vector2 aimDirection;
    [SerializeField] protected float aimAngle;

    public Vector2 AimDirection => aimDirection;
    public float AimAngle => aimAngle;

    public virtual void HandleAim() //Called By the corresponding entity StateHandler: PlayerStateHandler, MeleeEnemyStateHandler, etc
    {
        if (!CanAim()) return;
        UpdateAim();
    }

    protected virtual bool CanAim()
    {
        if (!entityHealth.IsAlive()) return false;

        return true;
    }

    protected void UpdateAim()
    {
        aimDirection = CalculateAimDirection();
        aimAngle = CalculateAimAngle();
    }

    protected abstract Vector2 CalculateAimDirection();
    protected abstract float CalculateAimAngle();
    public bool IsAimingRight() => aimDirection.x >= 0;

    #region Interface Methods
    public Vector2 GetDirection() => aimDirection;
    public float GetAngle() => aimAngle;
    #endregion

}
