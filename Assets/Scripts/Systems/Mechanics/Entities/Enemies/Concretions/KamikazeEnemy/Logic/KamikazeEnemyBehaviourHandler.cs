using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemyBehaviourHandler : EnemyBehaviourHandler
{
    [Header("Melee Enemy Components")]
    [SerializeField] private KamikazeEnemyExplosion kamikazeEnemyExplosion;

    [Header("State - Runtime Filled")]
    [SerializeField] private KamikazeEnemyState kamikazeEnemyState;

    private enum KamikazeEnemyState { Spawning, FollowingPlayer, Exploding, Dead }
    private KamikazeEnemySO KamikazeEnemySO => enemyIdentifier.EnemySO as KamikazeEnemySO;

    private void OnEnable()
    {
        enemySpawnHandler.OnEnemySpawnStart += EnemySpawnHandler_OnEnemySpawnStart;
        enemySpawnHandler.OnEnemySpawnComplete += EnemySpawnHandler_OnEnemySpawnComplete;

        enemyHealth.OnEnemyDeath += EnemyHealth_OnEnemyDeath;
    }

    private void OnDisable()
    {
        enemySpawnHandler.OnEnemySpawnStart -= EnemySpawnHandler_OnEnemySpawnStart;
        enemySpawnHandler.OnEnemySpawnComplete -= EnemySpawnHandler_OnEnemySpawnComplete;

        enemyHealth.OnEnemyDeath -= EnemyHealth_OnEnemyDeath;
    }

    private void Start()
    {
        SetState(KamikazeEnemyState.Spawning);
    }

    private void Update()
    {
        HandleStates();
    }

    private void HandleStates()
    {
        switch (kamikazeEnemyState)
        {
            case KamikazeEnemyState.Spawning:
                SpawningLogic();
                break;
            case KamikazeEnemyState.FollowingPlayer:
                FollowingPlayerLogic();
                break;
            case KamikazeEnemyState.Exploding:
                ExplodingLogic();
                break;
            case KamikazeEnemyState.Dead:
            default:
                DeadLogic();
                break;
        }
    }

    private void SpawningLogic()
    {
        enemyMovement.StopOnCurrentPosition();
    }

    private void FollowingPlayerLogic()
    {
        enemyMovement.MoveTowardsPlayerDirection();

        enemyAimDirectionerHandler.HandleAim();
        enemyFacingDirectionHandler.HandleFacing();

        if (OnExplosionRange())
        {
            enemyMovement.StopOnCurrentPosition();
            kamikazeEnemyExplosion.TriggerExplosion();

            SetState(KamikazeEnemyState.Exploding);
        }
    }

    private void ExplodingLogic()
    {
        //enemyMovement.StopOnCurrentPosition();
    }

    private void DeadLogic()
    {
        enemyMovement.StopOnCurrentPosition();
    }

    private void SetState(KamikazeEnemyState state) => kamikazeEnemyState = state;

    #region Ranges
    private bool OnExplosionRange() => playerRelativeHandler.DistanceToPlayer <= KamikazeEnemySO.detectionRange;
    #endregion

    #region Susbcriptions

    private void EnemySpawnHandler_OnEnemySpawnStart(object sender, EnemySpawnHandler.OnEnemySpawnEventArgs e)
    {
        SetState(KamikazeEnemyState.Spawning);
    }

    private void EnemySpawnHandler_OnEnemySpawnComplete(object sender, EnemySpawnHandler.OnEnemySpawnEventArgs e)
    {
        SetState(KamikazeEnemyState.FollowingPlayer);
    }

    private void EnemyHealth_OnEnemyDeath(object sender, EventArgs e)
    {
        kamikazeEnemyExplosion.TriggerExplosionStop();
        enemyMovement.StopOnCurrentPosition();

        SetState(KamikazeEnemyState.Dead);
    }
    #endregion
}
