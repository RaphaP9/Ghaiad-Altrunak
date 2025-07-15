using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoldDespawnHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GoldMovement goldMovement;

    [Header("Settings")]
    [SerializeField, Range(5f, 30f)] private float minTimeToStartDespawning;
    [SerializeField, Range(5f, 30f)] private float maxToStartDespawning;
    [SerializeField, Range(5f, 10)] private float minTimeToDespawn;
    [SerializeField, Range(5f, 10)] private float maxTimeToDespawn;

    public static event EventHandler OnAnyGoldStartDespawning;
    public event EventHandler OnGoldStartDespawning;

    public static event EventHandler OnAnyGoldDespawn;
    public event EventHandler OnGoldDespawn;

    public static event EventHandler OnAnyGoldCancelDespawning;
    public event EventHandler OnGoldCancelDespawning;

    private void OnEnable()
    {
        goldMovement.OnPlayerDetected += GoldMovement_OnPlayerDetected;
        goldMovement.OnCollectionByRoundEnd += GoldMovement_OnCollectionByRoundEnd;
    }

    private void OnDisable()
    {
        goldMovement.OnPlayerDetected -= GoldMovement_OnPlayerDetected;
        goldMovement.OnCollectionByRoundEnd -= GoldMovement_OnCollectionByRoundEnd;
    }

    private void Start()
    {
        StartCoroutine(DespawnCoroutine());
    }

    private IEnumerator DespawnCoroutine()
    {
        float timeToStartDespawning = UnityEngine.Random.Range(minTimeToStartDespawning,maxToStartDespawning);

        yield return new WaitForSeconds(timeToStartDespawning);

        OnAnyGoldStartDespawning?.Invoke(this, EventArgs.Empty);
        OnGoldStartDespawning?.Invoke(this, EventArgs.Empty);

        float timeToDespawn = UnityEngine.Random.Range(minTimeToDespawn, maxTimeToDespawn);

        yield return new WaitForSeconds(timeToDespawn);

        OnAnyGoldDespawn?.Invoke(this, EventArgs.Empty);
        OnGoldDespawn?.Invoke(this, EventArgs.Empty);

        Destroy(gameObject);
    }

    private void DespawnImmediately()
    {
        OnAnyGoldDespawn?.Invoke(this, EventArgs.Empty);
        OnGoldDespawn?.Invoke(this, EventArgs.Empty);

        Destroy(gameObject);
    }

    private void GoldMovement_OnPlayerDetected(object sender, EventArgs e)
    {
        StopAllCoroutines();
        OnAnyGoldCancelDespawning?.Invoke(this, EventArgs.Empty);
        OnGoldCancelDespawning?.Invoke(this, EventArgs.Empty);
    }

    private void GoldMovement_OnCollectionByRoundEnd(object sender, EventArgs e)
    {
        StopAllCoroutines();
        OnAnyGoldCancelDespawning?.Invoke(this, EventArgs.Empty);
        OnGoldCancelDespawning?.Invoke(this, EventArgs.Empty);
    }
}
