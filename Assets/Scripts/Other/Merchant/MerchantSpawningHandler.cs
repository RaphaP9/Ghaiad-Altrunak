using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantSpawningHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool spawnMerchantOnInitialization;
    [Space]
    [SerializeField, Range(1f, 7f)] private float minDistanceFromPlayerToSpawn;
    [SerializeField, Range(3f, 10f)] private float maxDistanceFromPlayerToSpawn;
    [Space]
    [SerializeField, Range(0f, 2f)] private float stateChangeTimeToSpawn;
    [SerializeField, Range(0f, 2f)] private float stateInitializationTimeToSpawn;
    [SerializeField, Range(0f, 2f)] private float timeToDespawn;

    [Header("Debug")]
    [SerializeField] private Color gizmosColor;
    [SerializeField] private bool debug;

    public static event EventHandler<OnMerchantSpawnEventArgs> OnMerchantSpawn;
    public static event EventHandler OnMerchantDespawn;

    public class OnMerchantSpawnEventArgs : EventArgs
    {
        public Vector2 position;
    }

    private void OnEnable()
    {
        GameManager.OnStateInitialized += GameManager_OnStateInitialized;
        GameManager.OnStateChanged += GameManager_OnStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnStateInitialized -= GameManager_OnStateInitialized;
        GameManager.OnStateChanged -= GameManager_OnStateChanged;
    }

    private void SpawnMerchantLogic(float timeToSpawn)
    {
        StartCoroutine(SpawnMerchantCoroutine(timeToSpawn));
    }

    private void DespawnMerchantLogic(float timeToDespawn)
    {
        StartCoroutine(DespawnMerchantCoroutine(timeToDespawn));
    }

    private IEnumerator SpawnMerchantCoroutine(float timeToSpawn)
    {
        yield return new WaitForSeconds(timeToSpawn);

        if (!StageEventsDefiner.Instance.OpenShopOnThisRound()) yield break;
        if (GeneralStagesManager.Instance.HasCompletedAllRounds) yield break;

        Vector2 spawnPosition = Vector2.zero;
        bool positionFound = ChoosePositionToSpawn(out spawnPosition);

        if (!positionFound) yield break;

        OnMerchantSpawn?.Invoke(this, new OnMerchantSpawnEventArgs{ position = spawnPosition });
    }

    private IEnumerator DespawnMerchantCoroutine(float timeToDespawn)
    {
        yield return new WaitForSeconds(timeToDespawn);
        OnMerchantDespawn?.Invoke(this, EventArgs.Empty);
    }

    private bool ChoosePositionToSpawn(out Vector2 chosenPosition)
    {
        List<Transform> totalSpawnPoints = GeneralStagesManager.Instance.CurrentStageGroup.stageHandler.StageSpawnPointsHandler.GetEnabledSpawnPoints(); //We Will Use same spawnpoints as Enemy SpawnPoints

        List<Transform> validSpawnPoints = new List<Transform>();

        foreach (Transform spawnPoint in totalSpawnPoints)
        {
            if (Vector3.Distance(spawnPoint.position, PlayerTransformRegister.Instance.PlayerTransform.position) < minDistanceFromPlayerToSpawn) continue;
            if (Vector3.Distance(spawnPoint.position, PlayerTransformRegister.Instance.PlayerTransform.position) > maxDistanceFromPlayerToSpawn) continue;

            validSpawnPoints.Add(spawnPoint);
        }

        if(validSpawnPoints.Count <= 0)
        {
            if (debug) Debug.Log("Could not find a valid SpawnPoint for Merchant.");
            chosenPosition = Vector2.zero;
            return false;
        }

        Transform chosenSpawnPoint = GeneralUtilities.ChooseRandomElementFromList(validSpawnPoints);
        chosenPosition = GeneralUtilities.TransformPositionVector2(chosenSpawnPoint);
        return true;
    }

    #region Subscriptions
    private void GameManager_OnStateInitialized(object sender, GameManager.OnStateInitializedEventArgs e)
    {
        if (e.state == GameManager.State.StartingGame && spawnMerchantOnInitialization)
        {
            SpawnMerchantLogic(stateInitializationTimeToSpawn);
        }
    }

    private void GameManager_OnStateChanged(object sender, GameManager.OnStateChangeEventArgs e)
    {
        if(e.newState == GameManager.State.EndingCombat)
        {
            SpawnMerchantLogic(stateChangeTimeToSpawn);
            return;
        }

        if(e.previousState == GameManager.State.Shop)
        {
            DespawnMerchantLogic(timeToDespawn);
            return;
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;

        Vector3 position = Vector3.zero;

        if (PlayerTransformRegister.Instance != null && PlayerTransformRegister.Instance.PlayerTransform != null)
        {
            position = PlayerTransformRegister.Instance.PlayerTransform.position;
        }

        Gizmos.DrawWireSphere(position, minDistanceFromPlayerToSpawn);
        Gizmos.DrawWireSphere(position, maxDistanceFromPlayerToSpawn);
    }
}
