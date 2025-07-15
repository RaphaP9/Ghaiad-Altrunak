using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public abstract class VolumeManager : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField] protected AudioMixer masterAudioMixer;
    [SerializeField, Range(0f, 1f)] protected float initialVolume;

    private const float MAX_VOLUME = 1f;
    private const float MIN_VOLUME = 0.0001f;

    [Header("Load Settings")]
    [SerializeField] private string playerPrefsKey;

    public class OnVolumeChangedEventArgs: EventArgs
    {
        public float newVolume;
    }

    private void Start()
    {
        LoadVolumePlayerPrefs();
        InitializeVolume();
    }

    protected void LoadVolumePlayerPrefs()
    {
        if (!PlayerPrefs.HasKey(playerPrefsKey))
        {
            PlayerPrefs.SetFloat(playerPrefsKey, initialVolume);
        }

        initialVolume = PlayerPrefs.GetFloat(playerPrefsKey);
    }

    protected void SaveVolumePlayerPrefs(float volume)
    {
        PlayerPrefs.SetFloat(playerPrefsKey, volume);
    }

    protected abstract string GetVolumePropertyName();
    protected abstract void OnVolumeManagerInitialized(float volume);
    protected abstract void OnVolumeChanged(float volume);

    protected virtual void InitializeVolume()
    {
        ChangeVolume(initialVolume);
        OnVolumeManagerInitialized(initialVolume);
    }

    public virtual void ChangeVolume(float volume)
    {
        volume = volume < GetMinVolume() ? GetMinVolume() : volume;
        volume = volume > GetMaxVolume() ? GetMaxVolume() : volume;

        masterAudioMixer.SetFloat(GetVolumePropertyName(), Mathf.Log10(volume) * 20);
        SaveVolumePlayerPrefs(volume);

        OnVolumeChanged(volume);
    }

    protected virtual float GetLogarithmicVolume()
    {
        masterAudioMixer.GetFloat(GetVolumePropertyName(), out float logarithmicVolume);
        return logarithmicVolume;
    }

    public float GetLinearVolume()
    {
        float logarithmicVolume = GetLogarithmicVolume();
        float linearVolume = Mathf.Pow(10f, logarithmicVolume / 20f);
        return linearVolume;
    }

    public float GetMaxVolume() => MAX_VOLUME;
    public float GetMinVolume() => MIN_VOLUME;
}
