using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class MusicVolumeManager : VolumeManager
{
    public static MusicVolumeManager Instance { get; private set; }

    private const string MUSIC_VOLUME = "MusicVolume";

    public static event EventHandler OnMusicVolumeManagerInitialized;
    public static event EventHandler<OnVolumeChangedEventArgs> OnMusicVolumeChanged;

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
            //Debug.LogWarning("There is more than one MusicVolumeManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
    protected override string GetVolumePropertyName() => MUSIC_VOLUME;
    protected override void OnVolumeManagerInitialized(float volume)
    {
        OnMusicVolumeManagerInitialized?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnVolumeChanged(float volume)
    {
        OnMusicVolumeChanged?.Invoke(this, new OnVolumeChangedEventArgs { newVolume = volume });
    }
}
