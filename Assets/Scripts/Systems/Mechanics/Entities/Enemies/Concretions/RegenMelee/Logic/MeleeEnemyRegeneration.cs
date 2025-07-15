using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyRegeneration : MonoBehaviour, IHealSource
{
    [Header("Components")]
    [SerializeField] private EnemyIdentifier enemyIdentifier;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private EnemySpawnHandler enemySpawnHandler;
    [SerializeField] private MeleeEnemyAttack meleeEnemyAttack;

    [Header("Settings")]
    [SerializeField, Range(1, 5)] private int regeneration;
    [SerializeField, Range(0.5f, 5f)] private float regenInterval;
    [Space]
    [SerializeField, Range(0f, 5f)] private float timeToRegenerateAfterSpawning;
    [SerializeField, Range(0f, 5f)] private float timeToRegenerateAfterTakingDamage;
    [SerializeField, Range(0f, 5f)] private float timeToRegenerateAfterAttacking;

    [Header("Runtime Filled")]
    [SerializeField] private bool isRegenerating;

    public static event EventHandler OnAnyEnemyRegenerationStart;
    public event EventHandler OnEnemyRegenerationStart;

    public static event EventHandler OnAnyEnemyRegenerationInterrupted;
    public event EventHandler OnEnemyRegenerationInterrupted;

    public static event EventHandler OnAnyEnemyRegenerationInterruptedFullHealth;
    public event EventHandler OnEnemyRegenerationInterruptedFullHealth;

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

    #region Interface Methods
    public string GetHealSourceName() => enemyIdentifier.EnemySO.description;
    public Sprite GetHealSourceSprite() => enemyIdentifier.EnemySO.sprite;
    public string GetHealSourceDescription() => enemyIdentifier.EnemySO.description;

    #endregion

    private IEnumerator RegenerateEnemyAfterTimeCoroutine(float time)
    {
        if (isRegenerating) yield break;

        isRegenerating = true;

        yield return new WaitForSeconds(time);

        OnEnemyRegenerationStart?.Invoke(this, EventArgs.Empty);
        OnAnyEnemyRegenerationStart?.Invoke(this, EventArgs.Empty);

        while (true)
        {
            if (enemyHealth.IsFullHealth())
            {
                OnAnyEnemyRegenerationInterruptedFullHealth?.Invoke(this, EventArgs.Empty);
                OnEnemyRegenerationInterruptedFullHealth?.Invoke(this, EventArgs.Empty);
                break;
            }

            HealData healData = new HealData(regeneration, this);

            enemyHealth.Heal(healData);
            yield return new WaitForSeconds(regenInterval);
        }
    }

    private void InterruptEnemyRegeneration()
    {
        if (!isRegenerating) return;

        isRegenerating = false;

        StopAllCoroutines();

        OnAnyEnemyRegenerationInterrupted?.Invoke(this, EventArgs.Empty);
        OnEnemyRegenerationInterrupted?.Invoke(this, EventArgs.Empty);
    }

    #region Susbcriptions

    private void EnemySpawnHandler_OnEnemySpawnComplete(object sender, EnemySpawnHandler.OnEnemySpawnEventArgs e)
    {
        StopAllCoroutines();
        StartCoroutine(RegenerateEnemyAfterTimeCoroutine(timeToRegenerateAfterSpawning));
    }

    private void MeleeEnemyAttack_OnMeleeEnemyCharge(object sender, MeleeEnemyAttack.OnMeleeEnemyAttackEventArgs e)
    {
        StopAllCoroutines();
        InterruptEnemyRegeneration();
    }

    private void MeleeEnemyAttack_OnMeleeEnemyStopAttacking(object sender, MeleeEnemyAttack.OnMeleeEnemyAttackEventArgs e)
    {
        StopAllCoroutines();
        StartCoroutine(RegenerateEnemyAfterTimeCoroutine(timeToRegenerateAfterAttacking));
    }

    private void EnemyHealth_OnEnemyHealthTakeDamage(object sender, EntityHealth.OnEntityHealthTakeDamageEventArgs e)
    {
        StopAllCoroutines();
        InterruptEnemyRegeneration();
        StartCoroutine(RegenerateEnemyAfterTimeCoroutine(timeToRegenerateAfterTakingDamage));
    }

    private void EnemyHealth_OnEnemyDeath(object sender, EntityHealth.OnEntityDeathEventArgs e)
    {
        StopAllCoroutines();
        InterruptEnemyRegeneration();
    }
    #endregion
}
