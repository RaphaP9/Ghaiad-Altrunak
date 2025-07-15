using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyGhosting : MonoBehaviour, IDodger
{
    [Header("Components")]
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private EnemySpawnHandler enemySpawnHandler;
    [SerializeField] private MeleeEnemyAttack meleeEnemyAttack;

    [Header("Settings")]
    [SerializeField, Range(0f, 5f)] private float timeToGhostAfterSpawning;
    [SerializeField, Range(0f, 5f)] private float timeToGhostAfterTakingDamage;
    [SerializeField, Range(0f, 5f)] private float timeToGhostAfterAttacking;

    [Header("Runtime Filled")]
    [SerializeField] private bool isGhosted;

    public static event EventHandler OnAnyEnemyGhosting;
    public event EventHandler OnEnemyGhosting;

    public static event EventHandler OnAnyEnemyGhostingInterrupted;
    public event EventHandler OnEnemyGhostingInterrupted;

    private void OnEnable()
    {
        enemySpawnHandler.OnEnemySpawnComplete += EnemySpawnHandler_OnEnemySpawnComplete;
        meleeEnemyAttack.OnMeleeEnemyCharge += MeleeEnemyAttack_OnMeleeEnemyCharge;
        meleeEnemyAttack.OnMeleeEnemyStopAttacking += MeleeEnemyAttack_OnMeleeEnemyStopAttacking;

        enemyHealth.OnEnemyHealthTakeDamage += EnemyHealth_OnEnemyHealthTakeDamage;
        enemyHealth.OnEnemyDeath += EnemyHealth_OnEnemyDeath;
    }

    private void OnDisable()
    {
        enemySpawnHandler.OnEnemySpawnComplete -= EnemySpawnHandler_OnEnemySpawnComplete;
        meleeEnemyAttack.OnMeleeEnemyCharge -= MeleeEnemyAttack_OnMeleeEnemyCharge;
        meleeEnemyAttack.OnMeleeEnemyStopAttacking -= MeleeEnemyAttack_OnMeleeEnemyStopAttacking;

        enemyHealth.OnEnemyHealthTakeDamage -= EnemyHealth_OnEnemyHealthTakeDamage;
        enemyHealth.OnEnemyDeath -= EnemyHealth_OnEnemyDeath;
    }

    #region InterfaceMethods
    public bool IsDodging() => isGhosted;
    #endregion

    private IEnumerator GhostEnemyAfterTimeCoroutine(float time)
    {
        if (isGhosted) yield break;

        yield return new WaitForSeconds(time);
        GhostEnemy();
    }

    private void GhostEnemy()
    {
        if (isGhosted) return;

        isGhosted = true;

        OnEnemyGhosting?.Invoke(this, EventArgs.Empty);
        OnAnyEnemyGhosting?.Invoke(this, EventArgs.Empty);
    }

    private void InterruptEnemyGhost()
    {
        if (!isGhosted) return;

        isGhosted= false;

        OnEnemyGhostingInterrupted?.Invoke(this, EventArgs.Empty);
        OnAnyEnemyGhostingInterrupted?.Invoke(this, EventArgs.Empty);
    }

    #region Susbcriptions

    private void EnemySpawnHandler_OnEnemySpawnComplete(object sender, EnemySpawnHandler.OnEnemySpawnEventArgs e)
    {
        StopAllCoroutines();
        StartCoroutine(GhostEnemyAfterTimeCoroutine(timeToGhostAfterSpawning));
    }
    
    private void MeleeEnemyAttack_OnMeleeEnemyCharge(object sender, MeleeEnemyAttack.OnMeleeEnemyAttackEventArgs e)
    {
        StopAllCoroutines();
        InterruptEnemyGhost();
    }

    private void MeleeEnemyAttack_OnMeleeEnemyStopAttacking(object sender, MeleeEnemyAttack.OnMeleeEnemyAttackEventArgs e)
    {
        StopAllCoroutines();
        StartCoroutine(GhostEnemyAfterTimeCoroutine(timeToGhostAfterAttacking));
    }

    private void EnemyHealth_OnEnemyHealthTakeDamage(object sender, EntityHealth.OnEntityHealthTakeDamageEventArgs e)
    {
        if (isGhosted) return; //If take damage while ghosted, do not interrupt ghosting

        StopAllCoroutines();
        InterruptEnemyGhost();
        StartCoroutine(GhostEnemyAfterTimeCoroutine(timeToGhostAfterTakingDamage));
    }

    private void EnemyHealth_OnEnemyDeath(object sender, EntityHealth.OnEntityDeathEventArgs e)
    {
        StopAllCoroutines();
        InterruptEnemyGhost();
    }
    #endregion
}