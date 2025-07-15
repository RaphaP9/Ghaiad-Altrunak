using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileEnemyAttack : EnemyAttack
{
    [Header("Projectile Enemy Attack Components")]
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected Transform projectilePrefab;

    [Header("Interface Components")]
    [SerializeField] private Component directionerHandlerComponent;

    [Header("Projectile Settings")]
    [SerializeField] protected ProjectileDamageType projectileDamageType;
    [SerializeField, Range(0f, 3f)] protected float projectileAreaRadius;
    [Space]
    [SerializeField] protected LayerMask projectileImpactLayerMask;
    [Space]
    [SerializeField, Range(0.1f, 50f)] protected float projectileSpeed;
    [SerializeField, Range(0.5f, 15f)] protected float projectileLifespan;
    [Space]
    [SerializeField, Range(0f, 15f)] protected float projectileDispersionAngle;

    [Header("States - Runtime Filled")]
    [SerializeField] protected ProjectileAttackState projectileAttackState;

    protected enum ProjectileAttackState { NotAttacking, Aiming, Shooting, Reloading }
    private ProjectileEnemySO ProjectileEnemySO => EnemySO as ProjectileEnemySO;

    public static event EventHandler<OnProjectileEnemyAttackEventArgs> OnAnyProjectileEnemyAim;
    public static event EventHandler<OnProjectileEnemyAttackEventArgs> OnAnyProjectileEnemyShoot;
    public static event EventHandler<OnProjectileEnemyAttackEventArgs> OnAnyProjectileEnemyReload;
    public static event EventHandler<OnProjectileEnemyAttackEventArgs> OnAnyProjectileEnemyStopShooting;

    public event EventHandler<OnProjectileEnemyAttackEventArgs> OnProjectileEnemyAim;
    public event EventHandler<OnProjectileEnemyAttackEventArgs> OnProjectileEnemyShoot;
    public event EventHandler<OnProjectileEnemyAttackEventArgs> OnProjectileEnemyReload;
    public event EventHandler<OnProjectileEnemyAttackEventArgs> OnProjectileEnemyStopShooting;

    protected IDirectionHandler directionerHandler;

    public class OnProjectileEnemyAttackEventArgs : EventArgs
    {
        public ProjectileEnemySO projectileEnemySO;
        public Transform firePoint;
    }

    protected override void Awake()
    {
        base.Awake();
        GeneralUtilities.TryGetGenericFromComponent(directionerHandlerComponent, out directionerHandler);
    }

    private void Start()
    {
        ResetTimer();
        SetProjectileAttackState(ProjectileAttackState.NotAttacking);
    }

    private void Update()
    {
        HandleProjectileAttack();
    }

    #region Logic
    private void HandleProjectileAttack()
    {
        switch (projectileAttackState)
        {
            case ProjectileAttackState.NotAttacking:
            default:
                NotAttackingLogic();
                break;
            case ProjectileAttackState.Aiming:
                AimingLogic();
                break;
            case ProjectileAttackState.Shooting:
                ShootingLogic();
                break;
            case ProjectileAttackState.Reloading:
                ReloadingLogic();
                break;
        }
    }

    private void NotAttackingLogic()
    {
        hasExecutedAttack = false;

        if (!CanAttack())
        {
            ResetTimer();
            return;
        }

        if (shouldAttack)
        {
            shouldAttack = false;
            TransitionToState(ProjectileAttackState.Aiming);
        }
    }

    private void AimingLogic()
    {
        if (shouldStopAttack)
        {
            shouldStopAttack = false;
            TransitionToState(ProjectileAttackState.NotAttacking);
            return;
        }

        if (timer < GetAimingTime())
        {
            timer += Time.deltaTime;
            return;
        }

        TransitionToState(ProjectileAttackState.Shooting);
    }

    private void ShootingLogic()
    {
        if (shouldStopAttack)
        {
            shouldStopAttack = false;
            TransitionToState(ProjectileAttackState.NotAttacking);
            return;
        }

        if (timer >= GetAttackExecutionTime() && !hasExecutedAttack) //Control when to trigger the attack relative to the AttackState
        {
            Attack();
            hasExecutedAttack = true;
        }

        if (timer < GetShootingTime())
        {
            timer += Time.deltaTime;
            return;
        }

        TransitionToState(ProjectileAttackState.Reloading);
    }

    private void ReloadingLogic()
    {
        if (shouldStopAttack)
        {
            shouldStopAttack = false;
            TransitionToState(ProjectileAttackState.NotAttacking);
            return;
        }

        if (timer < GetReloadTime())
        {
            timer += Time.deltaTime;
            return;
        }

        hasExecutedAttack = false;

        OnEntityAttackCompletedMethod();

        if (shouldAttack)
        {
            shouldAttack = false;
            TransitionToState(ProjectileAttackState.Aiming);
        }

        else TransitionToState(ProjectileAttackState.NotAttacking);
    }
    #endregion

    private void SetProjectileAttackState(ProjectileAttackState state) => projectileAttackState = state;

    private void TransitionToState(ProjectileAttackState state)
    {
        switch (state)
        {
            case ProjectileAttackState.NotAttacking:
                SetProjectileAttackState(ProjectileAttackState.NotAttacking);
                OnAnyProjectileEnemyStopShooting?.Invoke(this, new OnProjectileEnemyAttackEventArgs { projectileEnemySO = ProjectileEnemySO, firePoint = firePoint });
                OnProjectileEnemyStopShooting?.Invoke(this, new OnProjectileEnemyAttackEventArgs { projectileEnemySO = ProjectileEnemySO, firePoint = firePoint });
                break;
            case ProjectileAttackState.Aiming:
                SetProjectileAttackState(ProjectileAttackState.Aiming);
                OnAnyProjectileEnemyAim?.Invoke(this, new OnProjectileEnemyAttackEventArgs { projectileEnemySO = ProjectileEnemySO, firePoint = firePoint });
                OnProjectileEnemyAim?.Invoke(this, new OnProjectileEnemyAttackEventArgs { projectileEnemySO = ProjectileEnemySO, firePoint = firePoint });
                break;
            case ProjectileAttackState.Shooting:
                SetProjectileAttackState(ProjectileAttackState.Shooting);
                OnAnyProjectileEnemyShoot?.Invoke(this, new OnProjectileEnemyAttackEventArgs { projectileEnemySO = ProjectileEnemySO, firePoint = firePoint });
                OnProjectileEnemyShoot?.Invoke(this, new OnProjectileEnemyAttackEventArgs { projectileEnemySO = ProjectileEnemySO, firePoint = firePoint });
                break;
            case ProjectileAttackState.Reloading:
                SetProjectileAttackState(ProjectileAttackState.Reloading);
                OnAnyProjectileEnemyReload?.Invoke(this, new OnProjectileEnemyAttackEventArgs { projectileEnemySO = ProjectileEnemySO, firePoint = firePoint });
                OnProjectileEnemyReload?.Invoke(this, new OnProjectileEnemyAttackEventArgs { projectileEnemySO = ProjectileEnemySO, firePoint = firePoint });
                break;
        }

        ResetTimer();
    }

    private float GetAimingTime() => 1 / GetRevisedAttackSpeed() * ProjectileEnemySO.GetNormalizedAimingMult();
    private float GetShootingTime() => 1 / GetRevisedAttackSpeed() * ProjectileEnemySO.GetNormalizedShootingMult();
    private float GetReloadTime() => 1 / GetRevisedAttackSpeed() * ProjectileEnemySO.GetNormalizedReloadMult();
    private float GetAttackExecutionTime() => GetRevisedAttackSpeed() * Mathf.Clamp01(ProjectileEnemySO.attackExecutionTimeMult);

    public override bool OnAttackExecution() => projectileAttackState != ProjectileAttackState.NotAttacking;

    protected override void Attack()
    {
        bool isCrit = MechanicsUtilities.EvaluateCritAttack(entityAttackCritChanceStatResolver.Value);
        int damage = isCrit ? MechanicsUtilities.CalculateCritDamage(entityAttackDamageStatResolver.Value, entityAttackCritDamageMultiplierStatResolver.Value) : entityAttackDamageStatResolver.Value;

        Vector2 shootDirection = directionerHandler.GetDirection();
        Vector2 processedShootDirection = MechanicsUtilities.DeviateShootDirection(shootDirection, projectileDispersionAngle);

        Vector2 position = firePoint.position;

        InstantiateProjectile(ProjectileEnemySO, projectilePrefab, position, processedShootDirection, damage, isCrit, projectileSpeed, projectileLifespan, projectileDamageType, projectileAreaRadius, attackLayermask, projectileImpactLayerMask);

        OnEntityAttackMethod(isCrit, damage);
    }

    protected void InstantiateProjectile(IDamageSource damageSource, Transform projectilePrefab, Vector2 position, Vector2 shootDirection, int damage, bool isCrit, float speed, float lifespan, ProjectileDamageType projectileDamageType, float areaRadius, LayerMask targetLayerMask, LayerMask impactLayerMask)
    {
        Vector3 vector3Position = GeneralUtilities.Vector2ToVector3(position);
        Transform instantiatedProjectile = Instantiate(projectilePrefab, vector3Position, Quaternion.identity);

        ProjectileHandler projectileHandler = instantiatedProjectile.GetComponent<ProjectileHandler>();

        if (projectileHandler == null)
        {
            if (debug) Debug.Log("Instantiated projectile does not contain a ProjectileHandler component. Projectile set will be ignored.");
            return;
        }

        projectileHandler.SetProjectile(damageSource, shootDirection, damage, isCrit, speed, lifespan, projectileDamageType, areaRadius, targetLayerMask, impactLayerMask);
    }
}
