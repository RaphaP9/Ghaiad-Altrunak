using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPointsManager : MonoBehaviour
{
    public static EnemySpawnPointsManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField, Range(2f, 10f)] private float minDistanceToPlayer;
    [SerializeField, Range(5f, 20f)] private float maxDistanceToPlayer;

    [Header("Debug")]
    [SerializeField] private Color gizmosColor;
    [SerializeField] private bool debug;

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
            Debug.LogWarning("There is more than one EnemySpawnPointValidator instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region SpawnPoint Validation

    public List<Transform> GetXValidSpawnPointsFromPool(List<Transform> spawnPointsPool, int quantity)
    {
        List<Transform> chosenSpawnPoints = new List<Transform>();
        List<Transform> validSpawnPointsPool = GetValidSpawnPointsFromPool(spawnPointsPool);

        for (int i=0; i<quantity; ++i)
        {
            if(validSpawnPointsPool.Count <= 0)
            {
                if (debug) Debug.Log("Not Enough different spawnPoints. Returning all available");
                return chosenSpawnPoints;
            }

            Transform chosenSpawnPoint = ChooseRandomEnemySpawnPoint(validSpawnPointsPool);
            validSpawnPointsPool.Remove(chosenSpawnPoint);
            chosenSpawnPoints.Add(chosenSpawnPoint);
        }

        return chosenSpawnPoints;   
    }

    public Transform GetRandomValidSpawnPointFromPool(List<Transform> spawnPointsPool)
    {  
        Transform chosenSpawnPoint = ChooseRandomEnemySpawnPoint(GetValidSpawnPointsFromPool(spawnPointsPool));
        return chosenSpawnPoint;
    }

    public List<Transform> GetValidSpawnPointsFromPool(List<Transform> spawnPointsPool)
    {
        List<Transform> validSpawnPoints = FilterValidEnemySpawnPointsByMinMaxDistanceRange(spawnPointsPool, minDistanceToPlayer, maxDistanceToPlayer);
        return validSpawnPoints;
    }

    private List<Transform> FilterValidEnemySpawnPointsByMinMaxDistanceRange(List<Transform> spawnPointsPool, float minDistance, float maxDistance)
    {
        List<Transform> validSpawnPoints = new List<Transform>();

        foreach (Transform spawnPoint in spawnPointsPool)
        {
            if (!EnemySpawnPointOnMinDistanceRange(spawnPoint, minDistance)) continue;
            if (!EnemySpawnPointOnMaxDistanceRange(spawnPoint, maxDistance)) continue;

            validSpawnPoints.Add(spawnPoint);
        }

        return validSpawnPoints;
    }

    private List<Transform> FilterValidEnemySpawnPointsByMinDistanceRange(List<Transform> spawnPointsPool, float minDistance)
    {
        List<Transform> validSpawnPoints = new List<Transform>();

        foreach (Transform spawnPoint in spawnPointsPool)
        {
            if (!EnemySpawnPointOnMinDistanceRange(spawnPoint, minDistance)) continue;

            validSpawnPoints.Add(spawnPoint);
        }

        return validSpawnPoints;
    }

    private List<Transform> ChooseValidEnemySpawnPointsByMaxDistanceRange(List<Transform> spawnPointsPool, float maxDistance)
    {
        List<Transform> validSpawnPoints = new List<Transform>();

        foreach (Transform spawnPoint in spawnPointsPool)
        {
            if (!EnemySpawnPointOnMaxDistanceRange(spawnPoint, maxDistance)) continue;

            validSpawnPoints.Add(spawnPoint);
        }

        return validSpawnPoints;
    }

    private Transform ChooseRandomEnemySpawnPoint(List<Transform> enemySpawnPointsPool)
    {
        Transform enemySpawnPoint = GeneralUtilities.ChooseRandomElementFromList(enemySpawnPointsPool);
        return enemySpawnPoint;
    }
    private bool EnemySpawnPointOnMinDistanceRange(Transform enemySpawnPoint, float minDistance)
    {
        if (Vector2.Distance(GeneralUtilities.TransformPositionVector2(enemySpawnPoint), GeneralUtilities.TransformPositionVector2(PlayerTransformRegister.Instance.PlayerTransform)) > minDistance) return true;
        return false;
    }

    private bool EnemySpawnPointOnMaxDistanceRange(Transform enemySpawnPoint, float maxDistance)
    {
        if (Vector2.Distance(GeneralUtilities.TransformPositionVector2(enemySpawnPoint), GeneralUtilities.TransformPositionVector2(PlayerTransformRegister.Instance.PlayerTransform)) < maxDistance) return true;
        return false;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;

        Vector3 position = Vector3.zero;

        if(PlayerTransformRegister.Instance != null && PlayerTransformRegister.Instance.PlayerTransform != null)
        {
            position = PlayerTransformRegister.Instance.PlayerTransform.position;   
        }

        Gizmos.DrawWireSphere(position, minDistanceToPlayer);
        Gizmos.DrawWireSphere(position, maxDistanceToPlayer);
    }
}
