using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VolumeManager;

public class DialoguesVolumeManager : VolumeManager
{
    public static DialoguesVolumeManager Instance { get; private set; }

    private const string DIALOGUES_VOLUME = "DialoguesVolume";

    public static event EventHandler OnDialoguesVolumeManagerInitialized;
    public static event EventHandler<OnVolumeChangedEventArgs> OnDialoguesVolumeChanged;

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
            //Debug.LogWarning("There is more than one DialoguesVolumeManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
    protected override string GetVolumePropertyName() => DIALOGUES_VOLUME;
    protected override void OnVolumeManagerInitialized(float volume)
    {
        OnDialoguesVolumeManagerInitialized?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnVolumeChanged(float volume)
    {
        OnDialoguesVolumeChanged?.Invoke(this, new OnVolumeChangedEventArgs { newVolume = volume });
    }
}
