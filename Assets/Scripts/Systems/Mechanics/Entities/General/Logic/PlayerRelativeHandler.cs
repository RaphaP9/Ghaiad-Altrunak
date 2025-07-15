using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRelativeHandler : MonoBehaviour
{
    [Header("RuntimeFilled")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector2 playerPosition;
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private Vector2 directionToPlayer;
    [SerializeField] private float angleToPlayer;

    public Transform PlayerTransform => playerTransform;
    public Vector2 PlayerPosition => playerPosition;
    public float DistanceToPlayer => distanceToPlayer;
    public Vector2 DirectionToPlayer => directionToPlayer;
    public float AngleToPlayer => angleToPlayer;

    private void Update()
    {
        HandleRelativesToPlayer();
    }

    private void HandleRelativesToPlayer()
    {
        if (PlayerTransformRegister.Instance.PlayerTransform == null) return;

        playerTransform = PlayerTransformRegister.Instance.PlayerTransform;
        playerPosition = PlayerTransformRegister.Instance.PlayerTransform.position;
        distanceToPlayer = Vector2.Distance(GetPosition(), GetPlayerPosition());
        directionToPlayer = (GetPlayerPosition() - GetPosition()).normalized;
        angleToPlayer = GeneralUtilities.Vector2ToAngleDegrees(directionToPlayer);
    }

    private Vector2 GetPlayerPosition()
    {
        return GeneralUtilities.TransformPositionVector2(PlayerTransformRegister.Instance.PlayerTransform);
    }

    private Vector2 GetPosition()
    {
        return GeneralUtilities.TransformPositionVector2(transform);
    }
}
