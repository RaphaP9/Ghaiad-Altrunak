using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityColliderHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected EntityHealth entityHealth;
    [SerializeField] protected Collider2D _collider2D;

    protected virtual void OnEnable()
    {
        entityHealth.OnEntityDeath += EntityHealth_OnEntityDeath;
    }

    protected virtual void OnDisable()
    {
        entityHealth.OnEntityDeath -= EntityHealth_OnEntityDeath;
    }

    protected void EnableCollider() => _collider2D.enabled = true;
    protected void DisableCollider() => _collider2D.enabled = false;

    private void EntityHealth_OnEntityDeath(object sender, System.EventArgs e)
    {
        DisableCollider();
    }
}
