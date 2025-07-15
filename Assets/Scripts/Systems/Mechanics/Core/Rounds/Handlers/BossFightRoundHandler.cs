using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightRoundHandler : RoundHandler
{
    public static BossFightRoundHandler Instance { get; private set; }

    [Header("Runtime Filled")]
    [SerializeField] private BossFightRoundSO currentBossFightRound;
    [SerializeField] protected float currentRoundElapsedTime;
    [Space]
    [SerializeField] private Transform currentBossTransform;

    public static event EventHandler<OnBossFightRoundEventArgs> OnBossFightRoundStart;
    public static event EventHandler<OnBossFightRoundEventArgs> OnBossFightRoundCompleted;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one BossFightRoundHandler instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        EnemyCleanupHandler.OnAnyEnemyCleanup += EnemyCleanupHandler_OnAnyEnemyCleanup; //BossDefeated when cleanup
    } 

    private void OnDisable()
    {
        EnemyCleanupHandler.OnAnyEnemyCleanup -= EnemyCleanupHandler_OnAnyEnemyCleanup;
    }

    public class OnBossFightRoundEventArgs : EventArgs
    {
        public BossFightRoundSO bossFightRoundSO;
    }

    private void Start()
    {
        ClearCurrentRound();
        ClearCurrentBossTransform();
        ResetCurrentRoundElapsedTime();
    }

    public void StartBossFightRound(BossFightRoundSO bossFightRoundSO, StageSpawnPointsHandler stageSpawnPointsHandler)
    {
        if (currentBossFightRound != null) return;

        OnRoundStartMethod(bossFightRoundSO);

        SetCurrentBossFightRound(bossFightRoundSO);
        ClearCurrentBossTransform();
        ResetCurrentRoundElapsedTime();

        StartCoroutine(StartRoundCoroutine(bossFightRoundSO, stageSpawnPointsHandler));
    }

    private IEnumerator StartRoundCoroutine(BossFightRoundSO bossFightRoundSO, StageSpawnPointsHandler stageSpawnPointsHandler)
    {
        float roundElapsedTimer = 0f;

        SpawnBoss(bossFightRoundSO.enemyBoss, stageSpawnPointsHandler.SpecialSpawnPoint.position);

        while (currentBossTransform != null)
        {
            roundElapsedTimer += Time.deltaTime;
            SetCurrentRoundElapsedTime(roundElapsedTimer);

            yield return null;
        }

        CompleteCurrentRound();
    }

    protected virtual void CompleteCurrentRound()
    {
        if (currentBossFightRound == null) return;

        OnRoundCompletedMethod(currentBossFightRound);

        ClearCurrentBossTransform();
        ClearCurrentRound();
        ResetCurrentRoundElapsedTime();

        EnemiesManager.Instance.ExecuteAllActiveEnemies(); //There should be no active enemies anyway, unless boss spawns enemies
    }

    private void SpawnBoss(EnemySO bossEnemySO, Vector3 position)
    {
        Transform bossTransform = EnemiesManager.Instance.SpawnEnemyAtPosition(bossEnemySO, position);
        SetCurrentBossTransform(bossTransform);
    }

    #region Set & Get
    protected void SetCurrentBossFightRound(BossFightRoundSO bossFightRoundSO) => currentBossFightRound = bossFightRoundSO;
    protected void SetCurrentRoundElapsedTime(float elapsedTime) => currentRoundElapsedTime = elapsedTime;
    protected void SetCurrentBossTransform(Transform bossTransform) => currentBossTransform = bossTransform;

    protected void ClearCurrentRound() => currentBossFightRound = null;
    protected void ResetCurrentRoundElapsedTime() => currentRoundElapsedTime = 0;
    protected void ClearCurrentBossTransform() => currentBossTransform = null;
    #endregion

    #region Virtual Methods
    protected override void OnRoundStartMethod(RoundSO roundSO)
    {
        base.OnRoundStartMethod(roundSO);
        OnBossFightRoundStart?.Invoke(this, new OnBossFightRoundEventArgs { bossFightRoundSO = roundSO as BossFightRoundSO });
    }

    protected override void OnRoundCompletedMethod(RoundSO roundSO)
    {
        base.OnRoundCompletedMethod(roundSO);
        OnBossFightRoundCompleted?.Invoke(this, new OnBossFightRoundEventArgs { bossFightRoundSO = roundSO as BossFightRoundSO });
    }
    #endregion

    #region Subscriptions

    private void EnemyCleanupHandler_OnAnyEnemyCleanup(object sender, EnemyCleanupHandler.OnEnemyCleanUpEventArgs e)
    {
        if (e.enemyTransform != currentBossTransform) return;
        ClearCurrentBossTransform();
    }
    #endregion
}
