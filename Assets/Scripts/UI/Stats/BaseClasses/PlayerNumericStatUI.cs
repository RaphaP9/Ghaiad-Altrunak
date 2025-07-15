using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class PlayerNumericStatUI<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] protected TextMeshProUGUI valueText;

    [Header("Settings")]
    [SerializeField] protected Color positiveColor;
    [SerializeField] protected Color neutralColor;
    [SerializeField] protected Color negativeColor;

    [Header("Debug")]
    [SerializeField] protected bool debug;

    protected T resolver;

    protected virtual void OnEnable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation += PlayerInstantiationHandler_OnPlayerInstantiation;
    }
    protected virtual void OnDisable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation -= PlayerInstantiationHandler_OnPlayerInstantiation;
        UnSubscribeToEvents();
    }

    protected abstract NumericStatType GetNumericStatType();

    #region Logic
    protected void UpdateUIByNewValue(float currentValue, float baseValue)
    {
        NumericStatState statState = MappingUtilities.GetNumericStatState(GetNumericStatType(), currentValue, baseValue);

        switch (statState)
        {
            case NumericStatState.Positive:
                SetValueTextColor(positiveColor);
                break;
            case NumericStatState.Neutral:
                SetValueTextColor(neutralColor);
                break;
            case NumericStatState.Negative:
                SetValueTextColor(negativeColor);
                break;
        }

        string processedValueText = MappingUtilities.ProcessNumericStatValueToString(GetNumericStatType(), currentValue);
        SetValueText(processedValueText);
    }

    protected void SetValueText(string text) => valueText.text = text;
    protected void SetValueTextColor(Color color) => valueText.color = color;

    protected abstract float GetCurrentValue();
    protected abstract float GetBaseValue();
    #endregion

    protected void FindResolverLogic(Transform playerTransform)
    {
        resolver = playerTransform.GetComponentInChildren<T>();

        if (resolver == null)
        {
            if (debug) Debug.Log("Could not find resolver.");
            return;
        }

        SubscribeToEvents();
    }

    protected abstract void SubscribeToEvents();
    protected abstract void UnSubscribeToEvents();

    #region Subscriptions
    private void PlayerInstantiationHandler_OnPlayerInstantiation(object sender, PlayerInstantiationHandler.OnPlayerInstantiationEventArgs e)
    {
        FindResolverLogic(e.playerTransform);
    }
    #endregion
}
