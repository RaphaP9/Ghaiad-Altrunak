using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCleanupHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyIdentifier enemyIdentifier;
    [SerializeField] private EnemyHealth enemyHealth;

    public static event EventHandler<OnEnemyCleanUpEventArgs> OnAnyEnemyCleanup;
    public event EventHandler<OnEnemyCleanUpEventArgs> OnEnemyCleanup;

    public class OnEnemyCleanUpEventArgs
    {
        public EnemySO enemySO;
        public Transform enemyTransform;
    }

    private void OnEnable()
    {
        enemyHealth.OnEnemyDeath += EnemyHealth_OnEnemyDeath;
    }

    private void OnDisable()
    {
        enemyHealth.OnEnemyDeath -= EnemyHealth_OnEnemyDeath;
    }

    private IEnumerator CleanUpEnemyCoroutine()
    {
        yield return new WaitForSeconds(enemyIdentifier.EnemySO.cleanupTime);

        OnEnemyCleanup?.Invoke(this, new OnEnemyCleanUpEventArgs { enemySO = enemyIdentifier.EnemySO, enemyTransform = transform});
        OnAnyEnemyCleanup?.Invoke(this, new OnEnemyCleanUpEventArgs { enemySO = enemyIdentifier.EnemySO, enemyTransform = transform });

        Destroy(gameObject);
    }

    private void EnemyHealth_OnEnemyDeath(object sender, System.EventArgs e)
    {
        StartCoroutine(CleanUpEnemyCoroutine());
    }
}
