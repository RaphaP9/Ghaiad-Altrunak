using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFacingDirectionHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private EntityHealth entityHealth;

    [Header("Interface Components")]
    [SerializeField] private Component directionHandlerComponent;
    [SerializeField] private List<Component> facingInterruptionComponents;

    [Header("Settings")]
    [SerializeField] private FacingType facingType;
    [SerializeField] private Vector2Int startingFacingDirection;
    [SerializeField, Range(0.5f, 10f)] private float minimumRigidbodyVelocity;

    [Header("Runtime Filled")]
    [SerializeField] private Vector2 currentRawFacingDirection;
    [SerializeField] private Vector2Int currentFacingDirection;
    [Space]
    [SerializeField] private bool isInterruptingFacing;
    [SerializeField] private bool isOverridingFacing;
    [SerializeField] private Vector2 overridenDirection;

    public Vector2Int CurrentFacingDirection => currentFacingDirection;
    public Vector2 CurrentRawFacingDirection => currentRawFacingDirection;
    public bool IsInterruptingFacingDirection => isInterruptingFacing;
    public bool IsOverridingFacingDirection => isOverridingFacing;

    public float RawFacingAngle => GeneralUtilities.GetVector2AngleDegrees(currentRawFacingDirection);
    public float FacingAngle => GeneralUtilities.GetVector2AngleDegrees(currentFacingDirection);

    private IDirectionHandler directionHandler;
    private List<IFacingInterruption> facingInterruptions;

    private enum FacingType { Rigidbody, Aim }
    public Vector2 OverridenDirection => overridenDirection;

    private void Awake()
    {
        GeneralUtilities.TryGetGenericFromComponent(directionHandlerComponent, out directionHandler);
        facingInterruptions = GeneralUtilities.TryGetGenericsFromComponents<IFacingInterruption>(facingInterruptionComponents);
    }

    private void Start()
    {
        RecalculateFacingDirections(startingFacingDirection);
    }

    public void HandleFacing() //Called By the corresponding entity StateHandler: PlayerStateHandler, MeleeEnemyStateHandler, etc
    {
        HandleDirectionInterruption();
        HandleFacingDirection();
    }

    #region FacingDirectionOverride

    private void HandleDirectionInterruption()
    {
        if (!CanFace()) return;

        int interruptorsCount = 0;
        bool hasOverrider = false;

        foreach (IFacingInterruption facingInterruptionAbility in facingInterruptions)
        {
            if (facingInterruptionAbility.IsInterruptingFacing())
            {
                interruptorsCount++;

                if (facingInterruptionAbility.OverrideFacingDirection())
                {
                    isOverridingFacing = true;
                    overridenDirection = facingInterruptionAbility.GetFacingDirection();

                    RecalculateFacingDirections(overridenDirection);

                    hasOverrider = true;
                    break;
                }
            }
        }

        isInterruptingFacing = interruptorsCount > 0;

        if (hasOverrider) isOverridingFacing = true;
        else
        {
            isOverridingFacing = false;
            overridenDirection = Vector2.zero;
        }
    }

    #endregion

    #region Facing Direction Logic
    private void HandleFacingDirection()
    {
        if (!CanFace()) return;
        if (isInterruptingFacing) return;

        switch (facingType)
        {
            case FacingType.Rigidbody:
            default:
                HandleFacingDirectionByRigidbody();
                break;
            case FacingType.Aim:
                HandleFacingDirectionByAim();
                break;

        }
    }

    private void HandleFacingDirectionByRigidbody()
    {
        if (_rigidbody2D.velocity.magnitude < minimumRigidbodyVelocity) return;

        Vector2 direction = _rigidbody2D.velocity.normalized;

        if (currentRawFacingDirection != direction)
        {
            RecalculateFacingDirections(direction);
        }
    }

    private void HandleFacingDirectionByAim()
    {
        Vector2 direction = directionHandler.GetDirection();

        if (currentRawFacingDirection != direction)
        {
            RecalculateFacingDirections(direction);
        }
    }

    private void RecalculateFacingDirections(Vector2 direction)
    {
        SetCurrentRawFacingDirection(direction);
        RecalculateCurrentFacingDirection();
    }

    private void RecalculateCurrentFacingDirection() => currentFacingDirection = GeneralUtilities.ClampVector2To8Direction(currentRawFacingDirection);
    private void SetCurrentRawFacingDirection(Vector2 rawFacingDirection) => currentRawFacingDirection = rawFacingDirection;
    private void SetCurrentFacingDirection(Vector2Int facingDirection) => currentFacingDirection = facingDirection;

    public bool IsFacingRight() => currentRawFacingDirection.x >= 0;
    public bool Is8DirFacingRight() => currentFacingDirection.x >= 0;
    public bool IsFacingUp() => currentRawFacingDirection.y >= 0;
    public bool Is8DirFacingUp() => currentFacingDirection.x >= 0;

    #endregion

    protected virtual bool CanFace()
    {
        if (!entityHealth.IsAlive()) return false;

        return true;
    }
}
