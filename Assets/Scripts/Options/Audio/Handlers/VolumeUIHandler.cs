using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class VolumeUIHandler : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] protected Button increaseVolumeButton;
    [SerializeField] protected Button decreaseVolumeButton;

    [Header("Components")]
    [SerializeField] List<OptionBarUI> optionBarUIs;

    protected const float VOLUME_BUTTON_CHANGE = 0.1f;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        increaseVolumeButton.onClick.AddListener(IncreaseVolumeByButton);
        decreaseVolumeButton.onClick.AddListener(DecreaseVolumeByButton);

        IntializeOptionBarUIs();
    }

    private void Start()
    {
        UpdateVisual();
    }

    protected abstract VolumeManager GetVolumeManager();

    protected void IntializeOptionBarUIs()
    {
        foreach(OptionBarUI optionBarUI in optionBarUIs)
        {
            optionBarUI.BackgroundButton.onClick.AddListener(() => GetVolumeManager().ChangeVolume(optionBarUI.BarValue));
        }
    }

    private void IncreaseVolumeByButton()
    {
        float currentVolume = GetVolumeManager().GetLinearVolume();
        float desiredVolume = currentVolume + VOLUME_BUTTON_CHANGE;

        if (desiredVolume > GetVolumeManager().GetMaxVolume()) return;

        desiredVolume = GeneralUtilities.RoundToNDecimalPlaces(desiredVolume, 1);
        GetVolumeManager().ChangeVolume(desiredVolume);
    }

    private void DecreaseVolumeByButton()
    {
        float currentVolume = GetVolumeManager().GetLinearVolume();
        float desiredVolume = currentVolume - VOLUME_BUTTON_CHANGE;

        if (desiredVolume < 0f) return;

        desiredVolume = GeneralUtilities.RoundToNDecimalPlaces(desiredVolume, 1);
        GetVolumeManager().ChangeVolume(desiredVolume);
    }

    protected void UpdateVisual()
    {
        HideAllOptionBars();
        float currentValue = GeneralUtilities.RoundToNDecimalPlaces(GetVolumeManager().GetLinearVolume(),1);

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

