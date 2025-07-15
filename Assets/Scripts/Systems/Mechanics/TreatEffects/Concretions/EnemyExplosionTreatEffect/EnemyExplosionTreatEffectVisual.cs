using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosionTreatEffectVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyExplosionTreatEffectHandler enemyExplosionTreatEffectHandler;
    [SerializeField] private Transform explosionVFXPrefab;

    private void OnEnable()
    {
        enemyExplosionTreatEffectHandler.OnThisTreatExplosion += EntityExplosion_OnEntityExplosion;
    }

    private void OnDisable()
    {
        enemyExplosionTreatEffectHandler.OnThisTreatExplosion -= EntityExplosion_OnEntityExplosion;
    }

    private void CreateVFX(Vector2 position)
    {
        Transform VFXTransform = Instantiate(explosionVFXPrefab, GeneralUtilities.Vector2ToVector3(position), Quaternion.identity);
    }

    private void EntityExplosion_OnEntityExplosion(object sender, EnemyExplosionTreatEffectHandler.OnTreatExplosionEventArgs e)
    {
        CreateVFX(e.position);
    }
}
