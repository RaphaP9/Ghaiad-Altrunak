using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMovement : ThrowMovement
{
    [Header("PlayerDetection Settings")]
    [SerializeField, Range(0.5f, 7f)] private float basePlayerDetectionRange;
    [SerializeField, Range(1f, 100f)] private float moveTowardsPlayerSmoothFactor;

    [Header("Collection By Round End Settings")]
    [SerializeField, Range(1f, 100f)] private float moveTowardsPlayerRoundEndSmoothFactor;

    private bool playerDetected = false;
    private bool collectionByRoundEnd = false;

    public static event EventHandler OnAnyPlayerDetected;
    public event EventHandler OnPlayerDetected;

    public static event EventHandler OnAnyCollectionByRoundEnd;
    public event EventHandler OnCollectionByRoundEnd;

    private void OnEnable()
    {
        GameManager.OnStateChanged += GameManager_OnStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= GameManager_OnStateChanged;
    }

    private void Update()
    {
        HandlePlayerDetection();
        HandleMoveTowardsPlayer();
        HandleMoveTowardsPlayerRoundEnd();
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
        if (collectionByRoundEnd) return;
        if (!playerDetected) return;
        if (PlayerTransformRegister.Instance == null) return;
        if (PlayerTransformRegister.Instance.PlayerTransform == null) return;

        transform.position = Vector3.Lerp(transform.position, PlayerTransformRegister.Instance.PlayerTransform.position, moveTowardsPlayerSmoothFactor*Time.deltaTime);
    }

    private void HandleMoveTowardsPlayerRoundEnd()
    {
        if (!collectionByRoundEnd) return;
        if (PlayerTransformRegister.Instance == null) return;
        if (PlayerTransformRegister.Instance.PlayerTransform == null) return;

        transform.position = Vector3.Lerp(transform.position, PlayerTransformRegister.Instance.PlayerTransform.position, moveTowardsPlayerRoundEndSmoothFactor * Time.deltaTime);
    }

    private void GameManager_OnStateChanged(object sender, GameManager.OnStateChangeEventArgs e)
    {
        if (e.newState != GameManager.State.EndingCombat) return;

        collectionByRoundEnd = true;
        OnAnyCollectionByRoundEnd?.Invoke(this, EventArgs.Empty);
        OnCollectionByRoundEnd?.Invoke(this, EventArgs.Empty);
    }
}
