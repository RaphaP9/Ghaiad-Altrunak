using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NumericFeedbackManager : FeedbackManager
{
    protected void CreateNumericFeedback(Transform prefab, Vector2 position, int numericValue, Color textColor)
    {
        Transform feedbackTransform = Instantiate(prefab, GeneralUtilities.Vector2ToVector3(position), Quaternion.identity);

        NumericFeedbackUI numericFeedbackUI = feedbackTransform.GetComponentInChildren<NumericFeedbackUI>();

        if (numericFeedbackUI == null)
        {
            if (debug) Debug.Log("Instantiated feedback does not contain a NumericFeedbackUI component.");
            return;
        }

        numericFeedbackUI.SetNumericFeedback(numericValue, textColor);
    }
}
