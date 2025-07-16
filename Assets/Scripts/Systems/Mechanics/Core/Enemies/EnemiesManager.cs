using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public static EnemiesManager Instance { get; private set; }

    [Header("List - Runtime Filled")]
    [SerializeField] private List<Transform> activeEnemies;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public static event EventHandler<OnEnemySpawnedEventArgs> OnEnemySpawned;
    public class OnEnemySpawnedEventArgs : EventArgs
    {
        public EnemySO enemySO;
        public Vector2 position;
    }

    private void OnEnable()
    {
        EnemyCleanupHandler.OnAnyEnemyCleanup += EnemyCleanupHandler_OnAnyEnemyCleanup;
    }

    private void OnDisable()
    {
        EnemyCleanupHandler.OnAnyEnemyCleanup -= EnemyCleanupHandler_OnAnyEnemyCleanup;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one EnemySpawnerManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public List<Transform> SpawnEnemiesOnDifferentValidRandomSpawnPointsFromPool(List<EnemySO> enemySOs, List<Transform> spawnPointsPool)
    {
        List<Transform> spawnPoints = EnemySpawnPointsManager.Instance.GetXValidSpawnPointsFromPool(spawnPointsPool, enemySOs.Count);
        List<Transform> spawnedEnemies = new List<Transform>();

        if(spawnPoints.Count < enemySOs.Count)
        {
            if (debug) Debug.Log("There are less spawnPoints than enemies to spawn.");
        }

        int spawnedEnemyIndex = 0;

        foreach (Transform spawnPoint in spawnPoints)
        {
            Transform spawnedEnemy = SpawnEnemyAtPosition(enemySOs[spawnedEnemyIndex], spawnPoint.position);
            spawnedEnemies.Add(spawnedEnemy);

            spawnedEnemyIndex++;
        }

        return spawnedEnemies;
    }

    public List<Transform> SpawnEnemiesOnValidRandomSpawnPointFromPool(List<EnemySO> enemySOs, List<Transform> spawnPointsPool)
    {
        List<Transform> spawnedEnemies = new List<Transform>();

        foreach (EnemySO enemySO in enemySOs)
        {
            Transform spawnedEnemy = SpawnEnemyOnValidRandomSpawnPointFromPool(enemySO, spawnPointsPool);
            spawnedEnemies.Add(spawnedEnemy);
        }

        return spawnedEnemies;
    }

    public Transform SpawnEnemyOnValidRandomSpawnPointFromPool(EnemySO enemySO, List<Transform> spawnPointsPool)
    {
        Transform chosenSpawnPoint = EnemySpawnPointsManager.Instance.GetRandomValidSpawnPointFromPool(spawnPointsPool);

        if(chosenSpawnPoint == null)
        {
            if (debug) Debug.Log("Chosen SpawnPoint is null. Spawn will be ignored");
            return null;
        }

        Transform spawnedEnemy = SpawnEnemyAtPosition(enemySO, chosenSpawnPoint.position);

        return spawnedEnemy;
    }

    

    public Transform SpawnEnemyAtPosition(EnemySO enemySO, Vector3 position)
    {
        if(enemySO.prefab == null)
        {
            if (debug) Debug.Log($"EnemySO with name {enemySO.entityName} does not contain an enemy prefab. Instantiation will be ignored.");
            return null;
        }

        Transform spawnedEnemy = Instantiate(enemySO.prefab, position, Quaternion.identity);
        RegisterEnemyIntoActiveEnemyList(spawnedEnemy);

        OnEnemySpawned?.Invoke(this, new OnEnemySpawnedEventArgs { enemySO = enemySO, position = position });

        return spawnedEnemy;
    }

    #region Enemy Execution

    public void ExecuteAllEnemiesOnScene() //Uses Find Objects Of Type!
    {
        EnemyHealth[] enemyHealths = GameObject.FindObjectsOfType<EnemyHealth>();

        foreach (EnemyHealth enemyHealth in enemyHealths)
        {
            SelfExecuteDamageData selfExecuteDamageData = new SelfExecuteDamageData(true, false);

            enemyHealth.SelfExecute(selfExecuteDamageData);
        }
    }

    public void ExecuteAllActiveEnemies()
    {
        List<EnemyHealth> enemyHealths = GeneralUtilities.TryGetGenericsFromTransforms<EnemyHealth>(activeEnemies);

        foreach (EnemyHealth enemyHealth in enemyHealths)
        {
            SelfExecuteDamageData selfExecuteDamageData = new SelfExecuteDamageData(true, false);

            enemyHealth.SelfExecute(selfExecuteDamageData);
        }
    }
    #endregion

    private void RegisterEnemyIntoActiveEnemyList(Transform enemy)
    {
        activeEnemies.Add(enemy);
    }

    private void RemoveEnemyFromActiveEnemyList(Transform enemy)
    {
        activeEnemies.Remove(enemy);
    }

    public int GetActiveEnemiesCount() => activeEnemies.Count;

    #region Subscriptions
    private void EnemyCleanupHandler_OnAnyEnemyCleanup(object sender, EnemyCleanupHandler.OnEnemyCleanUpEventArgs e)
    {
        RemoveEnemyFromActiveEnemyList(e.enemyTransform);    
    }
    #endregion
}
