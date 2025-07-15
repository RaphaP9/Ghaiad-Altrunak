using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesRoundHandler : RoundHandler
{
    public static WavesRoundHandler Instance { get; private set; }

    [Header("Runtime Filled")]
    [SerializeField] private WavesRoundSO currentWavesRound;
    [SerializeField] protected float currentRoundElapsedTime;
    [Space]
    [SerializeField] private int totalWaves;
    [SerializeField] private int currentWave;
    [Space]
    [SerializeField] private List<Transform> remainingEnemiesInWave;
    public float CurrentRoundElapsedTime => currentRoundElapsedTime;

    public static event EventHandler<OnWavesRoundEventArgs> OnWavesRoundStart;
    public static event EventHandler<OnWavesRoundEventArgs> OnWavesRoundCompleted;

    public static event EventHandler<OnWavesRoundWaveEventArgs> OnWavesRoundWaveStart;
    public static event EventHandler<OnWavesRoundWaveEventArgs> OnWavesRoundWaveCompleted;

    public class OnWavesRoundEventArgs : EventArgs
    {
        public WavesRoundSO wavesRoundSO;
    }

    public class OnWavesRoundWaveEventArgs : EventArgs
    {
        public WavesRoundSO wavesRoundSO;
        public int currentWave;
        public int totalWaves;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one WavesRoundHandler instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        EnemyHealth.OnAnyEnemyDeath += EnemyHealth_OnAnyEnemyDeath; //Enemy defeated when dead
    }

    private void OnDisable()
    {
        EnemyHealth.OnAnyEnemyDeath -= EnemyHealth_OnAnyEnemyDeath;
    }

    private void Start()
    {
        ClearCurrentRound();
        ClearRemainingEnemiesInWaveList();
        ResetCurrentRoundElapsedTime();
    }

    public void StartWavesRound(WavesRoundSO wavesRoundSO, StageSpawnPointsHandler stageSpawnPointsHandler)
    {
        if (currentWavesRound != null) return;

        OnRoundStartMethod(wavesRoundSO);

        SetCurrentWavesRound(wavesRoundSO);
        ResetCurrentRoundElapsedTime();

        ClearRemainingEnemiesInWaveList();

        StartCoroutine(StartRoundCoroutine(wavesRoundSO, stageSpawnPointsHandler));
    }

    private IEnumerator StartRoundCoroutine(WavesRoundSO wavesRoundSO, StageSpawnPointsHandler stageSpawnPointsHandler)
    {
        SetTotalWaves(wavesRoundSO.enemyWaves.Count);
        ResetCurrentWave();

        float roundElapsedTimer = 0f;

        while (currentWave < totalWaves)
        {
            SetCurrentWave(currentWave + 1);

            OnWavesRoundWaveStart?.Invoke(this, new OnWavesRoundWaveEventArgs { wavesRoundSO = wavesRoundSO, currentWave = currentWave, totalWaves = totalWaves });

            SpawnWaveEnemies(wavesRoundSO.enemyWaves[currentWave-1], stageSpawnPointsHandler.GetEnabledSpawnPoints());

            while (remainingEnemiesInWave.Count > 0)
            {
                roundElapsedTimer += Time.deltaTime;
                SetCurrentRoundElapsedTime(roundElapsedTimer);

                yield return null;
            }

            OnWavesRoundWaveCompleted?.Invoke(this, new OnWavesRoundWaveEventArgs { wavesRoundSO = wavesRoundSO, currentWave = currentWave, totalWaves = totalWaves });

            if (currentWave < totalWaves) yield return new WaitForSeconds(wavesRoundSO.waveSpawnInterval); //Wait interval only if there are remaining waves
        }

        CompleteCurrentRound();
    }

    protected virtual void CompleteCurrentRound()
    {
        if (currentWavesRound == null) return;

        OnRoundCompletedMethod(currentWavesRound);

        ClearRemainingEnemiesInWaveList();
        ClearCurrentRound();
        ResetCurrentRoundElapsedTime();

        EnemiesManager.Instance.ExecuteAllActiveEnemies(); //There should be no active enemies anyway
    }

    private void SpawnWaveEnemies(EnemyWave enemyWave, List<Transform> spawnPointsPool)
    {
        List<Transform> spawnedEnemies = EnemiesManager.Instance.SpawnEnemiesOnDifferentValidRandomSpawnPointsFromPool(enemyWave.enemies, spawnPointsPool);
        SetRemainingEnemiesInWaveList(spawnedEnemies);
    }

    #region Set & Get
    protected void SetCurrentWavesRound(WavesRoundSO wavesRoundSO) => currentWavesRound = wavesRoundSO;
    protected void SetCurrentRoundElapsedTime(float elapsedTime) => currentRoundElapsedTime = elapsedTime;
    protected void SetTotalWaves(int waves) => totalWaves = waves;
    protected void SetCurrentWave(int wave) => currentWave = wave;

    protected void ClearCurrentRound() => currentWavesRound = null;
    protected void ResetCurrentRoundElapsedTime() => currentRoundElapsedTime = 0;
    protected void ResetTotalWaves() => totalWaves = 0;
    protected void ResetCurrentWave() => currentWave = 0;

    protected void SetRemainingEnemiesInWaveList(List<Transform> enemyTransforms) => remainingEnemiesInWave = enemyTransforms;
    protected void RemoveEnemyFromRemainingEnemiesInWaveList(Transform enemyTransform) => remainingEnemiesInWave.Remove(enemyTransform);
    protected void ClearRemainingEnemiesInWaveList() => remainingEnemiesInWave.Clear();

    #endregion

    #region Virtual Methods
    protected override void OnRoundStartMethod(RoundSO roundSO)
    {
        base.OnRoundStartMethod(roundSO);
        OnWavesRoundStart?.Invoke(this, new OnWavesRoundEventArgs { wavesRoundSO = roundSO as WavesRoundSO });
    }

    protected override void OnRoundCompletedMethod(RoundSO roundSO)
    {
        base.OnRoundCompletedMethod(roundSO);
        OnWavesRoundCompleted?.Invoke(this, new OnWavesRoundEventArgs { wavesRoundSO = roundSO as WavesRoundSO});
    }
    #endregion

    #region Subscriptions
    private void EnemyHealth_OnAnyEnemyDeath(object sender, System.EventArgs e)
    {
        EnemyHealth enemyHealth = sender as EnemyHealth;
        RemoveEnemyFromRemainingEnemiesInWaveList(enemyHealth.transform);
    }
    #endregion
}
