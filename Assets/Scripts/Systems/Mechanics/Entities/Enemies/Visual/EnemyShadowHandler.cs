using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShadowHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject shadowGameObject;
    [Space]
    [SerializeField] private EnemySpawnHandler enemySpawnHandler;
    [SerializeField] private EnemyHealth enemyHealth;

    private void OnEnable()
    {
        enemySpawnHandler.OnEnemySpawnStart += EnemySpawningHandler_OnThisEnemySpawnStart;
        enemySpawnHandler.OnEnemySpawnComplete += EnemySpawningHandler_OnThisEnemySpawnComplete;

        enemyHealth.OnEnemyDeath += EnemyHealth_OnThisEnemyDeath;
    }

    private void OnDisable()
    {
        enemySpawnHandler.OnEnemySpawnStart -= EnemySpawningHandler_OnThisEnemySpawnStart;
        enemySpawnHandler.OnEnemySpawnComplete -= EnemySpawningHandler_OnThisEnemySpawnComplete;

        enemyHealth.OnEnemyDeath -= EnemyHealth_OnThisEnemyDeath;
    }

    private void EnableShadow() => shadowGameObject.SetActive(true);
    private void DisableShadow() => shadowGameObject.SetActive(false);

    private void EnemySpawningHandler_OnThisEnemySpawnStart(object sender, EnemySpawnHandler.OnEnemySpawnEventArgs e)
    {
        DisableShadow();
    }

    private void EnemySpawningHandler_OnThisEnemySpawnComplete(object sender, EnemySpawnHandler.OnEnemySpawnEventArgs e)
    {
        if (!enemyHealth.IsAlive()) return;
        EnableShadow();
    }

    private void EnemyHealth_OnThisEnemyDeath(object sender, EnemyHealth.OnEntityDeathEventArgs e)
    {
        DisableShadow();
    }
}
