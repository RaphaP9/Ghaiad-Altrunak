using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeEnemyBehaviourHandler : EnemyBehaviourHandler
{
    [Header("Melee Enemy Components")]
    [SerializeField] private MeleeEnemyAttack meleeEnemyAttack;

    [Header("State - Runtime Filled")]
    [SerializeField] private MeleeEnemyState meleeEnemyState;

    private enum MeleeEnemyState {Spawning, FollowingPlayer, Attacking, Dead}
    private MeleeEnemySO MeleeEnemySO => enemyIdentifier.EnemySO as MeleeEnemySO;

    private void OnEnable()
    {
        enemySpawnHandler.OnEnemySpawnStart += EnemySpawnHandler_OnEnemySpawnStart;
        enemySpawnHandler.OnEnemySpawnComplete += EnemySpawnHandler_OnEnemySpawnComplete;

        meleeEnemyAttack.OnEnemyAttackCompleted += MeleeEnemyAttack_OnEnemyAttackCompleted;

        enemyHealth.OnEnemyDeath += EnemyHealth_OnEnemyDeath;
    }

    private void OnDisable()
    {
        enemySpawnHandler.OnEnemySpawnStart -= EnemySpawnHandler_OnEnemySpawnStart;
        enemySpawnHandler.OnEnemySpawnComplete -= EnemySpawnHandler_OnEnemySpawnComplete;

        meleeEnemyAttack.OnEnemyAttackCompleted -= MeleeEnemyAttack_OnEnemyAttackCompleted;

        enemyHealth.OnEnemyDeath -= EnemyHealth_OnEnemyDeath;
    }

    private void Start()
    {
        SetState(MeleeEnemyState.Spawning);
    }

    private void Update()
    {
        HandleStates();
    }

    private void HandleStates()
    {
        switch (meleeEnemyState)
        {
            case MeleeEnemyState.Spawning:
                SpawningLogic();
                break;
            case MeleeEnemyState.FollowingPlayer:
                FollowingPlayerLogic();
                break;
            case MeleeEnemyState.Attacking:
                AttackingPlayerLogic();
                break;
            case MeleeEnemyState.Dead:
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

        if (OnAttackRange())
        {
            enemyMovement.StopOnCurrentPosition();
            meleeEnemyAttack.TriggerAttack();

            SetState(MeleeEnemyState.Attacking);
        }
    }

    private void AttackingPlayerLogic()
    {
        //enemyMovement.StopOnCurrentPosition();
    }

    private void DeadLogic()
    {
        enemyMovement.StopOnCurrentPosition();
    }

    private void SetState(MeleeEnemyState state) => meleeEnemyState = state;

    #region Ranges
    private bool OnAttackRange() => playerRelativeHandler.DistanceToPlayer <= MeleeEnemySO.attackDistance;
    #endregion

    #region Susbcriptions

    private void EnemySpawnHandler_OnEnemySpawnStart(object sender, EnemySpawnHandler.OnEnemySpawnEventArgs e)
    {
        SetState(MeleeEnemyState.Spawning);
    }

    private void EnemySpawnHandler_OnEnemySpawnComplete(object sender, EnemySpawnHandler.OnEnemySpawnEventArgs e)
    {
        SetState(MeleeEnemyState.FollowingPlayer);
    }

    private void MeleeEnemyAttack_OnEnemyAttackCompleted(object sender, EnemyAttack.OnEntityAttackCompletedEventArgs e)
    {
        enemyAimDirectionerHandler.HandleAim();
        enemyFacingDirectionHandler.HandleFacing();

        if (OnAttackRange())
        {
            meleeEnemyAttack.TriggerAttack();
            SetState(MeleeEnemyState.Attacking);
        }
        else
        {
            SetState(MeleeEnemyState.FollowingPlayer);
        }
    }

    private void EnemyHealth_OnEnemyDeath(object sender, EventArgs e)
    {
        meleeEnemyAttack.TriggerAttackStop();
        enemyMovement.StopOnCurrentPosition();

        SetState(MeleeEnemyState.Dead);
    }
    #endregion
}
