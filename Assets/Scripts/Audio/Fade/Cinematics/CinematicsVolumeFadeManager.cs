using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicsVolumeFadeManager : VolumeFadeManager
{
    public static CinematicsVolumeFadeManager Instance { get; private set; }

    private const string CINEMATICS_FADE_VOLUME = "CinematicsFadeVolume";

    protected override void Awake()
    {
        base.Awake();
        SetFadeVolumeKey(CINEMATICS_FADE_VOLUME);
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogWarning("There is more than one CinematicsVolumeFadeManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
}

