using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public abstract class VolumeFadeManager : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField] private AudioMixer masterAudioMixer;

    [Header("States")]
    [SerializeField] private State volumeFadeState;

    public enum State { Muted, Idle, FadingIn, FadingOut }

    private const float MAX_VOLUME = 1f;
    private const float MIN_VOLUME = 0.0001f;
    private const float INITIAL_VOLUME = 1f;

    private string fadeVolumeKey;

    protected virtual void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        SetVolumeFadeState(State.Idle);
        InitializeVolume();
    }

    protected abstract void SetSingleton();

    protected void SetFadeVolumeKey(string key) => fadeVolumeKey = key;

    private void SetVolumeFadeState(State state) => volumeFadeState = state;

    private void InitializeVolume() => ChangeVolume(INITIAL_VOLUME);

    private void ChangeVolume(float volume)
    {
        volume = volume < GetMinVolume() ? GetMinVolume() : volume;
        volume = volume > GetMaxVolume() ? GetMaxVolume() : volume;

        masterAudioMixer.SetFloat(fadeVolumeKey, Mathf.Log10(volume) * 20);
    }

    private float GetLogarithmicVolume()
    {
        masterAudioMixer.GetFloat(fadeVolumeKey, out float logarithmicVolume);
        return logarithmicVolume;
    }

    private float GetLinearVolume()
    {
        float logarithmicVolume = GetLogarithmicVolume();
        float linearVolume = Mathf.Pow(10f, logarithmicVolume / 20f);
        return linearVolume;
    }

    public float GetMaxVolume() => MAX_VOLUME;
    public float GetMinVolume() => MIN_VOLUME;

    #region Methods
    public void FadeOutVolume(float fadeOutTime)
    {
        if (volumeFadeState == State.FadingOut) return;
        if (volumeFadeState == State.Muted) return;

        StopAllCoroutines();
        StartCoroutine(FadeOutVolumeCoroutine(fadeOutTime));
    }

    public void FadeInVolume(float fadeInTime)
    {
        if (volumeFadeState == State.FadingIn) return;
        if (volumeFadeState == State.Idle) return;

        StopAllCoroutines();
        StartCoroutine(FadeInVolumeCoroutine(fadeInTime));
    }
    #endregion

    #region Coroutines
    public IEnumerator FadeOutVolumeCoroutine(float fadeOutTime)
    {
        SetVolumeFadeState(State.FadingOut);

        float initialVolume = GetLinearVolume();
        float realFadeOutTime = initialVolume * fadeOutTime;
        float time = 0;

        while (time < realFadeOutTime)
        {
            ChangeVolume(initialVolume * (1 - time / realFadeOutTime));
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        ChangeVolume(MIN_VOLUME);

        SetVolumeFadeState(State.Muted);
    }

    public IEnumerator FadeInVolumeCoroutine(float fadeInTime)
    {
        SetVolumeFadeState(State.FadingIn);

        float initialVolume = GetLinearVolume();
        float realFadeInTime = (1 - initialVolume) * fadeInTime;
        float time = 0;

        while (time < realFadeInTime)
        {
            ChangeVolume(initialVolume + (MAX_VOLUME - initialVolume) * time / realFadeInTime);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        ChangeVolume(MAX_VOLUME);

        SetVolumeFadeState(State.Idle);
    }

    #endregion
}
