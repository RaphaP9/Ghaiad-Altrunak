using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityExplosionVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EntityExplosion entityExplosion;
    [SerializeField] private Transform explosionVFXPrefab;

    private void OnEnable()
    {
        entityExplosion.OnEntityExplosion += EntityExplosion_OnEntityExplosion;
    }

    private void OnDisable()
    {
        entityExplosion.OnEntityExplosion -= EntityExplosion_OnEntityExplosion;
    }

    private void CreateVFX()
    {
        Transform VFXTransform = Instantiate(explosionVFXPrefab, transform.position, Quaternion.identity);
    }

    private void EntityExplosion_OnEntityExplosion(object sender, EntityExplosion.OnEntityExplosionEventArgs e)
    {
        CreateVFX();
    }
}
