using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected SFXPool SFXPool;

    [Header("Settings")]
    [SerializeField] protected bool pauseOnPause;

    [Header("AudioSource Settings")]
    [SerializeField] protected AudioMixerGroup audioMixerGroup;
    [SerializeField, Range(1f,100f)] protected float minDistance = 1f;
    [SerializeField, Range(1f, 100f)] protected float maxDistance = 1f;
    [SerializeField, Range(0f, 1f)] protected float spatialBlendFactor;
    [SerializeField] protected AudioRolloffMode audioRolloffMode;

    [Header("Debug")]
    [SerializeField] protected bool debug;

    protected AudioSource audioSource;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected void PlaySound(AudioClip[] audioClipArray)
    {
        if (audioClipArray.Length == 0)
        {
            if (debug) Debug.Log("SFX play will be ignored, audioClipArray lenght is 0!");
            return;
        }

        AudioClip audioClip = audioClipArray[Random.Range(0, audioClipArray.Length)];
        PlaySound(audioClip);
    }
    protected void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    protected void PlaySoundAtPoint(AudioClip[] audioClipArray, Vector2 position)
    {
        if (audioClipArray.Length == 0)
        {
            if (debug) Debug.Log("SFX play will be ignored, audioClipArray lenght is 0!");
            return;
        }

        AudioClip audioClip = audioClipArray[Random.Range(0, audioClipArray.Length)];
        PlaySoundAtPoint(audioClip, position);
    }

    protected void PlaySoundAtPoint(AudioClip audioClip, Vector2 position)
    {
        GameObject sfxGameObject = new GameObject("TempSFX");
        sfxGameObject.transform.position = position;

        AudioSource tempAudioSource = sfxGameObject.AddComponent<AudioSource>();
        TemporalSFXController temporalSFXController = sfxGameObject.AddComponent<TemporalSFXController>();

        temporalSFXController.SetPausable(pauseOnPause);

        tempAudioSource.clip = audioClip;
        tempAudioSource.outputAudioMixerGroup = audioMixerGroup;

        tempAudioSource.spatialBlend = spatialBlendFactor; 
        tempAudioSource.minDistance = minDistance; 
        tempAudioSource.maxDistance = maxDistance; 
        tempAudioSource.rolloffMode = audioRolloffMode; 

        tempAudioSource.Play();

        Destroy(sfxGameObject, audioClip.length);
    }
}
