using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttack : EntityAttack
{
    [Header("Player Attack Components")]
    [SerializeField] protected CharacterIdentifier characterIdentifier;

    [Header("Player Attack Settings")]
    [SerializeField] protected AttackTriggerType attackTriggerType;

    public AttackTriggerType AttackTriggerType_ => attackTriggerType;
    public enum AttackTriggerType {Automatic, SemiAutomatic}

    #region Events
    public event EventHandler<OnPlayerAttackEventArgs> OnPlayerAttack;
    public static event EventHandler<OnPlayerAttackEventArgs> OnAnyPlayerAttack;

    public event EventHandler<OnPlayerAttackCompletedEvtnArgs> OnPlayerAttackCompleted;
    public static event EventHandler<OnPlayerAttackCompletedEvtnArgs> OnAnyPlayerAttackCompleted;
    #endregion

    #region EventArgs Classes
    public class OnPlayerAttackEventArgs : OnEntityAttackEventArgs
    {
        public CharacterSO characterSO;
    }

    public class OnPlayerAttackCompletedEvtnArgs : OnEntityAttackCompletedEventArgs
    {
        public CharacterSO characterSO;
    }
    #endregion

    protected virtual void Start()
    {
        ResetAttackTimer();
    }

    protected virtual void Update()
    {
        HandleAttackCooldown();
    }

    public virtual void HandleAttack() //Called By PlayerStateHandler
    {
        if (!GetAttackInput()) return;
        if (!CanAttack()) return;

        Attack();
        MaxTimer();
    }

    private void HandleAttackCooldown()
    {
        if (attackTimer < 0) return;

        attackTimer -= Time.deltaTime;
    }

    protected void MaxTimer()
    {
        if (!HasValidAttackSpeed()) return;

        attackTimer = 1f / GetRevisedAttackSpeed();
    }

    private bool AttackOnCooldown() => attackTimer > 0f;
    private void ResetAttackTimer() => attackTimer = 0f;

    #region Virtual Event Methods
    protected override void OnEntityAttackMethod(bool isCrit, int attackDamage)
    {
        base.OnEntityAttackMethod(isCrit, attackDamage);

        OnPlayerAttack?.Invoke(this, new OnPlayerAttackEventArgs { characterSO = characterIdentifier.CharacterSO, isCrit = isCrit, attackDamage = attackDamage, attackSpeed = entityAttackSpeedStatResolver.Value, attackCritChance = entityAttackCritChanceStatResolver.Value, attackCritDamageMultiplier = entityAttackCritDamageMultiplierStatResolver.Value });
        OnAnyPlayerAttack?.Invoke(this, new OnPlayerAttackEventArgs { characterSO = characterIdentifier.CharacterSO, attackDamage = attackDamage, attackSpeed = entityAttackSpeedStatResolver.Value, attackCritChance = entityAttackCritChanceStatResolver.Value, attackCritDamageMultiplier = entityAttackCritDamageMultiplierStatResolver.Value });
    }

    protected override void OnEntityAttackCompletedMethod()
    {
        base.OnEntityAttackCompletedMethod();

        OnPlayerAttackCompleted?.Invoke(this, new OnPlayerAttackCompletedEvtnArgs { characterSO = characterIdentifier.CharacterSO });
        OnAnyPlayerAttackCompleted?.Invoke(this, new OnPlayerAttackCompletedEvtnArgs { characterSO = characterIdentifier.CharacterSO });
    }
    #endregion

    #region AttackTriggerType-Input Assignation
    protected bool GetSemiAutomaticInputAttack() => AttackInput.Instance.GetAttackDown();
    protected bool GetAutomaticInputAttack() => AttackInput.Instance.GetAttackHold();

    protected bool GetAttackInput()
    {
        switch (attackTriggerType)
        {
            case AttackTriggerType.SemiAutomatic:
            default:
                return GetSemiAutomaticInputAttack();
            case AttackTriggerType.Automatic:
                return GetAutomaticInputAttack();
        }
    }
    #endregion

    protected override bool CanAttack()
    {
        if (!base.CanAttack()) return false;
        if (AttackOnCooldown()) return false;

        return true;
    }
}
