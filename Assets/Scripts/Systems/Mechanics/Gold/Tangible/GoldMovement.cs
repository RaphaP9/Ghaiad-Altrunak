using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMovement : ThrowMovement
{
    [Header("PlayerDetection Settings")]
    [SerializeField, Range(0.5f, 7f)] private float basePlayerDetectionRange;
    [SerializeField, Range(1f, 100f)] private float moveTowardsPlayerSmoothFactor;

    private bool playerDetected = false;

    public static event EventHandler OnAnyPlayerDetected;
    public event EventHandler OnPlayerDetected;

    private void Update()
    {
        HandlePlayerDetection();
        HandleMoveTowardsPlayer();
    }

    private void HandlePlayerDetection()
    {
        if (playerDetected) return;
        if (PlayerTransformRegister.Instance == null) return;
        if(PlayerTransformRegister.Instance.PlayerTransform == null) return;

        float detectionRange = basePlayerDetectionRange;

        if(Vector2.Distance(transform.position, PlayerTransformRegister.Instance.PlayerTransform.position)< basePlayerDetectionRange)
        {
            playerDetected = true;
            OnAnyPlayerDetected?.Invoke(this, EventArgs.Empty);
            OnPlayerDetected?.Invoke(this, EventArgs.Empty);
        }
    }

    private void HandleMoveTowardsPlayer()
    {
        if (!playerDetected) return;
        if (PlayerTransformRegister.Instance == null) return;
        if (PlayerTransformRegister.Instance.PlayerTransform == null) return;

        transform.position = Vector3.Lerp(transform.position, PlayerTransformRegister.Instance.PlayerTransform.position, moveTowardsPlayerSmoothFactor*Time.deltaTime);
    }
}
