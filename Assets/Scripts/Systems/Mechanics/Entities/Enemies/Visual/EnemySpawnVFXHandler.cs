using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemySpawnVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemySpawnHandler enemySpawnHandler;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private VisualEffect visualEffect;

    private const float LIFESPAN = 2f;

    private void OnEnable()
    {
        enemySpawnHandler.OnEnemySpawnStart += EnemySpawnHandler_OnEnemySpawnStart;
        enemySpawnHandler.OnEnemySpawnComplete += EnemySpawnHandler_OnEnemySpawnComplete;

        enemyHealth.OnEnemyDeath += EnemyHealth_OnEnemyDeath;
    }

    private void OnDisable()
    {
        enemySpawnHandler.OnEnemySpawnStart -= EnemySpawnHandler_OnEnemySpawnStart;
        enemySpawnHandler.OnEnemySpawnComplete -= EnemySpawnHandler_OnEnemySpawnComplete;

        enemyHealth.OnEnemyDeath -= EnemyHealth_OnEnemyDeath;
    }

    private void StartVFX()
    {
        visualEffect.Play();
        visualEffect.gameObject.SetActive(true);
    }

    private void EndVFX()
    {
        transform.SetParent(null);
        visualEffect.Stop();

        StartCoroutine(DestroyAfterEndCoroutine());
    }

    private IEnumerator DestroyAfterEndCoroutine()
    {
        yield return new WaitForSeconds(LIFESPAN);
        Destroy(gameObject);
    }

    private void EnemySpawnHandler_OnEnemySpawnStart(object sender, EnemySpawnHandler.OnEnemySpawnEventArgs e)
    {
        StartVFX();
    }

    private void EnemySpawnHandler_OnEnemySpawnComplete(object sender, EnemySpawnHandler.OnEnemySpawnEventArgs e)
    {
        EndVFX();
    }

    private void EnemyHealth_OnEnemyDeath(object sender, EnemyHealth.OnEntityDeathEventArgs e)
    {
        EndVFX();
    }
}
