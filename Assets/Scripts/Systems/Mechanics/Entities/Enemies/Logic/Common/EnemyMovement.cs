using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : EntityMovement
{
    [Header("Components")]
    [SerializeField] private PlayerRelativeHandler playerRelativeHandler;
    [SerializeField] private Collider2D _collider2D;

    [Header("Terrain Avoidance")]
    [SerializeField] private LayerMask terrainLayerMask;
    [SerializeField, Range(0f, 2f)] private float terrainAvoidanceDetectionRadius;
    [SerializeField, Range(0f, 1f)] private float terrainAvoidanceWeight;

    [Header("Enemy Avoidance")]
    [SerializeField] private LayerMask enemyAvoidanceLayerMask;
    [SerializeField, Range (0f,2f)] private float enemyAvoidanceDetectionRadius;
    [SerializeField, Range (0f,1f)] private float enemyAvoidanceWeight;

    private const int MAX_AVOIDANCE_COUNT = 10;
    private const float AVOID_ENEMY_THRESHOLD_DISTANCE = 0.1f;
    private const float AVOID_TERRAIN_THRESHOLD_DISTANCE = 0.01f;

    public override void Stop()
    {
        _rigidbody2D.velocity = Vector2.zero;
    }

    protected void MoveTowardsDirection(Vector2 direction)
    {
        if (!CanApplyMovement()) return;

        Vector2 normalizedDirection = direction.normalized;
        _rigidbody2D.velocity = normalizedDirection * GetMovementSpeedValue();
    }

    protected void MoveTowardsPosition(Vector2 targetPosition)
    {
        if (!CanApplyMovement()) return;

        Vector2 direction = targetPosition - GeneralUtilities.TransformPositionVector2(transform);
        direction.Normalize();
        _rigidbody2D.velocity = direction * GetMovementSpeedValue();
    }

    public void MoveAwayFromPlayerDirection() => MoveTowardsDirection(-playerRelativeHandler.DirectionToPlayer);

    public void MoveTowardsPlayerDirection()
    {
        if (!CanApplyMovement()) return;

        Vector2 enemyAvoidanceDirection = CalculateAvoidanceVector(enemyAvoidanceDetectionRadius, enemyAvoidanceLayerMask, enemyAvoidanceWeight);
        Vector2 terrainAvoidanceDirection = CalculateTerrainRepulsionFromNearest(terrainAvoidanceDetectionRadius, terrainLayerMask, terrainAvoidanceWeight);
        Vector2 finalDirection = (playerRelativeHandler.DirectionToPlayer + enemyAvoidanceDirection + terrainAvoidanceDirection).normalized;

        _rigidbody2D.velocity = Vector2.Lerp(_rigidbody2D.velocity, finalDirection * GetMovementSpeedValue(), Time.deltaTime * smoothVelocityFactor);
    }

    public void MoveTowardsPlayerPosition() => MoveTowardsPosition(playerRelativeHandler.PlayerPosition);
    public void StopOnCurrentPosition() => Stop();

    private Vector2 CalculateAvoidanceVector(float radius, LayerMask avoidanceLayerMask, float weight)
    {
        Collider2D[] results = new Collider2D[MAX_AVOIDANCE_COUNT];
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, radius, results, avoidanceLayerMask);

        Vector2 separation = Vector2.zero;
        int validCount = 0;

        for (int i = 0; i < count; i++)
        {
            Collider2D col = results[i];
            if (col == null || col == _collider2D) continue;

            Vector2 directionFromAvoidee = GeneralUtilities.Vector3ToVector2(transform.position - col.transform.position);
            float distance = directionFromAvoidee.magnitude;

            if (distance > AVOID_ENEMY_THRESHOLD_DISTANCE)
            {
                separation += directionFromAvoidee.normalized / distance;
                validCount++;
            }
        }

        if (validCount > 0) separation = separation.normalized * weight;
        return separation;
    }

    private Vector2 CalculateTerrainRepulsionFromNearest(float terrainAvoidanceDetectionRadius, LayerMask terrainLayerMask, float terrainAvoidanceWeight)
    {
        Collider2D[] terrainResults = new Collider2D[1]; //Only the first terrain detected
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, terrainAvoidanceDetectionRadius, terrainResults, terrainLayerMask);

        if (count == 0) return Vector2.zero;

        Collider2D nearestTerrain = terrainResults[0];
        if (nearestTerrain == null) return Vector2.zero;

        Vector2 selfPosition = transform.position;
        Vector2 closestPoint = nearestTerrain.ClosestPoint(selfPosition);
        Vector2 awayDirection = selfPosition - closestPoint;

        float distance = awayDirection.magnitude;

        if (distance > AVOID_TERRAIN_THRESHOLD_DISTANCE)
        {
            return awayDirection.normalized * (terrainAvoidanceWeight / distance);
        }

        return Vector2.zero;
    }
}
