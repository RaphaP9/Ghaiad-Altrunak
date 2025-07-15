using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeFeedbackManager : FeedbackManager
{
    [Header("Specific Settings")]
    [SerializeField, ColorUsage(true, true)] private Color feedbackColor;

    private void OnEnable()
    {
        EntityHealth.OnAnyEntityDodge += EntityHealth_OnAnyEntityDodge;
    }

    private void OnDisable()
    {
        EntityHealth.OnAnyEntityDodge -= EntityHealth_OnAnyEntityDodge;
    }

    private void EntityHealth_OnAnyEntityDodge(object sender, EntityHealth.OnEntityDodgeEventArgs e)
    {
        Vector2 instantiationPosition = GetInstantiationPosition((sender as EntityHealth).transform.position);
        CreateFeedback(feedbackPrefab, instantiationPosition,feedbackColor);
    }
}
