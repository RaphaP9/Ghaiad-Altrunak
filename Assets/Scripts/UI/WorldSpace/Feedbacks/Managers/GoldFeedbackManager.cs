using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldFeedbackManager : NumericFeedbackManager
{
    [Header("Specific Settings")]
    [SerializeField, ColorUsage(true, true)] private Color feedbackColor;

    private void OnEnable()
    {
        GoldManager.OnProcessedGoldCollected += GoldManager_OnProcessedGoldCollected;
    }

    private void OnDisable()
    {
        GoldManager.OnProcessedGoldCollected -= GoldManager_OnProcessedGoldCollected;
    }

    private void GoldManager_OnProcessedGoldCollected(object sender, GoldManager.OnTangibleGoldEventArgs e)
    {
        Vector2 instantiationPosition = GetInstantiationPosition(e.position);
        CreateNumericFeedback(feedbackPrefab, instantiationPosition, e.goldAmount, feedbackColor);
    }
}
