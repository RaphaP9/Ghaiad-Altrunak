using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSpawnPointsHandler : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<SpawnPointHandler> stageSpawnPoints;
    [SerializeField] private Transform specialSpawnPoint;

    public List<SpawnPointHandler> StageSpawnPoints => stageSpawnPoints;
    public Transform SpecialSpawnPoint => specialSpawnPoint;

    public List<Transform> GetEnabledSpawnPoints()
    {
        List<Transform> enabledSpawnPoints = new List<Transform>();

        foreach(SpawnPointHandler spawnPoint in stageSpawnPoints)
        {
            if (spawnPoint.IsEnabled) enabledSpawnPoints.Add(spawnPoint.transform);
        }

        return enabledSpawnPoints;
    }

    public void EnableSpawnPoints()
    {
        foreach(SpawnPointHandler spawnPoint in stageSpawnPoints)
        {
            spawnPoint.SetIsEnabled(true);
        }
    }

    public void DisableSpawnPoints()
    {
        foreach (SpawnPointHandler spawnPoint in stageSpawnPoints)
        {
            spawnPoint.SetIsEnabled(false);
        }
    }
}
