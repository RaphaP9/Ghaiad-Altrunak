using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicsVolumeManager : VolumeManager
{
    public static CinematicsVolumeManager Instance { get; private set; }

    private const string CINEMATICS_VOLUME = "CinematicsVolume";

    public static event EventHandler OnCinematicsVolumeManagerInitialized;
    public static event EventHandler<OnVolumeChangedEventArgs> OnCinematicsVolumeChanged;

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
            //Debug.LogWarning("There is more than one CinematicsVolumeManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override string GetVolumePropertyName() => CINEMATICS_VOLUME;
    protected override void OnVolumeManagerInitialized(float volume)
    {
        OnCinematicsVolumeManagerInitialized?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnVolumeChanged(float volume)
    {
        OnCinematicsVolumeChanged?.Invoke(this, new OnVolumeChangedEventArgs { newVolume = volume });
    }
}
