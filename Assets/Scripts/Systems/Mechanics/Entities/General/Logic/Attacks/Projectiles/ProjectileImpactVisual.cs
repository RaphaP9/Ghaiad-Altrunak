using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileImpactVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ProjectileHandler projectileHandler;
    [SerializeField] private Transform impactVFXPrefab;

    private void OnEnable()
    {
        projectileHandler.OnProjectileImpactRegular += ProjectileHandler_OnProjectileDestroyByImpact;
        projectileHandler.OnProjectileImpactTarget += ProjectileHandler_OnProjectileImpactTarget;
    }

    private void OnDisable()
    {
        projectileHandler.OnProjectileImpactRegular -= ProjectileHandler_OnProjectileDestroyByImpact;
        projectileHandler.OnProjectileImpactTarget -= ProjectileHandler_OnProjectileImpactTarget;
    }

    private void CreateVFX()
    {
        Transform VFXTransform = Instantiate(impactVFXPrefab, transform.position, Quaternion.identity);
    }

    private void ProjectileHandler_OnProjectileDestroyByImpact(object sender, System.EventArgs e)
    {
        CreateVFX();
    }

    private void ProjectileHandler_OnProjectileImpactTarget(object sender, ProjectileHandler.OnProjectileEventArgs e)
    {
        CreateVFX();
    }
}
