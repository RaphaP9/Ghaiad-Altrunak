using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryObjectNumericStatUI : MonoBehaviour
{
    [Header("Runtime Filled")]
    [SerializeField] private NumericEmbeddedStat numericEmbeddedStat;

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [Header("Settings")]
    [SerializeField] protected Color positiveColor;
    [SerializeField] protected Color neutralColor;
    [SerializeField] protected Color negativeColor;


    public void SetNumericEmbededStat(NumericEmbeddedStat numericEmbeddedStat)
    {
        this.numericEmbeddedStat = numericEmbeddedStat;

        SetStatValueText(numericEmbeddedStat);
        SetStatNameText(numericEmbeddedStat);
    }

    private void SetStatNameText(NumericEmbeddedStat numericEmbeddedStat) => statNameText.text = MappingUtilities.MapNumericStatType(numericEmbeddedStat.numericStatType);

    private void SetStatValueText(NumericEmbeddedStat numericEmbeddedStat)
    {
        NumericStatState statState = MappingUtilities.GetNumericStatState(numericEmbeddedStat.numericStatType, numericEmbeddedStat.value, 0f); //Base Value is always 0

        switch (statState)
        {
            case NumericStatState.Positive:
                SetStatValueTextColor(positiveColor);
                break;
            case NumericStatState.Neutral:
                SetStatValueTextColor(neutralColor);
                break;
            case NumericStatState.Negative:
                SetStatValueTextColor(negativeColor);
                break;
        }

        if (numericEmbeddedStat.numericStatModificationType == NumericStatModificationType.Replacement) SetStatValueTextColor(neutralColor); //Replacement Values Are Always Neutral

        string processedValueText = MappingUtilities.ProcessObjectNumericStatValueToString(numericEmbeddedStat.numericStatType, numericEmbeddedStat.numericStatModificationType, numericEmbeddedStat.value);

        SetStatValueText(processedValueText);
    }

    private void SetStatValueText(string text) => statValueText.text = text;
    protected void SetStatValueTextColor(Color color) => statValueText.color = color;
}
