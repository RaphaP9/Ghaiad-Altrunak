using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoldDropperManager : MonoBehaviour
{
    public static GoldDropperManager Instance { get; private set; }

    [Header("Setting")]
    [SerializeField] private List<GoldValuePrefab> goldValuePrefabs;

    [Header("Debug")]
    [SerializeField] private bool debug;

    [System.Serializable]
    public class GoldValuePrefab
    {
        public Transform goldPrefab;
        public int value;
    }

    private void OnEnable()
    {
        EnemyHealth.OnAnyEnemyDeath += EnemyHealth_OnAnyEnemyDeath;
    }

    private void OnDisable()
    {
        EnemyHealth.OnAnyEnemyDeath -= EnemyHealth_OnAnyEnemyDeath;
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
            Debug.LogWarning("There is more than one EntityGoldDropManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void DropEntityGoldAtPosition(int goldAmount, Vector2 entityPosition)
    {
        int goldAlreadyDropped = 0;

        foreach (GoldValuePrefab goldValuePrefab in goldValuePrefabs)
        {
            if (goldAlreadyDropped == goldAmount) break;

            int prefabsToDrop = (goldAmount- goldAlreadyDropped) / goldValuePrefab.value;
            if (prefabsToDrop <= 0) continue;

            for (int i = 0; i < prefabsToDrop; i++)
            {
                CreateGoldPrefabAtPosition(goldValuePrefab.goldPrefab, entityPosition);
                goldAlreadyDropped += goldValuePrefab.value;
            }
        }
    }

    private void CreateGoldPrefabAtPosition(Transform prefab, Vector2 position)
    {
        Transform goldTransform = Instantiate(prefab, GeneralUtilities.Vector2ToVector3(position), Quaternion.identity);
    }

    private void HandleGoldDrop(object sender, EntityHealth.OnEntityDeathEventArgs e)
    {
        if (e.damageSource.GetDamageSourceClassification() != DamageSourceClassification.Character) return; //Must have been killed by player

        int goldAmount = (e.entitySO as EnemySO).goldDrop;
        Vector2 position = GeneralUtilities.Vector3ToVector2((sender as EntityHealth).transform.position);

        DropEntityGoldAtPosition(goldAmount, position);
    }

    #region Subscriptions
    private void EnemyHealth_OnAnyEnemyDeath(object sender, EntityHealth.OnEntityDeathEventArgs e)
    {
        HandleGoldDrop(sender, e);
    }
    #endregion
}
