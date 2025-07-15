using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System;

public class BrightnessIntensityManager : PostProcessingLinearValueManager
{
    public static BrightnessIntensityManager Instance { get; private set; }

    private ColorAdjustments colorAdjustments;

    public static event EventHandler OnBrightnessIntensityManagerInitialized;
    public static event EventHandler<OnIntensityChangedEventArgs> OnBrightnessIntensityChanged;

    private const float MAX_INTENSITY = 0.5f;
    private const float MIN_INTENSITY = -0.5f;

    private const float DEFAULT_NORMALIZED_INTENSITY = 0.5f;

    private void Awake()
    {
        SetSingleton();
        InitializeSetting();
        SetDefaultNormalizedIntensity(DEFAULT_NORMALIZED_INTENSITY);
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogWarning("There is more than one BrightnessIntensityManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void InitializeSetting()
    {
        if (!volumeProfile.TryGet(out colorAdjustments))
        {
            settingFound = false;
            Debug.Log("Color Adjustments settings not found in the Post Process Volume");
        }
    }

    protected override void OnPostProcesingManagerInitialized(float intensity)
    {
        OnBrightnessIntensityManagerInitialized?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnIntensityChanged(float intensity)
    {
        OnBrightnessIntensityChanged?.Invoke(this, new OnIntensityChangedEventArgs { newIntensity = intensity });
    }

    protected override void SetIntensity(float intensity) => colorAdjustments.postExposure.value = intensity;

    protected override float GetIntensity() => colorAdjustments.postExposure.value;
    public override float GetMaxIntensity() => MAX_INTENSITY;
    public override float GetMinIntensity() => MIN_INTENSITY;
}
