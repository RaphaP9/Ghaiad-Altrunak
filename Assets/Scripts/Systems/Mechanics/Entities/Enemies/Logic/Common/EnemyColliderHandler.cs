using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColliderHandler : EntityColliderHandler
{
    [Header("Enemy Components")]
    [SerializeField] private EnemySpawnHandler enemySpawnHandler;

    protected override void OnEnable()
    {
        base.OnEnable();
        enemySpawnHandler.OnEnemySpawnStart += EnemySpawnHandler_OnEnemySpawnStart;
        enemySpawnHandler.OnEnemySpawnComplete += EnemySpawnHandler_OnEnemySpawnComplete;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        enemySpawnHandler.OnEnemySpawnStart -= EnemySpawnHandler_OnEnemySpawnStart;
        enemySpawnHandler.OnEnemySpawnComplete -= EnemySpawnHandler_OnEnemySpawnComplete;
    }

    private void EnemySpawnHandler_OnEnemySpawnStart(object sender, EnemySpawnHandler.OnEnemySpawnEventArgs e)
    {
        DisableCollider();
    }

    private void EnemySpawnHandler_OnEnemySpawnComplete(object sender, EnemySpawnHandler.OnEnemySpawnEventArgs e)
    {
        if (!entityHealth.IsAlive()) return;
        EnableCollider();
    }
}
