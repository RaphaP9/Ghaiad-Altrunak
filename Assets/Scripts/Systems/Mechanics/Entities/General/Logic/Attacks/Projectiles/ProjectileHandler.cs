using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    [Header("Identifiers")]
    [SerializeField] private int id;

    [Header("Runtime Filled")]
    [SerializeField] private Vector2 direction;
    [Space]
    [SerializeField] private bool isCrit;
    [SerializeField] private int damage;
    [Space]
    [SerializeField, Range(5f, 15f)] private float speed;
    [SerializeField, Range(5f, 10f)] private float lifespan;
    [Space]
    [SerializeField] private ProjectileDamageType damageType;
    [SerializeField] private float areaRadius;
    [Space]
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private LayerMask impactLayerMask;

    private IDamageSource damageSource;
    private Rigidbody2D _rigidbody2D;

    public static event EventHandler<OnProjectileEventArgs> OnAnyProjectileImpactTarget;
    public static event EventHandler<OnProjectileEventArgs> OnAnyProjectileImpactRegular;
    public static event EventHandler<OnProjectileEventArgs> OnAnyProjectileLifespanEnd;

    public event EventHandler<OnProjectileEventArgs> OnProjectileSet;

    public event EventHandler<OnProjectileEventArgs> OnProjectileImpactTarget;
    public event EventHandler<OnProjectileEventArgs> OnProjectileImpactRegular;
    public event EventHandler<OnProjectileEventArgs> OnProjectileLifespanEnd;

    private bool hasTriggeredImpact = false;

    public class OnProjectileEventArgs : EventArgs
    {
        public int id;
        public IDamageSource damageSource;
        public Vector2 direction;
        public int damage;
        public bool isCrit; 
        public float speed;
        public float lifespan;  
        public ProjectileDamageType damageType;
        public float areaRadius;
        public LayerMask targetLayerMask;
        public LayerMask impactLayerMask;
    }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(LifespanCoroutine());
    }

    private void Update()
    {
        HandleMovement();
    }

    public void SetProjectile(IDamageSource damageSource, Vector2 direction, int damage, bool isCrit, float speed, float lifespan, ProjectileDamageType damageType, float areaRadius, LayerMask targetLayerMask, LayerMask impactLayerMask)
    {
        this.damageSource = damageSource;
        this.direction = direction;
        this.damage = damage;
        this.isCrit = isCrit;
        this.speed = speed;
        this.lifespan = lifespan;   
        this.damageType = damageType;
        this.areaRadius = areaRadius;
        this.targetLayerMask = targetLayerMask;
        this.impactLayerMask = impactLayerMask;

        OnProjectileSet?.Invoke(this, new OnProjectileEventArgs { id = id, damageSource = damageSource, direction = direction, damage = damage, isCrit = isCrit, speed = speed, lifespan = lifespan, damageType = damageType, areaRadius = areaRadius, targetLayerMask = targetLayerMask, impactLayerMask = impactLayerMask });
    }

    private IEnumerator LifespanCoroutine()
    {
        yield return new WaitForSeconds(lifespan);

        EndLifespan();
    }

    private void HandleMovement()
    {
        _rigidbody2D.velocity = direction * speed;
    }

    private void ImpactProjectileTarget()
    {
        hasTriggeredImpact = true;
        OnAnyProjectileImpactTarget?.Invoke(this, new OnProjectileEventArgs { id = id , damageSource = damageSource, direction = direction, damage = damage, isCrit = isCrit, speed = speed, lifespan = lifespan, damageType = damageType, areaRadius = areaRadius, targetLayerMask = targetLayerMask, impactLayerMask = impactLayerMask});
        OnProjectileImpactTarget?.Invoke(this, new OnProjectileEventArgs { id = id , damageSource = damageSource, direction = direction, damage = damage, isCrit = isCrit, speed = speed, lifespan = lifespan, damageType = damageType, areaRadius = areaRadius, targetLayerMask = targetLayerMask, impactLayerMask = impactLayerMask});
        Destroy(gameObject);
    }

    private void ImpactProjectileRegular()
    {
        hasTriggeredImpact = true;
        OnAnyProjectileImpactRegular?.Invoke(this, new OnProjectileEventArgs { id = id, damageSource = damageSource, direction = direction, damage = damage, isCrit = isCrit, speed = speed, lifespan = lifespan, damageType = damageType, areaRadius = areaRadius, targetLayerMask = targetLayerMask, impactLayerMask = impactLayerMask });
        OnProjectileImpactRegular?.Invoke(this, new OnProjectileEventArgs { id = id, damageSource = damageSource, direction = direction, damage = damage, isCrit = isCrit, speed = speed, lifespan = lifespan, damageType = damageType, areaRadius = areaRadius, targetLayerMask = targetLayerMask, impactLayerMask = impactLayerMask });
        Destroy(gameObject);
    }

    private void EndLifespan()
    {
        OnAnyProjectileLifespanEnd?.Invoke(this, new OnProjectileEventArgs { id = id, damageSource = damageSource, direction = direction, damage = damage, isCrit = isCrit, speed = speed, lifespan = lifespan, damageType = damageType, areaRadius = areaRadius, targetLayerMask = targetLayerMask, impactLayerMask = impactLayerMask });
        OnProjectileLifespanEnd?.Invoke(this, new OnProjectileEventArgs { id = id, damageSource = damageSource, direction = direction, damage = damage, isCrit = isCrit, speed = speed, lifespan = lifespan, damageType = damageType, areaRadius = areaRadius, targetLayerMask = targetLayerMask, impactLayerMask = impactLayerMask });
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageData damageData = new DamageData(damage, isCrit, damageSource, true, true, true, true);

        if (GeneralUtilities.CheckGameObjectInLayerMask(collision.gameObject, targetLayerMask))
        {
            if (hasTriggeredImpact) return;
            HandleDamageImpact(collision.transform, damageData);
            return;
        }

        if (GeneralUtilities.CheckGameObjectInLayerMask(collision.gameObject, impactLayerMask))
        {
            if (hasTriggeredImpact) return;
            HandleRegularImpact(collision.transform, damageData);
            return;
        }
    }

    private void HandleDamageImpact(Transform impactTransform, DamageData originalDamageData) //Impacted Something on the targetLayerMask
    {
        bool impacted = false;

        switch (damageType)
        {
            case ProjectileDamageType.Singular:
            default:
                impacted = MechanicsUtilities.DealDamageToTransform(impactTransform, originalDamageData); //Deal damage only to impact transform
                    break;
            case ProjectileDamageType.Area:
                impacted = MechanicsUtilities.DealDamageToTransform(impactTransform, originalDamageData); //Deal damage to impact transform
                if(impacted) //If impacted, deal area damage to EACH OTHER transform and CAN'T BE DODGED
                {
                    DamageData areaDamageData = new DamageData (originalDamageData.damage, isCrit, originalDamageData.damageSource, false, true, true, true );
                    MechanicsUtilities.DealDamageInArea(GeneralUtilities.TransformPositionVector2(transform), areaRadius, areaDamageData, targetLayerMask, new List<Transform> {impactTransform}); 
                }
                break;         
        }

        if (impacted) ImpactProjectileTarget();
    }

    private void HandleRegularImpact(Transform impactTransform, DamageData originalDamageData)
    {
        switch (damageType)
        {
            case ProjectileDamageType.Singular:
            default:
                //Nothing
                break;
            case ProjectileDamageType.Area: // Deal area damage to EVERY transform and CAN'T BE DODGED
                DamageData areaDamageData = new DamageData(originalDamageData.damage, originalDamageData.isCrit, originalDamageData.damageSource, false, true, true, true );
                MechanicsUtilities.DealDamageInArea(GeneralUtilities.TransformPositionVector2(transform), areaRadius, areaDamageData, impactLayerMask);
                break;
        }

        ImpactProjectileRegular();
    }
}
