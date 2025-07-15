using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityAttack : MonoBehaviour
{
    [Header("Entity Attack Components")]
    [SerializeField] protected EntityHealth entityHealth;
    [Space]
    [SerializeField] protected EntityAttackDamageStatResolver entityAttackDamageStatResolver;
    [SerializeField] protected EntityAttackSpeedStatResolver entityAttackSpeedStatResolver;
    [SerializeField] protected EntityAttackCritChanceStatResolver entityAttackCritChanceStatResolver;
    [SerializeField] protected EntityAttackCritDamageMultiplierStatResolver entityAttackCritDamageMultiplierStatResolver;
    [Space]
    [SerializeField] protected LayerMask attackLayermask;
    [Space]
    [SerializeField] protected List<Component> attackInterruptionComponents;
    [SerializeField] protected List<Component> critOverriderComponents;

    [Header("Debug")]
    [SerializeField] protected bool debug;

    protected const float REVISED_PROOF_ATTACK_SPEED = 0.01f;

    protected float attackTimer = 0f;
    private List<IAttackInterruption> attackInterruptions;
    private List<ICritOverrider> critOverriders;

    #region Events
    public event EventHandler<OnEntityAttackEventArgs> OnEntityAttack;
    public static event EventHandler<OnEntityAttackEventArgs> OnAnyEntityAttack;

    public event EventHandler<OnEntityAttackCompletedEventArgs> OnEntityAttackCompleted;
    public static event EventHandler<OnEntityAttackCompletedEventArgs> OnAnyEntityAttackCompleted;
    #endregion

    #region EventArgs Classes
    public class OnEntityAttackEventArgs : EventArgs
    {
        public bool isCrit;

        public int attackDamage;
        public float attackSpeed;
        public float attackCritChance;
        public float attackCritDamageMultiplier;
    }

    public class OnEntityAttackCompletedEventArgs : EventArgs
    {

    }
    #endregion

    protected virtual void Awake()
    {
        attackInterruptions = GeneralUtilities.TryGetGenericsFromComponents<IAttackInterruption>(attackInterruptionComponents);
        critOverriders = GeneralUtilities.TryGetGenericsFromComponents<ICritOverrider>(critOverriderComponents);
    }

    protected abstract void Attack();

    protected float GetAttackSpeed() => entityAttackSpeedStatResolver.Value;

    protected bool HasValidAttackSpeed() => entityAttackSpeedStatResolver.Value > 0f;

    protected float GetRevisedAttackSpeed()
    {
        if(!HasValidAttackSpeed()) return REVISED_PROOF_ATTACK_SPEED;
        return GetAttackSpeed();
    }

    protected virtual bool CanAttack()
    {
        if (!entityHealth.IsAlive()) return false;
        if (!HasValidAttackSpeed()) return false;

        foreach (IAttackInterruption attackInterruption in attackInterruptions)
        {
            if (attackInterruption.IsInterruptingAttack()) return false;
        }

        return true;
    }

    protected virtual bool IsOverridingCrit()
    {
        foreach (ICritOverrider critOverrider in critOverriders)
        {
            if (critOverrider.IsOverridingCrit()) return true;
        }

        return false;
    }

    #region Virtual Event Methods
    protected virtual void OnEntityAttackMethod(bool isCrit, int attackDamage)
    {
        OnEntityAttack?.Invoke(this, new OnEntityAttackEventArgs {isCrit = isCrit, attackDamage = attackDamage, attackSpeed = entityAttackSpeedStatResolver.Value, attackCritChance = entityAttackCritChanceStatResolver.Value, attackCritDamageMultiplier = entityAttackCritDamageMultiplierStatResolver.Value });
        OnAnyEntityAttack?.Invoke(this, new OnEntityAttackEventArgs {isCrit = isCrit, attackDamage = attackDamage, attackSpeed = entityAttackSpeedStatResolver.Value, attackCritChance = entityAttackCritChanceStatResolver.Value, attackCritDamageMultiplier = entityAttackCritDamageMultiplierStatResolver.Value });
    }

    protected virtual void OnEntityAttackCompletedMethod()
    {
        OnEntityAttackCompleted?.Invoke(this, new OnEntityAttackCompletedEventArgs {});
        OnAnyEntityAttackCompleted?.Invoke(this, new OnEntityAttackCompletedEventArgs {});
    }
    #endregion
}
