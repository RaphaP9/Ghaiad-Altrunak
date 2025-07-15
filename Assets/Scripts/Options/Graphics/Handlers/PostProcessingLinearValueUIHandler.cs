using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PostProcessingLinearValueUIHandler : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] protected Button increaseIntensityButton;
    [SerializeField] protected Button decreaseIntensityButton;

    [Header("Components")]
    [SerializeField] List<OptionBarUI> optionBarUIs;

    protected const float INTENSITY_BUTTON_CHANGE = 0.1f;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        increaseIntensityButton.onClick.AddListener(IncreaseIntensityByButton);
        decreaseIntensityButton.onClick.AddListener(DecreaseIntensityByButton);

        IntializeOptionBarUIs();
    }

    private void Start()
    {
        UpdateVisual();
    }

    protected abstract PostProcessingLinearValueManager GetPostProcessingManager();

    protected void IntializeOptionBarUIs()
    {
        foreach (OptionBarUI optionBarUI in optionBarUIs)
        {
            optionBarUI.BackgroundButton.onClick.AddListener(() => GetPostProcessingManager().ChangeIntensity(optionBarUI.BarValue));
        }
    }

    private void IncreaseIntensityByButton()
    {
        float currentIntensity = GetPostProcessingManager().GetNormalizedIntensity();
        float desiredIntensity = currentIntensity + INTENSITY_BUTTON_CHANGE;

        desiredIntensity = GeneralUtilities.RoundToNDecimalPlaces(desiredIntensity, 1);

        if (desiredIntensity > GetPostProcessingManager().GetMaxNormalizedIntensity()) return;

        GetPostProcessingManager().ChangeIntensity(desiredIntensity);
    }

    private void DecreaseIntensityByButton()
    {
        float currentIntensity = GetPostProcessingManager().GetNormalizedIntensity();
        float desiredIntensity = currentIntensity - INTENSITY_BUTTON_CHANGE;

        desiredIntensity = GeneralUtilities.RoundToNDecimalPlaces(desiredIntensity, 1);

        if (desiredIntensity < GetPostProcessingManager().GetMinNormalizedIntensity()) return;

        GetPostProcessingManager().ChangeIntensity(desiredIntensity);
    }

    protected void UpdateVisual()
    {
        HideAllOptionBars();
        float currentValue = GeneralUtilities.RoundToNDecimalPlaces(GetPostProcessingManager().GetNormalizedIntensity(), 1);

        foreach (OptionBarUI optionBarUI in optionBarUIs)
        {
            if (optionBarUI.BarValue <= currentValue) optionBarUI.ShowActiveIndicator();
            else optionBarUI.HideActiveIndicator();
        }
    }

    protected void HideAllOptionBars()
    {
        foreach (OptionBarUI optionBarUI in optionBarUIs)
        {
            optionBarUI.HideActiveIndicator();
        }
    }
}
