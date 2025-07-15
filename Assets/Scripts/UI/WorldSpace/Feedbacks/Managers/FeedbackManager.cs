using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected Transform feedbackPrefab;

    [Header("Settings")]
    [SerializeField, Range(-2f, 2f)] protected float YOffset;
    [SerializeField, Range(0f, 3f)] protected float maxXOffset;

    [Header("Debug")]
    [SerializeField] protected bool debug;

    protected void CreateFeedback(Transform prefab, Vector2 position, Color textColor)
    {
        Transform feedbackTransform = Instantiate(prefab, GeneralUtilities.Vector2ToVector3(position), Quaternion.identity);

        FeedbackUI feedbackUI = feedbackTransform.GetComponentInChildren<FeedbackUI>();

        if (feedbackUI == null)
        {
            if (debug) Debug.Log("Instantiated feedback does not contain a FeedbackUI component.");
            return;
        }

        feedbackUI.SetFeedback(textColor);
    }

    protected Vector2 GetInstantiationPosition(Vector2 basePosition)
    {
        Vector2 offsetVector = new Vector2(GetRandomXOffset(), YOffset);
        Vector2 instantiationPosition = basePosition + offsetVector;
        return instantiationPosition;
    }

    private float GetRandomXOffset()
    {
        float xOffset = Random.Range(-maxXOffset, maxXOffset);
        return xOffset;
    }
}
