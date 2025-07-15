using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class MasterVolumeManager : VolumeManager
{
    public static MasterVolumeManager Instance { get; private set; }

    private const string MASTER_VOLUME = "MasterVolume";

    public static event EventHandler OnMasterVolumeManagerInitialized;
    public static event EventHandler<OnVolumeChangedEventArgs> OnMasterVolumeChanged;

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
            //Debug.LogWarning("There is more than one MasterVolumeManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
    protected override string GetVolumePropertyName() => MASTER_VOLUME;
    protected override void OnVolumeManagerInitialized(float volume)
    {
        OnMasterVolumeManagerInitialized?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnVolumeChanged(float volume)
    {
        OnMasterVolumeChanged?.Invoke(this, new OnVolumeChangedEventArgs { newVolume = volume });
    }
}
