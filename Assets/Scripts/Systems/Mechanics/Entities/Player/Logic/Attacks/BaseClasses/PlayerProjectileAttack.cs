using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileAttack : PlayerAttack
{
    [Header("Player Projectile Attack Components")]
    [SerializeField] protected Transform mainFirePoint;
    [SerializeField] protected Transform mainProjectilePrefab;

    [Header("Interface Components")]
    [SerializeField] private Component directionerHandlerComponent;

    [Header("Projectile Settings")]
    [SerializeField] protected ProjectileDamageType projectileDamageType;
    [SerializeField, Range(0f,3f)] protected float projectileAreaRadius;
    [Space]
    [SerializeField] protected LayerMask projectileImpactLayerMask;
    [Space]
    [SerializeField, Range(0.1f,50f)] protected float projectileSpeed;
    [SerializeField, Range(0.5f,15f)] protected float projectileLifespan;
    [Space]
    [SerializeField, Range(0f, 15f)] protected float projectileDispersionAngle;

    protected IDirectionHandler directionerHandler;

    protected override void Awake()
    {
        base.Awake();
        GeneralUtilities.TryGetGenericFromComponent(directionerHandlerComponent, out directionerHandler);
    }

    protected override void Attack()
    {
        ShootProjectile(mainProjectilePrefab, mainFirePoint, directionerHandler.GetDirection(), characterIdentifier.CharacterSO.attackDamagePercentage);
    }

    protected void ShootProjectile(Transform projectilePrefab, Transform firePoint, Vector2 shootDirection, float extraProjectileDamagePercentage = 1f)
    {
        bool isCrit = MechanicsUtilities.EvaluateCritAttack(entityAttackCritChanceStatResolver.Value, IsOverridingCrit());
        int regularDamage = Mathf.CeilToInt(entityAttackDamageStatResolver.Value * characterIdentifier.CharacterSO.attackDamagePercentage * extraProjectileDamagePercentage);

        int damage = isCrit ? MechanicsUtilities.CalculateCritDamage(regularDamage, entityAttackCritDamageMultiplierStatResolver.Value) : regularDamage;

        Vector2 processedShootDirection = MechanicsUtilities.DeviateShootDirection(shootDirection, projectileDispersionAngle);

        Vector2 position = firePoint.position;

        InstantiateProjectile(characterIdentifier.CharacterSO, projectilePrefab, position, processedShootDirection, damage, isCrit, projectileSpeed, projectileLifespan, projectileDamageType, projectileAreaRadius, attackLayermask, projectileImpactLayerMask);

        OnEntityAttackMethod(isCrit, damage);
        OnEntityAttackCompletedMethod();
    }

    protected void InstantiateProjectile(IDamageSource damageSource, Transform projectilePrefab, Vector2 position, Vector2 shootDirection, int damage, bool isCrit, float speed, float lifespan, ProjectileDamageType projectileDamageType , float areaRadius, LayerMask targetLayerMask, LayerMask impactLayerMask)
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
