using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListener : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerHealth.OnAnyPlayerHealthTakeDamage += PlayerHealth_OnAnyPlayerHealthTakeDamage;
        EntityExplosion.OnAnyEntityExplosion += EntityExplosion_OnAnyEntityExplosion;

        EnemyExplosionTreatEffectHandler.OnTreatExplosion += EnemyExplosionTreatEffectHandler_OnTreatExplosion;
    }



    private void OnDisable()
    {
        PlayerHealth.OnAnyPlayerHealthTakeDamage -= PlayerHealth_OnAnyPlayerHealthTakeDamage;
        EntityExplosion.OnAnyEntityExplosion -= EntityExplosion_OnAnyEntityExplosion;

        EnemyExplosionTreatEffectHandler.OnTreatExplosion -= EnemyExplosionTreatEffectHandler_OnTreatExplosion;
    }

    #region Subscriptions

    private void PlayerHealth_OnAnyPlayerHealthTakeDamage(object sender, EntityHealth.OnEntityHealthTakeDamageEventArgs e) => GameLogManager.Instance.Log("Player/TakeDamage");
    private void EntityExplosion_OnAnyEntityExplosion(object sender, EntityExplosion.OnEntityExplosionEventArgs e) => GameLogManager.Instance.Log("Entity/Explosion");

    private void EnemyExplosionTreatEffectHandler_OnTreatExplosion(object sender, EnemyExplosionTreatEffectHandler.OnTreatExplosionEventArgs e) => GameLogManager.Instance.Log("Treat/EnemyExplosion/Explosion");

    #endregion
}
