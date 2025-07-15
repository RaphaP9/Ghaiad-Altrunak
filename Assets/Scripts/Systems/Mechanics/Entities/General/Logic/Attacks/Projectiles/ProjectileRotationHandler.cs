using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRotationHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ProjectileHandler projectileHandler;
    [SerializeField] private Transform visualTransform;

    private void OnEnable()
    {
        projectileHandler.OnProjectileSet += ProjectileHandler_OnProjectileSet;
    }

    private void OnDisable()
    {
        projectileHandler.OnProjectileSet -= ProjectileHandler_OnProjectileSet;
    }

    private void RotateTowardsDirection(Vector2 direction)
    {
        GeneralUtilities.RotateTransformTowardsVector2(visualTransform, direction);
    }

    private void CheckScaling(Vector2 direction)
    {
        if(direction.x < 0)
        {
            visualTransform.localScale = new Vector3(-visualTransform.localScale.x, -visualTransform.localScale.y, visualTransform.localScale.z);
        }
    }

    private void ProjectileHandler_OnProjectileSet(object sender, ProjectileHandler.OnProjectileEventArgs e)
    {
        RotateTowardsDirection(e.direction);
        CheckScaling(e.direction);
    }
}
