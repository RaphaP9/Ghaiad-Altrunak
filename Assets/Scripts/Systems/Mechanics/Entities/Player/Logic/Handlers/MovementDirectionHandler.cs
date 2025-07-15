using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementDirectionHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidbody2D;

    [Header("Settings")]
    [SerializeField] private EvaluationMode evaluationMode;

    [Header("Runtime Filled")]
    [SerializeField] private Vector2 lastMovementDirection;

    private Vector2 defaultStartingMovementDirection = new Vector2(1f,0f);

    public Vector2 Input => MovementInput.Instance.GetMovementInputNormalized();
    public Vector2 LastNonZeroInput => MovementInput.Instance.GetLastNonZeroMovementInputNormalized();
    public Vector2 LastMovementDirection => lastMovementDirection;

    private enum EvaluationMode { RigidbodyVelocity, LastNonZeroInput, CurrentDirectionInput }


    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        HandleLastMovementDirection();
    }

    private void InitializeVariables()
    {
        lastMovementDirection = defaultStartingMovementDirection.normalized;
    }

    private void HandleLastMovementDirection()
    {
        Vector2 valueToEvaluate;

        switch (evaluationMode)
        {
            case EvaluationMode.RigidbodyVelocity:
            default:
                valueToEvaluate = _rigidbody2D.velocity;
                break;
            case EvaluationMode.LastNonZeroInput:
                valueToEvaluate = LastNonZeroInput;
                break;
            case EvaluationMode.CurrentDirectionInput:
                valueToEvaluate = Input;
                break;

        }

        Vector2 rawLastMovementDirection = valueToEvaluate != Vector2.zero? valueToEvaluate : lastMovementDirection;

        lastMovementDirection = rawLastMovementDirection.normalized;
    }
}
