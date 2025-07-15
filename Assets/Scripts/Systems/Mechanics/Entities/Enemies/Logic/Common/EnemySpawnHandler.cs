using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnHandler : MonoBehaviour, IPushImmuner, IAttackInterruption, IMovementInterruption
{
    [Header("Components")]
    [SerializeField] private EnemyIdentifier enemyIdentifier;
    [SerializeField] private EnemyHealth enemyHealth;

    [Header("Runtime Filled")]
    [SerializeField] private bool isSpawning;

    public bool IsSpawning => isSpawning;

    public static event EventHandler<OnEnemySpawnEventArgs> OnAnyEnemySpawnStart;
    public static event EventHandler<OnEnemySpawnEventArgs> OnAnyEnemySpawnComplete;

    public event EventHandler<OnEnemySpawnEventArgs> OnEnemySpawnStart;
    public event EventHandler<OnEnemySpawnEventArgs> OnEnemySpawnComplete;

    public class OnEnemySpawnEventArgs
    {
        public EnemySO enemySO;
    }

    private void OnEnable()
    {
        enemyHealth.OnEnemyDeath += EnemyHealth_OnEnemyDeath;
    }

    private void OnDisable()
    {
        enemyHealth.OnEnemyDeath -= EnemyHealth_OnEnemyDeath;
    }

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        isSpawning = true;

        OnAnyEnemySpawnStart?.Invoke(this, new OnEnemySpawnEventArgs { enemySO = enemyIdentifier.EnemySO });
        OnEnemySpawnStart?.Invoke(this, new OnEnemySpawnEventArgs { enemySO = enemyIdentifier.EnemySO });

        yield return new WaitForSeconds(enemyIdentifier.EnemySO.spawnDuration);

        isSpawning = false;

        OnAnyEnemySpawnComplete?.Invoke(this, new OnEnemySpawnEventArgs { enemySO = enemyIdentifier.EnemySO });
        OnEnemySpawnComplete?.Invoke(this, new OnEnemySpawnEventArgs { enemySO = enemyIdentifier.EnemySO });
    } 

    public bool IsPushImmuning() => isSpawning;
    public bool IsInterruptingMovement() => isSpawning;
    public bool StopMovementOnInterruption() => true; 
    public bool IsInterruptingAttack() => isSpawning;   

    private void EnemyHealth_OnEnemyDeath(object sender, EntityHealth.OnEntityDeathEventArgs e)
    {
        StopAllCoroutines();
        isSpawning = false;
    }
}

