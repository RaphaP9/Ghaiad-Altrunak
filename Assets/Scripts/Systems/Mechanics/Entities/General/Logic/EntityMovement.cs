using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityMovement : MonoBehaviour
{
    [Header("Entity Movement Components")]
    [SerializeField] protected Rigidbody2D _rigidbody2D;
    [SerializeField] protected EntityMovementSpeedStatResolver entityMovementSpeedStatResolver;
    [Space]
    [SerializeField] protected List<Component> movementInterruptorComponents;

    [Header("Smooth Settings")]
    [SerializeField, Range(1f, 100f)] protected float smoothVelocityFactor = 5f;
    [SerializeField, Range(1f, 100f)] protected float smoothDirectionFactor = 5f;

    protected List<IMovementInterruption> movementInterruptors;
    public float DistanceCovered { get; private set; } = 0f;

    protected virtual void Awake()
    {
        movementInterruptors = GeneralUtilities.TryGetGenericsFromComponents<IMovementInterruption>(movementInterruptorComponents);
    }

    protected virtual void Update()
    {
        HandleMovementStopByInterruptors();
        HandleDistanceCovered();
    }

    protected float GetMovementSpeedValue() => entityMovementSpeedStatResolver.Value;
    public float GetCurrentSpeed() => _rigidbody2D.velocity.magnitude;
    public abstract void Stop();

    protected bool CanApplyMovement()
    {
        foreach (IMovementInterruption displacementAbility in movementInterruptors)
        {
            if (displacementAbility.IsInterruptingMovement()) return false;
        }

        return true;
    }

    protected void HandleMovementStopByInterruptors()
    {
        foreach (IMovementInterruption movementInterruptor in movementInterruptors)
        {
            if (movementInterruptor.IsInterruptingMovement() && movementInterruptor.StopMovementOnInterruption()) Stop();     
        }
    }

    protected void HandleDistanceCovered()
    {
        DistanceCovered += _rigidbody2D.velocity.magnitude * Time.deltaTime;
    }
}
