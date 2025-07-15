using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyBehaviourHandler : EnemyBehaviourHandler
{
    [Header("Projectile Enemy Components")]
    [SerializeField] private ProjectileEnemyAttack projectileEnemyAttack;

    [Header("State - Runtime Filled")]
    [SerializeField] private ProjectileEnemyState projectileEnemyState;

    private enum ProjectileEnemyState { Spawning, FollowingPlayer, MovingAwayFromPlayer, Attacking, Dead }
    private ProjectileEnemySO ProjectileEnemySO => enemyIdentifier.EnemySO as ProjectileEnemySO;

    private const float PREFERRED_DISTANCE_THRESHOLD = 0.5f;

    private void OnEnable()
    {
        enemySpawnHandler.OnEnemySpawnStart += EnemySpawnHandler_OnEnemySpawnStart;
        enemySpawnHandler.OnEnemySpawnComplete += EnemySpawnHandler_OnEnemySpawnComplete;

        projectileEnemyAttack.OnEnemyAttackCompleted += ProjectileEnemyAttack_OnEnemyAttackCompleted;

        enemyHealth.OnEnemyDeath += EnemyHealth_OnEnemyDeath;
    }

    private void OnDisable()
    {
        enemySpawnHandler.OnEnemySpawnStart -= EnemySpawnHandler_OnEnemySpawnStart;
        enemySpawnHandler.OnEnemySpawnComplete -= EnemySpawnHandler_OnEnemySpawnComplete;

        projectileEnemyAttack.OnEnemyAttackCompleted -= ProjectileEnemyAttack_OnEnemyAttackCompleted;

        enemyHealth.OnEnemyDeath -= EnemyHealth_OnEnemyDeath;
    }

    private void Start()
    {
        SetState(ProjectileEnemyState.Spawning);
    }

    private void Update()
    {
        HandleStates();
    }

    private void HandleStates()
    {
        switch (projectileEnemyState)
        {
            case ProjectileEnemyState.Spawning:
                SpawningLogic();
                break;
            case ProjectileEnemyState.FollowingPlayer:
                FollowingPlayerLogic();
                break;
            case ProjectileEnemyState.MovingAwayFromPlayer:
                MovingAwayFromPlayerLogic();
                break;
            case ProjectileEnemyState.Attacking:
                AttackingPlayerLogic();
                break;
            case ProjectileEnemyState.Dead:
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

        if (OnTooCloseDistance())
        {
            SetState(ProjectileEnemyState.MovingAwayFromPlayer);
            return;
        }

        if (OnPreferredDistance())
        {
            enemyMovement.StopOnCurrentPosition();
            projectileEnemyAttack.TriggerAttack();

            SetState(ProjectileEnemyState.Attacking);
            return;
        }
    }

    private void MovingAwayFromPlayerLogic()
    {
        enemyMovement.MoveAwayFromPlayerDirection();

        enemyAimDirectionerHandler.HandleAim();
        enemyFacingDirectionHandler.HandleFacing();

        if (OnTooFarDistance())
        {
            SetState(ProjectileEnemyState.FollowingPlayer);
            return;
        }

        if (OnPreferredDistance())
        {
            enemyMovement.StopOnCurrentPosition();
            projectileEnemyAttack.TriggerAttack();

            SetState(ProjectileEnemyState.Attacking);
            return;
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

    private void SetState(ProjectileEnemyState state) => projectileEnemyState = state;

    #region Ranges
    public bool OnPreferredDistance() //EXACT Preferred Distance + Threshold
    {
        if (Math.Abs(playerRelativeHandler.DistanceToPlayer - ProjectileEnemySO.preferredDistance) > PREFERRED_DISTANCE_THRESHOLD) return false;
        return true;
    }

    public bool OnTooFarDistance() => playerRelativeHandler.DistanceToPlayer >= ProjectileEnemySO.tooFarDistance;
    public bool OnTooCloseDistance() => playerRelativeHandler.DistanceToPlayer <= ProjectileEnemySO.tooCloseDistance;
    public bool OnAttackRange() => !OnTooFarDistance() && !OnTooCloseDistance();

    #endregion

    #region Susbcriptions

    private void EnemySpawnHandler_OnEnemySpawnStart(object sender, EnemySpawnHandler.OnEnemySpawnEventArgs e)
    {
        SetState(ProjectileEnemyState.Spawning);
    }

    private void EnemySpawnHandler_OnEnemySpawnComplete(object sender, EnemySpawnHandler.OnEnemySpawnEventArgs e)
    {
        SetState(ProjectileEnemyState.FollowingPlayer);
    }

    private void ProjectileEnemyAttack_OnEnemyAttackCompleted(object sender, EnemyAttack.OnEntityAttackCompletedEventArgs e)
    {
        enemyAimDirectionerHandler.HandleAim();
        enemyFacingDirectionHandler.HandleFacing();

        if (OnAttackRange())
        {
            projectileEnemyAttack.TriggerAttack();
            SetState(ProjectileEnemyState.Attacking);
            return;
        }

        if (OnTooCloseDistance())
        {
            SetState(ProjectileEnemyState.MovingAwayFromPlayer);
            return;
        }

        if (OnTooFarDistance())
        {
            SetState(ProjectileEnemyState.FollowingPlayer);
            return;
        }
    }

    private void EnemyHealth_OnEnemyDeath(object sender, EventArgs e)
    {
        projectileEnemyAttack.TriggerAttackStop();
        enemyMovement.StopOnCurrentPosition();

        SetState(ProjectileEnemyState.Dead);
    }
    #endregion
}
