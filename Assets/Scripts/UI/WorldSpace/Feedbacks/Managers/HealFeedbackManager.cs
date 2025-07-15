using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealFeedbackManager : NumericFeedbackManager
{
    [Header("Specific Settings")]
    [SerializeField, ColorUsage(true, true)] private Color feedbackColor;

    private void OnEnable()
    {
        EntityHealth.OnAnyEntityHeal += EntityHealth_OnAnyEntityHeal;
    }

    private void OnDisable()
    {
        EntityHealth.OnAnyEntityHeal -= EntityHealth_OnAnyEntityHeal;
    }

    private void EntityHealth_OnAnyEntityHeal(object sender, EntityHealth.OnEntityHealEventArgs e)
    {
        Vector2 instantiationPosition = GetInstantiationPosition((sender as EntityHealth).transform.position);
        CreateNumericFeedback(feedbackPrefab, instantiationPosition, e.healDone, feedbackColor);
    }
}
