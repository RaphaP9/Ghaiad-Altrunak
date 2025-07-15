using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritDamageFeedbackManager : NumericFeedbackManager
{
    [Header("Specific Settings")]
    [SerializeField, ColorUsage(true, true)] private Color feedbackColor;

    private void OnEnable()
    {
        EntityHealth.OnAnyEntityHealthTakeDamage += EntityHealth_OnAnyEntityHealthTakeDamage;
    }

    private void OnDisable()
    {
        EntityHealth.OnAnyEntityHealthTakeDamage -= EntityHealth_OnAnyEntityHealthTakeDamage;
    }

    private void EntityHealth_OnAnyEntityHealthTakeDamage(object sender, EntityHealth.OnEntityHealthTakeDamageEventArgs e)
    {
        if (!e.isCrit) return;

        Vector2 instantiationPosition = GetInstantiationPosition((sender as EntityHealth).transform.position);
        CreateNumericFeedback(feedbackPrefab, instantiationPosition, e.rawDamage, e.damageSource.GetDamageSourceColor());
    }
}
