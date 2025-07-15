using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVolumeFadeManager : VolumeFadeManager
{
    public static MusicVolumeFadeManager Instance { get; private set; }

    private const string MUSIC_FADE_VOLUME = "MusicFadeVolume";

    protected override void Awake()
    {
        base.Awake();
        SetFadeVolumeKey(MUSIC_FADE_VOLUME);
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogWarning("There is more than one MusicVolumeFadeManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
}
