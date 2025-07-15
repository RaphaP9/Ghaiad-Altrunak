using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityExplosion : MonoBehaviour
{
    [Header("Entity Explosion Components")]
    [SerializeField] protected EntityHealth entityHealth;
    [SerializeField] protected List<Transform> explosionPoints;

    [Header("Entity Explosion Settings")]
    [SerializeField] protected LayerMask explosionLayermask;
    [SerializeField] protected bool explodeOnDeath;

    protected bool hasExecutedExplosion = false;

    #region Events
    public event EventHandler<OnEntityExplosionEventArgs> OnEntityExplosion;
    public static event EventHandler<OnEntityExplosionEventArgs> OnAnyEntityExplosion;

    public event EventHandler<OnEntityExplosionCompletedEventArgs> OnEntityExplosionCompleted;
    public static event EventHandler<OnEntityExplosionCompletedEventArgs> OnAnyEntityExplosionCompleted;
    #endregion

    #region EventArgs Classes
    public class OnEntityExplosionEventArgs
    {
        public List<Transform> explosionPoints;
        public int explosionDamage;
        public float explosionRadius;
        public Vector2 position;
    }

    public class OnEntityExplosionCompletedEventArgs
    {

    }
    #endregion

    protected virtual void OnEnable()
    {
        entityHealth.OnEntityDeath += EntityHealth_OnEntityDeath;
    }

    protected virtual void OnDisable()
    {
        entityHealth.OnEntityDeath -= EntityHealth_OnEntityDeath;
    }

    protected virtual bool CanExplode()
    {
        if (!entityHealth.IsAlive()) return false;

        return true;
    }

    public abstract bool OnExplosionExecution();
    protected abstract void Explode();

    #region Virtual Event Methods
    protected virtual void OnEntityExplosionMethod(int explosionDamage, float explosionRadius, Vector2 position)
    {
        OnEntityExplosion?.Invoke(this, new OnEntityExplosionEventArgs { explosionPoints = explosionPoints, explosionDamage = explosionDamage, explosionRadius = explosionRadius, position = position });
        OnAnyEntityExplosion?.Invoke(this, new OnEntityExplosionEventArgs { explosionPoints = explosionPoints, explosionDamage = explosionDamage, explosionRadius = explosionRadius, position = position });
    }

    protected virtual void OnEntityExplosionCompletedMethod()
    {
        OnAnyEntityExplosionCompleted?.Invoke(this, new OnEntityExplosionCompletedEventArgs { });
        OnEntityExplosionCompleted?.Invoke(this, new OnEntityExplosionCompletedEventArgs { });
    }
    #endregion

    #region Subscriptions
    protected virtual void EntityHealth_OnEntityDeath(object sender, EventArgs e)
    {
        if (explodeOnDeath) Explode();
    }
    #endregion
}
