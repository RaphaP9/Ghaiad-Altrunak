using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class SFXVolumeManager : VolumeManager
{
    public static SFXVolumeManager Instance { get; private set; }

    private const string SFX_VOLUME = "SFXVolume";

    public static event EventHandler OnSFXVolumeManagerInitialized;
    public static event EventHandler<OnVolumeChangedEventArgs> OnSFXVolumeChanged;

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogWarning("There is more than one SFXVolumeManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override string GetVolumePropertyName() => SFX_VOLUME;
    protected override void OnVolumeManagerInitialized(float volume)
    {
        OnSFXVolumeManagerInitialized?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnVolumeChanged(float volume)
    {
        OnSFXVolumeChanged?.Invoke(this, new OnVolumeChangedEventArgs { newVolume = volume });
    }
}
