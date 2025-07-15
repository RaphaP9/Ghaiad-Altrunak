using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TemporalSFXController : MonoBehaviour
{
    private bool pausable;
    public bool Pausable => pausable;

    public void SetPausable(bool pausable) => this.pausable = pausable;

    private AudioSource audioSource;

    private void OnEnable()
    {
        AudioPauseHandler.OnPauseAudio += AudioPauseHandler_OnPauseAudio;
        AudioPauseHandler.OnResumeAudio += AudioPauseHandler_OnResumeAudio;
    }
    private void OnDisable()
    {
        AudioPauseHandler.OnPauseAudio -= AudioPauseHandler_OnPauseAudio;
        AudioPauseHandler.OnResumeAudio -= AudioPauseHandler_OnResumeAudio;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void CheckPauseAudio()
    {
        if (!pausable) return;

        audioSource.Pause();
    }

    private void CheckResumeAudio()
    {
        if (!pausable) return;

        audioSource.UnPause();
    }

    private void AudioPauseHandler_OnPauseAudio(object sender, System.EventArgs e)
    {
        CheckPauseAudio();
    }

    private void AudioPauseHandler_OnResumeAudio(object sender, System.EventArgs e)
    {
        CheckResumeAudio();
    }
}
