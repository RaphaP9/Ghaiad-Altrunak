using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPauseHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private List<AudioSource> audioSourcesToPause;

    [Header("Enabler")]
    [SerializeField] private bool enable;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public static event EventHandler OnPauseAudio;
    public static event EventHandler OnResumeAudio;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void PauseAudioSources()
    {
        foreach (AudioSource audioSource in audioSourcesToPause)
        {
            audioSource.Pause();
        }
    }

    private void UnPauseAudioSources()
    {
        foreach (AudioSource audioSource in audioSourcesToPause)
        {
            audioSource.UnPause();
        }
    }

    private void PauseGlobalSFX()
    {
        if (!enable) return;
        PauseAudioSources();
    }

    private void ResumeGlobalSFX()
    {
        if (!enable) return;
        UnPauseAudioSources();
    }
 
    private void PauseManager_OnGamePaused(object sender, System.EventArgs e)
    {
        PauseGlobalSFX();
        OnPauseAudio?.Invoke(this, EventArgs.Empty);
    }

    private void PauseManager_OnGameResumed(object sender, System.EventArgs e)
    {
        ResumeGlobalSFX();
        OnResumeAudio?.Invoke(this, EventArgs.Empty);
    }
}
