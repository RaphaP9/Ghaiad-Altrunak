using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;

public abstract class PostProcessingLinearValueManager : MonoBehaviour
{
    [Header("Intensity Settings")]
    [SerializeField] protected VolumeProfile volumeProfile;
    [SerializeField, Range(0f, 1f)] protected float initialNormalizedIntensity;

    [Header("Load Settings")]
    [SerializeField] protected string playerPrefsKey;

    protected const float MAX_NORMALIZED_INTENSITY = 1f;
    protected const float MIN_NORMALIZED_INTENSITY = 0f;

    protected float defaultNormalizedIntensity;

    protected bool settingFound = true;

    public class OnIntensityChangedEventArgs : EventArgs
    {
        public float newIntensity;
    }

    private void Start()
    {
        LoadIntensityPlayerPrefs();
        InitializeIntensity();
    }

    protected void LoadIntensityPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey(playerPrefsKey))
        {
            PlayerPrefs.SetFloat(playerPrefsKey, defaultNormalizedIntensity);
        }

        initialNormalizedIntensity = PlayerPrefs.GetFloat(playerPrefsKey);
    }

    protected void SaveIntensityPlayerPrefs(float intensity)
    {
        PlayerPrefs.SetFloat(playerPrefsKey, intensity);
    }

    protected virtual void InitializeIntensity()
    {
        ChangeIntensity(initialNormalizedIntensity);
        OnPostProcesingManagerInitialized(initialNormalizedIntensity);
    }
        
    public virtual void ChangeIntensity(float normalizedIntensity)
    {

        normalizedIntensity = normalizedIntensity < GetMinNormalizedIntensity() ? GetMinNormalizedIntensity() : normalizedIntensity;
        normalizedIntensity = normalizedIntensity > GetMaxNormalizedIntensity() ? GetMaxNormalizedIntensity() : normalizedIntensity;

        SaveIntensityPlayerPrefs(normalizedIntensity);

        if (settingFound) SetIntensityNormalized(normalizedIntensity);

        OnIntensityChanged(normalizedIntensity);
    }


    protected abstract void OnPostProcesingManagerInitialized(float intensity);
    protected abstract void OnIntensityChanged(float intensity);


    protected abstract void InitializeSetting();

    public float GetMaxNormalizedIntensity() => MAX_NORMALIZED_INTENSITY;
    public float GetMinNormalizedIntensity() => MIN_NORMALIZED_INTENSITY;
    protected abstract float GetIntensity();
    public abstract float GetMaxIntensity();
    public abstract float GetMinIntensity();

    protected void SetIntensityNormalized(float normalizedIntensity) => SetIntensity(GetMinIntensity() + normalizedIntensity * (GetMaxIntensity() - GetMinIntensity()));
    protected void SetDefaultNormalizedIntensity(float defaultNormalizedIntensity) => this.defaultNormalizedIntensity = defaultNormalizedIntensity; 
    protected abstract void SetIntensity(float intensity);
    public float GetNormalizedIntensity() => (GetIntensity() - GetMinIntensity()) / (GetMaxIntensity()-GetMinIntensity());
}
