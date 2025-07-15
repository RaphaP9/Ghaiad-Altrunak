using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [Space]
    [SerializeField] private EntityFacingDirectionHandler facingDirectionHandler;
    [SerializeField] private EnemySpawnHandler enemySpawnHandler;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private EnemyMovement enemyMovement;

    protected const string SPEED_FLOAT = "Speed";
    protected const string FACE_X_FLOAT = "FaceX";
    protected const string FACE_Y_FLOAT = "FaceY";

    protected const string SPAWN_ANIMATION_NAME = "Spawn";
    protected const string MOVEMENT_BLEND_TREE_NAME = "MovementBlendTree";
    protected const string DEATH_ANIMATION_NAME = "Death";

    protected bool isDead = false;

    protected virtual void OnEnable()
    {
        enemySpawnHandler.OnEnemySpawnStart += EnemySpawnHandler_OnEnemySpawnStart;
        enemySpawnHandler.OnEnemySpawnComplete += EnemySpawnHandler_OnEnemySpawnComplete;

        enemyHealth.OnEnemyDeath += EnemyHealth_OnEnemyDeath;
    }

    protected virtual void OnDisable()
    {
        enemySpawnHandler.OnEnemySpawnStart -= EnemySpawnHandler_OnEnemySpawnStart;
        enemySpawnHandler.OnEnemySpawnComplete -= EnemySpawnHandler_OnEnemySpawnComplete;

        enemyHealth.OnEnemyDeath -= EnemyHealth_OnEnemyDeath;
    }

    protected virtual void Update()
    {
        HandleSpeedBlend();
        HandleFacingBlend();
    }

    private void HandleSpeedBlend()
    {
        animator.SetFloat(SPEED_FLOAT, enemyMovement.GetCurrentSpeed());
    }

    private void HandleFacingBlend()
    {
        animator.SetFloat(FACE_X_FLOAT, facingDirectionHandler.CurrentFacingDirection.x);
        animator.SetFloat(FACE_Y_FLOAT, facingDirectionHandler.CurrentFacingDirection.y);
    }

    protected void PlayAnimation(string animationName, bool playEvenAfterDeath = false)
    {
        if (isDead && !playEvenAfterDeath) return;

        animator.Play(animationName);
    }

    #region Subscriptions
    private void EnemySpawnHandler_OnEnemySpawnStart(object sender, EnemySpawnHandler.OnEnemySpawnEventArgs e)
    {
        PlayAnimation(SPAWN_ANIMATION_NAME);
    }

    private void EnemySpawnHandler_OnEnemySpawnComplete(object sender, EnemySpawnHandler.OnEnemySpawnEventArgs e)
    {
        PlayAnimation(MOVEMENT_BLEND_TREE_NAME);
    }

    protected virtual void EnemyHealth_OnEnemyDeath(object sender, System.EventArgs e)
    {
        PlayAnimation(DEATH_ANIMATION_NAME);
        isDead = true;
    }
    #endregion
}
